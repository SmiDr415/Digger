using UnityEngine;

namespace Digger
{
    [System.Serializable]
    public class FormData
    {
        public FormType FormType;
        public Vector2 SizeInTiles;
        public TileType[] SuitableResources;
        public TileType[] UnsuitableResources;
    }
}
