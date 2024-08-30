using StarterPack.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace MultiTool
{
    public class ShopLot : MonoBehaviour
    {
        [SerializeField] private Image _icon;

        [SerializeField] private Slider _itemsCountSlider;
        [SerializeField] private Text _itemCountText;
        [SerializeField] private Text _price;
        [SerializeField] private Text _priceResult;

        private DropItem _item;
        private int _count;
        public event Action<DropItem, int> OnInventoryItemSell;

        public DropItem Item => _item;

        public void Init(DropItem item, int count)
        {
            _item = item;
            _icon.sprite = item.Sprite;
            _price.text = item.SellCost.ToString();

            _itemsCountSlider.maxValue = count;
            _itemsCountSlider.value = count;

            ChangeCount(count);

            gameObject.SetActive(count > 0);
        }


        public void ChangeCount(float count)
        {
            _count = (int)count;
            _itemCountText.text = _count.ToString();
            _priceResult.text = (_item.SellCost * _count).ToString();
        }

        public void SellItem()
        {
            OnInventoryItemSell?.Invoke(_item, _count);
            AudioManager.Instance.GetMoneySfx();
        }
    }

}