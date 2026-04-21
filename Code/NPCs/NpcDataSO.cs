using UnityEngine;

namespace HN.Code.NPCs
{
    [CreateAssetMenu(fileName = "NpcData", menuName = "SO/Npc/Data", order = 0)]
    public class NpcDataSO : ScriptableObject
    {
        public string npcName;
    }
}