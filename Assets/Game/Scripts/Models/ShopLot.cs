using System;
using UnityEngine;
using UnityEngine.UI;

namespace MultiTool
{
    public class ShopLot : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Button _sellOne;
        [SerializeField] private Button _sellTen;

        private ItemType _itemType;
        private DropItem _item;
        public event Action<DropItem, int> OnInventoryItemSell;

        public ItemType ItemType => _itemType;

        public void Init(DropItem item, int count)
        {
            _item = item;
            _icon.sprite = item.Sprite;

            if(count > 0)
            {
                _sellOne.interactable = count > 0;
                _sellTen.interactable = count >= 10;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void SellItem(int count)
        {
            OnInventoryItemSell?.Invoke(_item, count);
        }
    }

}