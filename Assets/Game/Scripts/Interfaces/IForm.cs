using UnityEngine;

namespace Digger
{
    public interface IForm
    {
        string FormName { get; }
        Vector2 SizeInTiles { get; }
        TileType[] SuitableResources { get; }
        TileType[] UnsuitableResources { get; }
        void UseAbility();
    }
}
