using HN.Code.EventSystems;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace HN.Code.Stages
{
    public class Stage : MonoBehaviour
    {
        [field: SerializeField] public StageDataSO StageData { get; private set; }
        [SerializeField] protected GameEventChannelSO stageChannel;
        [SerializeField] protected GameEventChannelSO playerChannel;
        [SerializeField] protected Tilemap torchTilemap;
        [SerializeField] protected TileBase torchTile;
        [SerializeField] protected Vector2 torchLightOffset;
        [SerializeField] protected GameObject torchLight;
        [SerializeField] protected Transform lightTrm;
        [SerializeField] protected Transform playerSpawnTrm;

        private Vector2Int _prevPlayerPos;

        public virtual void InitStage()
        {
            stageChannel.RaiseEvent(StageEvents.PlayerSpawnEvent.Initializer(playerSpawnTrm.position));
            
            BoundsInt bounds = torchTilemap.cellBounds;
            TileBase[] allTiles = torchTilemap.GetTilesBlock(bounds);

            for (int x = 0; x < bounds.size.x; x++)
            {
                for (int y = 0; y < bounds.size.y; y++)
                {
                    Vector3Int tilePos = new Vector3Int(x + bounds.xMin, y + bounds.yMin, 0);
                    TileBase currentTile = allTiles[x + y * bounds.size.x];
                    
                    if (currentTile == torchTile)
                    {
                        Vector3 worldPos = torchTilemap.CellToWorld(tilePos) + (Vector3)torchLightOffset;
                        Instantiate(torchLight, worldPos, Quaternion.identity, lightTrm);
                    }
                }
            }
        }
    }
}