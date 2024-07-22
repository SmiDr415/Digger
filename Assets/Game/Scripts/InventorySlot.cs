using UnityEngine;
using UnityEngine.UI;

namespace MultiTool
{
    public class InventorySlot : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _quantityText;

        private DropItem _item;
        private int _quantity;

        public void SetItem(DropItem item, int quantity)
        {
            _item = item;
            _quantity = quantity;

            _icon.sprite = item.Sprite;
            _quantityText.text = quantity > 1 ? quantity.ToString() : "";
        }

        public void ClearSlot()
        {
            _item = null;
            _quantity = 0;

            _icon.sprite = null;
            _quantityText.text = "";
        }
    }
}
