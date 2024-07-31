using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MultiTool
{
    [Serializable]
    public class DropItem
    {
        [SerializeField] private string _nameItemRu;
        [SerializeField] private string _nameItemEN;
        [SerializeField] private int _sellCost;
        [SerializeField] private ItemType _itemType;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private TileBase _tile;

        public string NameItemRu => _nameItemRu;
        public string NameItemEN => _nameItemEN;
        public int SellCost => _sellCost;
        public ItemType ItemType => _itemType;
        public Sprite Sprite => _sprite;
        public TileBase Tile => _tile;
    }

}