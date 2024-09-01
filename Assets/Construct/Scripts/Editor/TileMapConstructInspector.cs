using UnityEngine;
using UnityEditor;
using MultiTool;

[CustomEditor(typeof(TileMapConstruct))]
public class TileMapConstructInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TileMapConstruct tileMapConstruct = (TileMapConstruct)target;

        if(GUILayout.Button("Load Tile Map"))
        {
            tileMapConstruct.GenerateTiles();
        }

        if(GUILayout.Button("Save Tile Map"))
        {
            tileMapConstruct.SaveTileMap();
        }

    }
}