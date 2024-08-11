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
        private readonly List<TileHarvestable> _tileHarvestables;

        private float _timeStepRepair = 1f;
        private float _lastTimeRepair;

        private int _damageLevel = 0;
        private int _productionSpeedLevel = 0;
        private int _productionLevel = 0;

        public PlayerForm(FormData data, int index)
        {
            _formName = data.FormType.ToString();
            _sprite = data.Sprite;
            _sizeInTiles = data.SizeInTiles;
            _index = index;
            _strength = 10;
            _cooldown = data.Cooldown;
            _damage = data.Damage;
            _tileHarvestables = data.TileHarvestable;
        }

        public int Index => _index;
        public string FormName => _formName;
        public Vector2 SizeInTiles => _sizeInTiles;
        public Sprite Sprite => _sprite;
        public int Strength => _strength;
        public float Cooldown => _cooldown - (float)_productionSpeedLevel / 10;
        public int Damage => _damage + _damageLevel;
        public int Production => 1 + _productionLevel;

        public void UseAbility()
        {
            // ���������� ����������� "�����"
            Debug.Log("Using Scythe Ability: Destroying Grass tiles");
            // �������� ������ ��� ���������� ������ ���� "Grass"
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

        internal void Repair()
        {
            if(_strength < 100)
            {
                if(Time.time - _lastTimeRepair > _timeStepRepair)
                {
                    _lastTimeRepair = Time.time;
                    _strength = Math.Clamp(_strength + 10, 10, 100);
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
    }
}
