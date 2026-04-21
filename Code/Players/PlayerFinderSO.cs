using UnityEngine;

namespace HN.Code.Players
{
    [CreateAssetMenu(fileName = "PlayerFinder", menuName = "SO/PlayerFinder", order = 0)]
    public class PlayerFinderSO : ScriptableObject
    {
        public Player player;
    }
}