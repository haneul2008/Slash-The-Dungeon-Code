using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace HN.Code.Weapon.Core.Editor
{
    [CustomEditor(typeof(WeaponDataManagerSO))]
    public class WeaponDataManagerWindow : UnityEditor.EditorWindow
    {
        [SerializeField] private VisualTreeAsset weaponDataManagerAsset = default;
        [SerializeField] private VisualTreeAsset weaponDataAsset = default;
        [SerializeField] private WeaponDataManagerSO weaponDataManager;

        private ScrollView _weaponDataView;
        private UIToolkitWeaponData _currentWeapon;
        private WeaponDataInspector _weaponInspector;
        private List<UIToolkitWeaponData> _weaponList = new List<UIToolkitWeaponData>();
        
        [MenuItem("Weapons/WeaponManager")]
        public static void ShowWindow()
        {
            WeaponDataManagerWindow wnd = GetWindow<WeaponDataManagerWindow>();
            wnd.titleContent = new GUIContent("ItemManager");
            wnd.minSize = new Vector2(700, 600);
        }

        private void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            VisualElement content = weaponDataManagerAsset.Instantiate();
            content.style.flexGrow = 1;
            root.Add(content);

            Initialize(content);
            GenerateUI();
        }

        private void Initialize(VisualElement root)
        {
            Button createBtn = root.Q<Button>("CreateBtn");
            createBtn.clicked += HandleCreateWeaponData;

            _weaponDataView = root.Q<ScrollView>("DataContainer");

            _weaponInspector = new WeaponDataInspector(root, this);
            _weaponInspector.OnInfoChanged += HandleInfoChanged;
        }

        private void HandleInfoChanged(WeaponDataSO weaponData)
        {
            InfoData newInfo = _weaponInspector.GetInfo();

            string newName = newInfo.weaponName;
            
            string weaponPath = AssetDatabase.GetAssetPath(weaponData);

            bool exist = weaponDataManager.weaponDataList.Any(weapon => weapon.weaponName == newName);
            bool isNameChanged = _currentWeapon.WeaponData.weaponName != newName;
            
            if (isNameChanged && exist)
            {
                EditorUtility.DisplayDialog(
                    "Duplicated name!",
                    $"Given asset name {newName} is already exist",
                    "OK");
                return;
            }
            
            AssetDatabase.RenameAsset(weaponPath, $"{newName}_weapon");

            weaponData.weaponName = newName;
            weaponData.firePos = newInfo.firePos;
            weaponData.sprite = newInfo.sprite;
            weaponData.className = newInfo.className;
            weaponData.damage = newInfo.damage;
            
            EditorUtility.SetDirty(weaponData);
            AssetDatabase.SaveAssets();

            GenerateUI();
        }

        private void HandleCreateWeaponData()
        {
            Guid guid = Guid.NewGuid();
            WeaponDataSO weaponData = ScriptableObject.CreateInstance<WeaponDataSO>();
            weaponData.weaponName = guid.ToString();
            
            string weaponFileName = $"{weaponData.weaponName}_weapon.asset";
            string folderPath = weaponDataManager.folderPath;
            
            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            
            AssetDatabase.CreateAsset(weaponData, $"{folderPath}/{weaponFileName}");
            weaponDataManager.weaponDataList.Add(weaponData);
            
            EditorUtility.SetDirty(weaponDataManager);
            AssetDatabase.SaveAssets();

            GenerateUI();
        }

        private void GenerateUI()
        {
            _weaponDataView.Clear();
            _weaponList.Clear();

            foreach (WeaponDataSO weapon in weaponDataManager.weaponDataList)
            {
                AddWeaponView(weapon);
            }
        }

        private void AddWeaponView(WeaponDataSO weapon)
        {
            var itemUIAsset = weaponDataAsset.Instantiate();
            UIToolkitWeaponData uiToolkitWeapon = new UIToolkitWeaponData(itemUIAsset, weapon);

            _weaponDataView.Add(itemUIAsset);
            _weaponList.Add(uiToolkitWeapon);

            uiToolkitWeapon.Name = weapon.weaponName;
            uiToolkitWeapon.OnSelect += HandleSelectItem;
            uiToolkitWeapon.OnDelete += HandleDeleteItem;
        }

        private void HandleDeleteItem(UIToolkitWeaponData weapon)
        {
            if (EditorUtility.DisplayDialog(
                    "Delete Item", $"{weapon.Name}을 지우시겠습니까?", "Yes", "No"))
            {
                weaponDataManager.weaponDataList.Remove(weapon.WeaponData);

                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(weapon.WeaponData));
                EditorUtility.SetDirty(weaponDataManager);

                AssetDatabase.SaveAssets();

                if (_currentWeapon == weapon)
                {
                    _currentWeapon = null;

                    _weaponInspector.ClearInspector();
                }

                GenerateUI();
            }
        }

        private void HandleSelectItem(UIToolkitWeaponData weapon)
        {
            _weaponList.ForEach(item => item.IsActive = false);
            weapon.IsActive = true;
            _currentWeapon = weapon;
            _weaponInspector.UpdateInspector(weapon.WeaponData);
        }
    }
}
