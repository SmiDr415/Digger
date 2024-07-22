using UnityEngine;

namespace MultiTool
{
    [System.Serializable]
    public class FormData
    {
        public FormType FormType;
        public Vector2 SizeInTiles;
        public Sprite Sprite;
        public TileType[] SuitableResources;
        public TileType[] UnsuitableResources;

        [Tooltip("Количество урона, которое форма наносит за один удар по ресурсу")]
        public int Damage;

        [Tooltip("Количество ресурса, которые выпадает с тайла/спрайта за один удар")]
        public int ExtractedResourceAmount;

        [Tooltip("Количество прочности, на которое уменьшается прочность ресурса за один удар по ресурсу")]
        public int Cost;

        [Tooltip("Количество времени, через которое игрок может ударить ресурс снова")]
        public float Cooldown;
    }
}
