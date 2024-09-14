using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MultiTool
{
    [System.Serializable]
    public class FormData
    {
        [SerializeField] private FormType _formType;
        [SerializeField] private Vector2 _sizeInTiles;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private Sprite _icon;
        [SerializeField] private List<TileHarvestable> _tileHarvestables;
        [SerializeField] private TextAsset _upgradeJson;

        [SerializeField, Tooltip("Количество урона, которое форма наносит за один удар по ресурсу")] private int _damage;
        [SerializeField, Tooltip("Количество ресурса, которые выпадает с тайла/спрайта за один удар")] private int _extractedResourceAmount;
        [SerializeField, Tooltip("Количество прочности, на которое уменьшается прочность ресурса за один удар по ресурсу")] private int _cost;
        [SerializeField, Tooltip("Количество времени, через которое игрок может ударить ресурс снова")] private float _cooldown;
        [SerializeField, Tooltip("Прочность")] private int _durability;

        [NonSerialized]
        private UpgradeData _upgradeData = null;

        public FormType FormType => _formType;
        public Vector2 SizeInTiles => _sizeInTiles;
        public Sprite Sprite => _sprite;
        public Sprite Icon => _icon;
        public int Damage => _damage;
        public int ExtractedResourceAmount => _extractedResourceAmount;
        public int Cost => _cost;
        public float Cooldown => _cooldown;
        public List<TileHarvestable> TileHarvestable => _tileHarvestables;
        public int Durability => _durability;

        // Свойство с ленивой инициализацией
        public UpgradeData UpgradeData
        {
            get
            {
                if(_upgradeData == null)
                {
                    if(_upgradeJson != null)
                    {
                        _upgradeData = JsonConvert.DeserializeObject<UpgradeData>(_upgradeJson.text);
                        ;
                    }
                    else
                    {
                        Debug.LogError("JSON файл не назначен.");
                    }
                }
                return _upgradeData;
            }
            private set
            {
                _upgradeData = value;
            }
        }
    }
}
