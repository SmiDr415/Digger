using UnityEngine;

namespace MultiTool
{
    [CreateAssetMenu(fileName = "DropItemDatabase", menuName = "ScriptableObjects/DropItemDatabase", order = 1)]
    public class DropItemDatabase : ScriptableObject
    {
        [SerializeField] private DropItem[] _dropItems;

        public DropItem[] DropItems => _dropItems;

        public DropItem GetDropItemByNameEN(string nameEN)
        {
            foreach(var item in _dropItems)
            {
                if(item.NameItemEN == nameEN)
                {
                    return item;
                }
            }
            Debug.LogWarning($"DropItem with nameEN '{nameEN}' not found.");
            return null;
        }

        public string GetNameByTileName(string tileName)
        {
            foreach(var item in _dropItems)
            {
                var name = tileName.Split('_')[1];
                if(item.Tile.name.Contains(name))
                    return item.NameItemEN;
            }

            return null;
        }
    }
}
