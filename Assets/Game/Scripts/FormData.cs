using UnityEngine;

namespace MultiTool
{
    [System.Serializable]
    public class FormData
    {
        [SerializeField] private FormType _formType;
        [SerializeField] private Vector2 _sizeInTiles;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private TileType[] _suitableResources;
        [SerializeField] private TileType[] _unsuitableResources;
        [SerializeField, Tooltip("Количество урона, которое форма наносит за один удар по ресурсу")] private int _damage;
        [SerializeField, Tooltip("Количество ресурса, которые выпадает с тайла/спрайта за один удар")] private int _extractedResourceAmount;
        [SerializeField, Tooltip("Количество прочности, на которое уменьшается прочность ресурса за один удар по ресурсу")] private int _cost;
        [SerializeField, Tooltip("Количество времени, через которое игрок может ударить ресурс снова")] private float _cooldown;

        public FormType FormType => _formType;
        public Vector2 SizeInTiles => _sizeInTiles;
        public Sprite Sprite => _sprite;
        public TileType[] SuitableResources => _suitableResources;
        public TileType[] UnsuitableResources => _unsuitableResources;
        public int Damage => _damage;
        public int ExtractedResourceAmount => _extractedResourceAmount;
        public int Cost => _cost;
        public float Cooldown => _cooldown;
    }
}
