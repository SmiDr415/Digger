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
            foreach(var item in inventoryItems)
            {
                var lot = _lots.FirstOrDefault(l => l.ItemType == item.Key.ItemType);
                if(!lot)
                {
                    var lotGO = Instantiate(_lotItemPrefab, _parentLots) as GameObject;
                    lot = lotGO.GetComponent<ShopLot>();
                    _lots.Add(lot);
                }
                lot.Init(item.Key, item.Value);
            }
        }
    }

}