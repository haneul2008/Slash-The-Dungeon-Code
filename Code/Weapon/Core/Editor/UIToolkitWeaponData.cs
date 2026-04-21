using System;
using UnityEngine.UIElements;

namespace HN.Code.Weapon.Core.Editor
{
    public class UIToolkitWeaponData
    {
        public event Action<UIToolkitWeaponData> OnDelete; 
        public event Action<UIToolkitWeaponData> OnSelect; 
        
        public WeaponDataSO WeaponData { get; private set; }

        private VisualElement _root;
        private Label _weaponName;
        
        public string Name
        {
            get => _weaponName.text;
            set => _weaponName.text = value;
        }
        
        public bool IsActive
        {
            get => _root.ClassListContains("select");
            set
            {
                if(value)
                    _root.AddToClassList("select");
                else
                    _root.RemoveFromClassList("select");
            }
        }
        
        public UIToolkitWeaponData(VisualElement root, WeaponDataSO weaponData)
        {
            WeaponData = weaponData;
            
            _root = root.Q("Container");

            Button deleteBtn = root.Q<Button>("DeleteBtn");
            VisualElement weaponImage = root.Q("Image");
            _weaponName = root.Q<Label>("WeaponName");
            
            if(weaponData.sprite != null)
            {
                var texture = new StyleBackground(weaponData.sprite);
                weaponImage.style.backgroundImage = texture;
            }

            _root.RegisterCallback<ClickEvent>(evt => OnSelect?.Invoke(this));
            deleteBtn.RegisterCallback<ClickEvent>(evt => OnDelete?.Invoke(this));
            
            _weaponName.text = weaponData.weaponName;
        }
    }
}