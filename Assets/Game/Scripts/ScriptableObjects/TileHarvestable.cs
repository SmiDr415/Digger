using UnityEngine;

namespace MultiTool
{
    [System.Serializable]
    public class TileHarvestable
    {
        [SerializeField] private TileType _tileType;
        [SerializeField] private HarvestType _harvestType;

        public TileType TileType => _tileType;
        public HarvestType HarvestType => _harvestType;
    }
}
