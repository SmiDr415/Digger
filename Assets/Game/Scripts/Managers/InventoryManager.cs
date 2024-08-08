using System.Collections.Generic;
using UnityEngine;

namespace MultiTool
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance;

        [SerializeField] private DropItemDatabase _itemDatabase;
        [SerializeField] private Dictionary<DropItem, int> _items = new();
        [SerializeField] private int _maxSlots = 8;

        public Dictionary<DropItem, int> Items => _items;

        private void Start()
        {
            Instance = this;
        }

        public bool AddItem(string itemName, int quantity)
        {
            var item = _itemDatabase.GetDropItemByNameEN(itemName);
            if(item == null)
            {
                return false;
            }

            if(_items.ContainsKey(item))
            {
                _items[item] += quantity;
            }
            else
            {
                if(_items.Count >= _maxSlots)
                {
                    return false;
                }

                _items[item] = quantity;
            }

            GameEventManager.Instance.TriggerEvent(GameEvent.OnItemAdded);
            return true;
        }

        public bool RemoveItem(DropItem item, int quantity)
        {
            if(_items.ContainsKey(item))
            {
                _items[item] -= quantity;

                if(_items[item] <= 0)
                {
                    _items.Remove(item);
                }

                GameEventManager.Instance.TriggerEvent(GameEvent.OnItemRemoved);
                return true;
            }

            return false;
        }

        public Dictionary<DropItem, int> GetItems()
        {
            return _items;
        }

        public int GetMaxSlots()
        {
            return _maxSlots;
        }

        public DropItem GetItem(int index)
        {
            if(index < 0 || index >= _items.Count)
            {
                return null;
            }

            int i = 0;
            foreach(var item in _items.Keys)
            {
                if(i == index)
                {
                    return item;
                }
                i++;
            }

            return null;
        }

        public int GetItemQuantity(DropItem item)
        {
            return _items.ContainsKey(item) ? _items[item] : 0;
        }
    }
}
