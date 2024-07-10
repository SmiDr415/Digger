using UnityEngine;

namespace Digger
{
    public class PickaxeForm : IPickaxeForm
    {
        private readonly string _formName;
        private readonly Vector2 _sizeInTiles;
        private readonly TileType[] _suitableResources;
        private readonly TileType[] _unsuitableResources;

        public PickaxeForm(FormData data)
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
            // Реализация способности "Кирки"
            Debug.Log("Using Pickaxe Ability: Destroying Stone tiles");
            // Добавить здесь логику для разрушения тайлов типа "Stone"
        }
    }
}
