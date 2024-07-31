using System;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class CustomProjectWindow : EditorWindow
{
    private static ProjectWindowSettings _settings;
    private static string _currentFilePath;

    static CustomProjectWindow()
    {
        _settings = AssetDatabase.LoadAssetAtPath<ProjectWindowSettings>("Assets/Editor/ProjectWindowSettings.asset");

        EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemGUI;
        Selection.selectionChanged += OnSelectionChanged;
    }

    private static void OnSelectionChanged()
    {
        if(Selection.activeObject != null)
        {
            string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if(!string.IsNullOrEmpty(assetPath))
            {
                _currentFilePath = assetPath;
                EditorApplication.delayCall += EditorApplication.RepaintProjectWindow; // Перерисовываем окно
            }
        }
    }

    private static void OnProjectWindowItemGUI(string guid, Rect selectionRect)
    {
        if(_settings == null)
        {
            Debug.LogWarning("Настройки ProjectWindowSettings не найдены. Пожалуйста, создайте ассет настроек.");
            return;
        }

        string path = AssetDatabase.GUIDToAssetPath(guid);

        if(AssetDatabase.IsValidFolder(path))
        {
            DrawFolderItem(path, selectionRect);
        }
        else
        {
            DrawFileItem(path, selectionRect);
        }
    }

    private static void DrawFolderItem(string path, Rect selectionRect)
    {
        string fileName = System.IO.Path.GetFileName(path);
        Color folderColor = GetFolderColor(path);
        GUIStyle style = CreateLabelStyle(folderColor);
        Texture2D icon = _settings.FolderIcon != null ? _settings.FolderIcon : EditorGUIUtility.IconContent("Folder Icon").image as Texture2D;


        // Отрисовка иконки и текста
        Rect iconRect = new(selectionRect.x, selectionRect.y, selectionRect.height, selectionRect.height);
        float space = iconRect.width + _settings.Space;

        bool isRenaming = EditorGUIUtility.editingTextField && Selection.activeObject != null && _currentFilePath.Contains(Selection.activeObject.name);

        if(!isRenaming)
        {
            Rect labelRect;
            if(selectionRect.width >= selectionRect.height)
            {

                DrawBackground(selectionRect);
                GUI.DrawTexture(iconRect, icon);
                labelRect = new Rect(selectionRect.x + space, selectionRect.y, selectionRect.width - space, selectionRect.height);

                DrawBackground(labelRect);
                EditorGUI.LabelField(labelRect, new GUIContent(fileName), style);
                HighlightSelectedItem(path, selectionRect);
            }
            else
            {
                icon = GetFolderIcon(path);

                iconRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.width, selectionRect.height - style.fontSize - _settings.Space);
                DrawBackground(iconRect);
                GUI.DrawTexture(iconRect, icon);
                labelRect = new Rect(iconRect.x - _settings.Space, iconRect.y + iconRect.height + _settings.Space, iconRect.width + _settings.Space * 2, style.fontSize);
                style.alignment = TextAnchor.MiddleCenter;
                style.wordWrap = true;

                // Рассчитываем высоту текста с учетом ширины и стиля
                float textHeight = style.CalcHeight(new GUIContent(fileName), labelRect.width);

                // Создаем новый прямоугольник с адаптированной высотой
                Rect adaptedRect = new Rect(labelRect.x - _settings.Space, labelRect.y, labelRect.width + _settings.Space, textHeight);

                DrawBackground(adaptedRect);
                EditorGUI.LabelField(adaptedRect, fileName, style);
                HighlightSelectedItem(path, adaptedRect);
            }
        }

        CheckForSelectionEvent(path, selectionRect);
    }

    private static Texture2D GetFolderIcon(string path)
    {
        string lowerPath = path.ToLower(); // Приведение пути к нижнему регистру для консистентности

        return true switch
        {
            bool _ when lowerPath.Contains("animations") => _settings.AnimFolder,
            bool _ when lowerPath.Contains("scenes") => _settings.ScenesFolder,
            bool _ when lowerPath.Contains("scripts") => _settings.ScriptsFolder,
            bool _ when lowerPath.Contains("prefabs") => _settings.PrefabsFolder,
            bool _ when lowerPath.Contains("audio") => _settings.AudioFolder,
            bool _ when lowerPath.Contains("materials") => _settings.MaterialsFolder,
            bool _ when lowerPath.Contains("textures") => _settings.TexturesFolder,
            bool _ when lowerPath.Contains("models") => _settings.ModelsFolder,
            bool _ when lowerPath.Contains("shaders") => _settings.ShadersFolder,
            bool _ when lowerPath.Contains("editor") => _settings.EditorFolder,
            bool _ when lowerPath.Contains("game") => _settings.GameFolder,
            _ => _settings.FolderIcon != null ? _settings.FolderIcon : EditorGUIUtility.IconContent("Folder Icon").image as Texture2D,
        };
    }



    private static void DrawFileItem(string path, Rect selectionRect)
    {
        // Реализация для файлов, если это необходимо
    }

    private static Color GetFolderColor(string path)
    {
        string lowerPath = path.ToLower(); // Приведение пути к нижнему регистру для консистентности

        return true switch
        {
            bool _ when lowerPath.Contains("animations") => _settings.EditorFolderColor,
            bool _ when lowerPath.Contains("scenes") => _settings.ScenesFolderColor,
            bool _ when lowerPath.Contains("game") => _settings.GameFolderColor,
            bool _ when lowerPath.Contains("editor") => _settings.AnimationFolderColor,
            _ => _settings.DefaultTextColor,
        };
    }

    private static GUIStyle CreateLabelStyle(Color textColor)
    {
        return new GUIStyle(EditorStyles.label)
        {
            normal = { textColor = textColor }
        };
    }

    private static void DrawBackground(Rect selectionRect)
    {
        Color backgroundColor = EditorGUIUtility.isProSkin ? new Color(0.219f, 0.219f, 0.219f) : new Color(0.8f, 0.8f, 0.8f);
        EditorGUI.DrawRect(selectionRect, backgroundColor);
    }

    private static void HighlightSelectedItem(string path, Rect selectedRect)
    {
        if(path == _currentFilePath)
        {
            Rect rect = selectedRect;
            EditorGUI.DrawRect(rect, _settings.SelectedColor);
        }
    }

    private static void CheckForSelectionEvent(string path, Rect selectionRect)
    {
        Event currentEvent = Event.current;
        if(currentEvent.type == EventType.MouseDown && selectionRect.Contains(currentEvent.mousePosition) && currentEvent.button == 0)
        {
            _currentFilePath = path;
            EditorApplication.delayCall += EditorApplication.RepaintProjectWindow; // Перерисовываем окно при выборе
        }
    }
}
