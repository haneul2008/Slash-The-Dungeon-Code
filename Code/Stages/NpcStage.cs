using System.Collections.Generic;
using HN.Code.NPCs;
using UnityEngine;
using UnityEngine.Serialization;

namespace HN.Code.Stages
{
    public class NpcStage : Stage
    {
        [SerializeField] private List<Transform> npcPosList;
        [SerializeField] private GameObject questionObject;
        [SerializeField] private Npc npc;
        
        public override void InitStage()
        {
            base.InitStage();

            int randNum = Random.Range(0, npcPosList.Count);
            Vector2 npcPos = npcPosList[randNum].position;
            npc.transform.position = npcPos;

            foreach (Transform trm in npcPosList)
            {
                Instantiate(questionObject, new Vector3(trm.position.x, trm.position.y - 1.5f), Quaternion.identity);
            }
        }
    }
}