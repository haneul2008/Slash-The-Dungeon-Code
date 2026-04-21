using System;
using System.Collections.Generic;
using UnityEngine;

namespace HN.Code.Weapon.Core
{
    public class PlayerWeaponCompo : MonoBehaviour
    {
        [SerializeField] private WeaponDataManagerSO weaponListSo;
        
        public IWeaponLogic CurrentWeaponLogic { get; private set; }

        private readonly Dictionary<WeaponEnum, IWeaponLogic> _weaponParis = new Dictionary<WeaponEnum, IWeaponLogic>();

        private void Awake()
        {
            InitDictionaryAndLogic();
        }

        private void InitDictionaryAndLogic()
        {
            weaponListSo.weaponDataList.ForEach(data =>
            {
                if (Enum.TryParse(data.weaponName, out WeaponEnum weaponEnum))
                {
                    Type type = Type.GetType(data.className);

                    Debug.Assert(type != null, $"{type} is null");
                    
                    IWeaponLogic logic = Activator.CreateInstance(type) as IWeaponLogic;
                    
                    logic?.Initialize(data);
                    
                    _weaponParis.Add(weaponEnum, logic);
                }
                else
                {
                    Debug.LogWarning($"{data.name}'s enum is null");
                }
            });
        }

        public void HandleShootKeyPressed()
        {
            CurrentWeaponLogic?.Shoot();
        }

        private void Update()
        {
            CurrentWeaponLogic?.Update();
        }

        public void ChangeWeapon(WeaponEnum weaponEnum)
        {
            CurrentWeaponLogic?.Exit();

            IWeaponLogic newLogic = _weaponParis.GetValueOrDefault(weaponEnum);
            
            Debug.Assert(newLogic is not null, $"{weaponEnum} logic is null");

            CurrentWeaponLogic = newLogic;
            CurrentWeaponLogic?.Enter();
        }
    }
}