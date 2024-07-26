using System.Collections.Generic;
using UnityEngine;

namespace MultiTool
{
    public class PlayerForm
    {
        private readonly int _index;
        private readonly string _formName;
        private readonly Vector2 _sizeInTiles;
        private readonly Sprite _sprite;
        private int _strength;
        private float _cooldown;
        private int _damage;
        private readonly List<TileHarvestable> _tileHarvestables;

        public PlayerForm(FormData data, int index)
        {
            _formName = data.FormType.ToString();
            _sprite = data.Sprite;
            _sizeInTiles = data.SizeInTiles;
            _index = index;
            _strength = 100;
            _cooldown = data.Cooldown;
            _damage = data.Damage;
            _tileHarvestables = data.TileHarvestable;
        }

        public int Index => _index;
        public string FormName => _formName;
        public Vector2 SizeInTiles => _sizeInTiles;
        public Sprite Sprite => _sprite;
        public int Strength => _strength;
        public float Cooldown => _cooldown;
        public int Damage => _damage;   

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

        public HarvestType GetHarvestType(TileType tileType)
        {
            foreach(var harvest in _tileHarvestables)
            {
                if(harvest.TileType == tileType)
                {
                    return harvest.HarvestType;
                }
            }

            return HarvestType.Unharvestable;
        }


    }
}
