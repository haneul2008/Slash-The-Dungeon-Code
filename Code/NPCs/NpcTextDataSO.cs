using HN.Code.Reference.Texts;
using UnityEngine;

namespace HN.Code.NPCs
{
    [CreateAssetMenu(fileName = "NpcTextData", menuName = "SO/Npc/TextData", order = 0)]
    public class NpcTextDataSO : TextDataSO
    {
        [SerializeField] private NpcDataSO targetNpc;
        
        public override object GetKey()
        {
            return targetNpc;
        }
    }
}