using UnityEngine;

namespace MultiTool
{
    public interface IForm
    {
        int Index { get; }
        string FormName { get; }
        Vector2 SizeInTiles { get; }
        Sprite Sprite { get; }
        TileType[] SuitableResources { get; }
        TileType[] UnsuitableResources { get; }
        void UseAbility();
    }
}
