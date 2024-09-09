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

        private int _damageLevel = 1;
        private int _productionSpeedLevel = 1;
        private int _productionLevel = 1;
        private int _repairStepValue = 10;
        private int _production = 1;

        private UpgradeData _upgradeData;

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
            _upgradeData = data.UpgradeData;
        }

        public int Index => _index;
        public string FormName => _formName;
        public Vector2 SizeInTiles => _sizeInTiles;
        public Sprite Sprite => _sprite;
        public int Strength => _strength;
        public float Cooldown => (float)Math.Round(_cooldown, 2);
        public int Damage => _damage;
        public int Production => _production;
        public int Cost => _cost;
        public int Durability => _durability;

        internal void GetDamage(int val)
        {
            _strength = Mathf.Clamp(_strength -= _cost * val, 0, _durability);
        }

        public UpgradeLevel GetCost(UpgradeType upgradeType)
        {
            var upgrade = _upgradeData.Upgrades.Find(u => u.UpgradeType == upgradeType);
            var levelType = upgradeType == UpgradeType.Speed ? _productionSpeedLevel : upgradeType == UpgradeType.Damage ? _damageLevel : _productionLevel;

            var lev = upgrade.Levels.Find(l => l.Level == levelType);
            return lev;
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

        public void DamageUpgrade(int value)
        {
            _damageLevel++;
            _damage += value;
        }

        public void ProductionSpeedUpgrade(float value)
        {
            _productionSpeedLevel++;
            _cooldown += value;
        }

        public void ProductionUpgrade(int value)
        {
            _productionLevel++;
            _production += value;

        }

        internal void Repair(int val)
        {
            _strength = Math.Clamp(_strength + val, 0, _durability);
        }


    }
}
