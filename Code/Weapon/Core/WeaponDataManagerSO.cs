using System.Collections.Generic;
using UnityEngine;

namespace HN.Code.Weapon.Core
{
    [CreateAssetMenu(menuName = "SO/Weapon/WeaponManager", order = 2)]
    public class WeaponDataManagerSO : ScriptableObject
    {
        public List<WeaponDataSO> weaponDataList;
        public string folderPath;
    }
}