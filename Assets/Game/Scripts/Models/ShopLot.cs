using StarterPack.Audio;
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
        [SerializeField] private Text _priceOne;
        [SerializeField] private Text _priceTen;

        private DropItem _item;
        public event Action<DropItem, int> OnInventoryItemSell;

        public DropItem Item => _item;

        public void Init(DropItem item, int count)
        {
            _item = item;
            _icon.sprite = item.Sprite;
            _priceOne.text = item.SellCost.ToString();
            _priceTen.text = (item.SellCost * 10).ToString();
            if(count > 0)
            {
                _sellOne.interactable = count > 0;
                _sellTen.interactable = count >= 10;
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void SellItem(int count)
        {
            OnInventoryItemSell?.Invoke(_item, count);
            AudioManager.Instance.GetMoneySfx();
        }
    }

}