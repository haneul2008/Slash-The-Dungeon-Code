using UnityEngine;

namespace HN.Code.Items
{
    public enum ItemType
    {
        Gold
    }

    [CreateAssetMenu(menuName = "SO/Item/Data")]
    public class ItemSO : ScriptableObject
    {
        public ItemType itemType;
        public Sprite itemSprite;

        public int minAmount, maxAmount;
        public string poolName;

        public int GetRandomAmount() => Random.Range(minAmount, maxAmount + 1);
    }
}