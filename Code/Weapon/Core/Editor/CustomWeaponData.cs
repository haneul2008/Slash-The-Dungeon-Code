using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace HN.Code.Weapon.Core.Editor
{
    [CustomEditor(typeof(WeaponDataSO))]
    public class CustomWeaponData : UnityEditor.Editor
    {
        public VisualTreeAsset treeAsset = default;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            treeAsset.CloneTree(root);

            ObjectField spriteField = root.Q<ObjectField>("SpriteField");
            spriteField.objectType = typeof(Sprite);

            List<string> classNames = Assembly.GetAssembly(typeof(IWeaponLogic))
                .GetTypes()
                .Where(t => typeof(IWeaponLogic).IsAssignableFrom(t) && !t.IsInterface).Select(t => t.FullName)
                .ToList();
            
            DropdownField classNameField = root.Q<DropdownField>("ClassNameField");
            classNameField.choices = classNames;

            return root;
        }
    }
}