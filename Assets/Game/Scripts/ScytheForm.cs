using System;
using UnityEngine;

namespace MultiTool
{
    public class PlayerForm
    {
        private readonly int _index;
        private readonly string _formName;
        private readonly Vector2 _sizeInTiles;
        private readonly Sprite _sprite;
        private readonly TileType[] _suitableResources;
        private readonly TileType[] _unsuitableResources;
        private int _strength;
        private float _cooldown;

        public PlayerForm(FormData data, int index)
        {
            _formName = data.FormType.ToString();
            _sprite = data.Sprite;
            _sizeInTiles = data.SizeInTiles;
            _suitableResources = data.SuitableResources;
            _unsuitableResources = data.UnsuitableResources;
            _index = index;
            _strength = 100;
            _cooldown = data.Cooldown;
        }

        public int Index => _index;
        public string FormName => _formName;
        public Vector2 SizeInTiles => _sizeInTiles;
        public Sprite Sprite => _sprite;
        public TileType[] SuitableResources => _suitableResources;
        public TileType[] UnsuitableResources => _unsuitableResources;
        public int Strength => _strength;
        public float Cooldown => _cooldown;

        public void UseAbility()
        {
            // Реализация способности "Серпа"
            Debug.Log("Using Scythe Ability: Destroying Grass tiles");
            // Добавить логику для разрушения тайлов типа "Grass"
        }

        internal void GetDamage(int val)
        {
            _strength = Mathf.Clamp(_strength -= val, 0, 100);
        }
    }
}
