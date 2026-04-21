using UnityEngine;

namespace HN.Code.Weapon.Core
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "SO/Weapon/WeaponData", order = 0)]
    public class WeaponDataSO : ScriptableObject
    {
        public Sprite sprite;
        public Vector2 firePos;
        public string weaponName;
        public string className;
        public int damage;
    }
}