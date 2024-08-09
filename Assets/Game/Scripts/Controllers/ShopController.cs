using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MultiTool
{
    public class ShopController : MonoBehaviour
    {
        [SerializeField] private GameObject _lotItemPrefab;
        [SerializeField] private InventoryManager _inventoryManager;
        [SerializeField] private Transform _parentLots;

        private List<ShopLot> _lots = new();


        private void OnEnable()
        {
            Init();
        }

        public void Init()
        {
            var inventoryItems = _inventoryManager.Items;
            foreach(var lot in _lots)
            {
                lot.gameObject.SetActive(false);
            }
            foreach(var item in inventoryItems)
            {
                var lot = _lots.FirstOrDefault(l => l.Item == item.Key);
                if(!lot)
                {
                    var lotGO = Instantiate(_lotItemPrefab, _parentLots) as GameObject;
                    lot = lotGO.GetComponent<ShopLot>();
                    lot.OnInventoryItemSell += SellItem;
                    _lots.Add(lot);
                }
                lot.Init(item.Key, item.Value);
            }
        }

        private void SellItem(DropItem item, int count)
        {
            _inventoryManager.RemoveItem(item, count);
            PlayerController.Instance.AddMoney(count * item.SellCost);
            Init();
        }

        private void OnDestroy()
        {
            foreach(var lot in _lots)
            {
                lot.OnInventoryItemSell -= SellItem;
            }
        }
    }

}