using System.Text;
using HN.Code.Core.Editor;
using UnityEditor;
using UnityEngine;

namespace HN.Code.Weapon.Core.Editor
{
    [CreateAssetMenu(fileName = "WeaponEnumGenerator", menuName = "SO/Weapon/EnumGenerator", order = 2)]
    public class WeaponEnumGenerator : SoEnumGenerator<WeaponDataSO, WeaponEnum>
    {
        [ContextMenu("Generate Enum")]
        private void Generate() => base.GenerateEnum();
    }
}