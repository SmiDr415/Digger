using System;
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
        private int _cost;
        private int _durability;
        private readonly List<TileHarvestable> _tileHarvestables;

        private float _timeStepRepair = 1f;
        private float _lastTimeRepair;

        private int _damageLevel = 0;
        private int _productionSpeedLevel = 0;
        private int _productionLevel = 0;
        private int _repairStepValue = 10;

        public PlayerForm(FormData data, int index)
        {
            _formName = data.FormType.ToString();
            _sprite = data.Sprite;
            _sizeInTiles = data.SizeInTiles;
            _index = index;
            _strength = data.Durability;
            _cooldown = data.Cooldown;
            _damage = data.Damage;
            _tileHarvestables = data.TileHarvestable;
            _cost = data.Cost;
            _durability = data.Durability;
        }

        public int Index => _index;
        public string FormName => _formName;
        public Vector2 SizeInTiles => _sizeInTiles;
        public Sprite Sprite => _sprite;
        public int Strength => _strength;
        public float Cooldown => _cooldown - (float)_productionSpeedLevel / 10;
        public int Damage => _damage + _damageLevel;
        public int Production => 1 + _productionLevel;
        public int Cost => _cost;
        public int Durability => _durability;

        internal void GetDamage(int val)
        {
            _strength = Mathf.Clamp(_strength -= _cost * val, 0, _durability);
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

        internal void Repair()
        {
            if(_strength < _durability)
            {
                if(Time.time - _lastTimeRepair > _timeStepRepair)
                {
                    _lastTimeRepair = Time.time;
                    _strength = Math.Clamp(_strength + _repairStepValue, 0, _durability);
                    UIController.Instance.SetStrengthValue(_index, _strength);
                }

            }
        }

        public void DamageUpgrade()
        {
            _damageLevel++;
        }

        public void ProductionSpeedUpgrade()
        {
            _productionSpeedLevel++;
        }

        public void ProductionUpgrade()
        {
            _productionLevel++;
        }

        internal void Repair(int val)
        {
            _strength = Math.Clamp(_strength + val, 0, _durability);
        }
    }
}
