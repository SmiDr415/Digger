using UnityEngine;

[CreateAssetMenu(fileName = "ProjectWindowSettings", menuName = "Settings/ProjectWindowSettings")]
public class ProjectWindowSettings : ScriptableObject
{
    [SerializeField] private Color _defaultTextColor = Color.yellow;
    [SerializeField] private Color _editorFolderColor = Color.green;
    [SerializeField] private Color _gameFolderColor = Color.blue;
    [SerializeField] private Color _animationFolderColor = Color.red;
    [SerializeField] private Color _scenesFolderColor = Color.gray;
    [SerializeField] private Color _selectedColor = new(0.0f, 1.0f, 0.0f, 0.2f);

    [SerializeField] private Texture2D _folderIcon;
    [SerializeField] private Texture2D _animFolder;
    [SerializeField] private Texture2D _scenesFolder;
    [SerializeField] private Texture2D _scriptsFolder;
    [SerializeField] private Texture2D _editorFolder;
    [SerializeField] private Texture2D _gameFolder;
    [SerializeField] private Texture2D _materialsFolder;
    [SerializeField] private Texture2D _prefabsFolder;
    [SerializeField] private Texture2D _audioFolder;
    [SerializeField] private Texture2D _dataFolder;
    [SerializeField] private Texture2D _spritesFolder;
    [SerializeField] private Texture2D _shaderFolder;

    [SerializeField, Range(0, 5)] private int _space = 1;

    public int Space => _space;
    public Color DefaultTextColor => _defaultTextColor;
    public Color EditorFolderColor => _editorFolderColor;
    public Color GameFolderColor => _gameFolderColor;
    public Color SelectedColor => _selectedColor;
    public Color AnimationFolderColor => _animationFolderColor;
    public Color ScenesFolderColor => _scenesFolderColor;
    public Texture2D FolderIcon => _folderIcon;
    public Texture2D AnimFolder => _animFolder != null ? _animFolder : _folderIcon;
    public Texture2D ScenesFolder => _scenesFolder != null ? _scenesFolder : _folderIcon;
    public Texture2D ScriptsFolder => _scriptsFolder != null ? _scriptsFolder : _folderIcon;
    public Texture2D PrefabsFolder => _prefabsFolder != null ? _prefabsFolder : _folderIcon;
    public Texture2D AudioFolder => _audioFolder != null ? _audioFolder : _folderIcon;
    public Texture2D MaterialsFolder => _materialsFolder != null ? _materialsFolder : _folderIcon;
    public Texture2D SpritesFolder => _spritesFolder != null ? _spritesFolder : _folderIcon;
    public Texture2D ModelsFolder { get; internal set; }
    public Texture2D ShadersFolder => _shaderFolder != null ? _shaderFolder : _folderIcon;
    public Texture2D EditorFolder => _editorFolder != null ? _editorFolder : _folderIcon;
    public Texture2D GameFolder => _gameFolder != null ? _gameFolder : _folderIcon;
    public Texture2D DataFolder => _dataFolder != null ? _dataFolder : _folderIcon;
}
