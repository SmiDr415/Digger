using UnityEngine;

namespace MultiTool
{
    [System.Serializable]
    public class InventoryItem
    {
        [SerializeField] private string _itemName;
        [SerializeField] private Sprite _itemIcon;
        [SerializeField] private int _maxStackSize;

        public string ItemName => _itemName;
        public Sprite ItemIcon => _itemIcon;
        public int MaxStackSize => _maxStackSize;

        public InventoryItem(string name, Sprite icon, int maxStack)
        {
            _itemName = name;
            _itemIcon = icon;
            _maxStackSize = maxStack;
        }
    }
}
