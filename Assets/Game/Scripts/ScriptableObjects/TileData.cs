using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MultiTool
{
    [Serializable]
    public class TileData
    {
        [SerializeField] private string _nameRu;
        [SerializeField] private string _nameEn;
        [SerializeField] private TileType _type;
        [SerializeField] private HarvestType _harvestType;
        [SerializeField] private TileBase[] _tiles;
        [SerializeField] private int _miningLevel;
        [SerializeField] private int[] _durability;
        [SerializeField] private RuntimeAnimatorController _animatorController;

        public string NameRu => _nameRu;
        public string NameEn => _nameEn;
        public TileType TileType => _type;
        public HarvestType HarvestType => _harvestType;
        public TileBase[] Tiles => _tiles;
        public int MiningLevel => _miningLevel;
        public int[] Durability => _durability;
        public RuntimeAnimatorController AnimatorController => _animatorController;
    }

}
