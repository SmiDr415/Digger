using UnityEngine;
using System.Collections.Generic;

namespace MultiTool
{
    [CreateAssetMenu(fileName = "InventoryItemDatabase", menuName = "Inventory/Item Database")]
    public class InventoryItemDatabase : ScriptableObject
    {
        public List<InventoryItem> items;

        public InventoryItem GetItemByName(string name)
        {
            return items.Find(item => item.ItemName == name);
        }
    }
}
