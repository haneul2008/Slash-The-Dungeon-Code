using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace HN.Code.Weapon.Core.Editor
{
    internal struct InfoData
    {
        public string weaponName;
        public Vector2 firePos;
        public string className;
        public Sprite sprite;
        public int damage;
    }
    
    public class WeaponDataInspector
    {
        public Action<WeaponDataSO> OnInfoChanged;

        private WeaponDataManagerWindow _window;

        private TextField _nameField;
        private Vector2Field _firePosField;
        private DropdownField _classNameDropdown;
        private ObjectField _spriteField;
        private IntegerField _damageField;

        private WeaponDataSO _targetWeapon;

        public WeaponDataInspector(VisualElement content, WeaponDataManagerWindow window)
        {
            _window = window;

            _nameField = content.Q<TextField>("WeaponNameField");
            _firePosField = content.Q<Vector2Field>("FirePosField");
            _spriteField = content.Q<ObjectField>("SpriteField");
            _classNameDropdown = content.Q<DropdownField>("ClassNameField");
            _damageField = content.Q<IntegerField>("DamageField");
            Label classNameLabel = content.Q<Label>("ClassNameLabel");
            Button infoChangeBtn = content.Q<Button>("InfoChangeBtn");

            _classNameDropdown.RegisterValueChangedCallback(evt => classNameLabel.text = _classNameDropdown.value);
            infoChangeBtn.clicked += () => OnInfoChanged?.Invoke(_targetWeapon);
            
            SetClassNameField();
        }

        public void UpdateInspector(WeaponDataSO weapon)
        {
            _nameField.SetValueWithoutNotify(weapon.weaponName);
            _firePosField.SetValueWithoutNotify(weapon.firePos);
            _spriteField.SetValueWithoutNotify(weapon.sprite);
            _damageField.SetValueWithoutNotify(weapon.damage);
            _classNameDropdown.value = weapon.className;
            _targetWeapon = weapon;
        }

        public void ClearInspector()
        {
            _nameField.SetValueWithoutNotify(string.Empty);
            _spriteField.SetValueWithoutNotify(null);
            _firePosField.SetValueWithoutNotify(Vector2.zero);
            _damageField.SetValueWithoutNotify(0);
            _classNameDropdown.choices = new List<string>();
            _targetWeapon = null;
        }
        
        private void SetClassNameField()
        {
            List<string> classNames = Assembly.GetAssembly(typeof(IWeaponLogic))
                .GetTypes()
                .Where(t => typeof(IWeaponLogic).IsAssignableFrom(t) && !t.IsInterface).Select(t => t.FullName)
                .ToList();

            _classNameDropdown.choices = classNames;
        }

        internal InfoData GetInfo()
        {
            return new InfoData()
            {
                weaponName = _nameField.value,
                firePos = _firePosField.value,
                sprite = _spriteField.value as Sprite,
                className = _classNameDropdown.value,
                damage = _damageField.value
            };
        }
    }
}