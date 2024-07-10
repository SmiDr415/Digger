using UnityEngine;

namespace Digger
{
    public class ScytheForm : IScytheForm
    {
        private readonly string _formName;
        private readonly Vector2 _sizeInTiles;
        private readonly TileType[] _suitableResources;
        private readonly TileType[] _unsuitableResources;

        public ScytheForm(FormData data)
        {
            _formName = data.FormType.ToString();
            _sizeInTiles = data.SizeInTiles;
            _suitableResources = data.SuitableResources;
            _unsuitableResources = data.UnsuitableResources;
        }

        public string FormName => _formName;
        public Vector2 SizeInTiles => _sizeInTiles;
        public TileType[] SuitableResources => _suitableResources;
        public TileType[] UnsuitableResources => _unsuitableResources;

        public void UseAbility()
        {
            // Реализация способности "Серпа"
            Debug.Log("Using Scythe Ability: Destroying Grass tiles");
            // Добавить логику для разрушения тайлов типа "Grass"
        }
    }
}
