using System.Collections.Generic;
using UnityEngine;

namespace MultiTool
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private InventoryManager _inventoryManager;
        [SerializeField] private Transform _inventoryPanel;
        [SerializeField] private GameObject _inventorySlotPrefab;

        private InventorySlot[] _inventorySlots;

        private void Start()
        {
            InitializeInventorySlots();
            GameEventManager.Instance.Subscribe(GameEvent.OnItemAdded, UpdateInventoryUI);
            GameEventManager.Instance.Subscribe(GameEvent.OnItemRemoved, UpdateInventoryUI);
            UpdateInventoryUI();
        }

        private void InitializeInventorySlots()
        {
            _inventorySlots = new InventorySlot[_inventoryManager.GetMaxSlots()];

            for(int i = 0; i < _inventorySlots.Length; i++)
            {
                GameObject slotObject = Instantiate(_inventorySlotPrefab, _inventoryPanel);
                _inventorySlots[i] = slotObject.GetComponent<InventorySlot>();
            }
        }

        private void UpdateInventoryUI()
        {
            Dictionary<DropItem, int> items = _inventoryManager.GetItems();

            int i = 0;
            foreach(KeyValuePair<DropItem, int> item in items)
            {
                if(i < _inventorySlots.Length)
                {
                    _inventorySlots[i].SetItem(item.Key, item.Value);
                    i++;
                }
            }

            // Clear remaining slots
            for(; i < _inventorySlots.Length; i++)
            {
                _inventorySlots[i].ClearSlot();
            }
        }

        private void OnDestroy()
        {
            GameEventManager.Instance.Unsubscribe(GameEvent.OnItemAdded, UpdateInventoryUI);
            GameEventManager.Instance.Unsubscribe(GameEvent.OnItemRemoved, UpdateInventoryUI);
        }
    }
}
