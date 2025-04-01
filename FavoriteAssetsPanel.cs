using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FavoriteAssetsPanel : EditorWindow
{
    private List<string> favoriteAssetPaths = new List<string>(); // Store asset paths
    private Vector2 scrollPos;
    private Object contextMenuAsset;
    private Object lastClickedAsset; // Track last clicked item

    private const string PREF_KEY = "FavoriteAssets"; // Save key
    private float lastClickTime = 0f;
    private const float doubleClickTime = 0.4f;

    [MenuItem("Window/Favorite Assets")]
    public static void ShowWindow()
    {
        FavoriteAssetsPanel panel = GetWindow<FavoriteAssetsPanel>(" Favorite Assets");
        Texture2D icon = EditorGUIUtility.IconContent("Favorite Icon").image as Texture2D;
        panel.titleContent = new GUIContent(" Favorite Assets", icon);
        panel.LoadFavorites(); // Load when opened
    }

    private void OnGUI()
    {
        HandleDragAndDrop();

        // Scrollable list like the Project panel
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        
        for (int i = 0; i < favoriteAssetPaths.Count; i++)
        {
            string assetPath = favoriteAssetPaths[i];
            Object asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);

            if (asset == null) continue;

            Rect itemRect = EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

            // Show asset icon and name like Project panel
            Texture2D icon = AssetPreview.GetMiniThumbnail(asset);
            GUILayout.Label(new GUIContent(icon), GUILayout.Width(20), GUILayout.Height(16));

            // Highlight last clicked asset
            Color defaultColor = GUI.color;
            if (lastClickedAsset == asset) GUI.color = Color.yellow;

            // Button with tooltip (shows full asset path)
            if (GUILayout.Button(new GUIContent(asset.name, assetPath), EditorStyles.label, GUILayout.ExpandWidth(true)))
            {
                Event e = Event.current;
                if (e.button == 0) // Left Click
                {
                    HandleAssetClick(asset);
                }
                else if (e.button == 1) // Right Click
                {
                    contextMenuAsset = asset;
                    ShowContextMenu();
                    e.Use();
                }
            }

            GUI.color = defaultColor; // Reset color
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
    }

    private void HandleDragAndDrop()
    {
        Event e = Event.current;
        if (e.type == EventType.DragUpdated || e.type == EventType.DragPerform)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            if (e.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                foreach (Object obj in DragAndDrop.objectReferences)
                {
                    string path = AssetDatabase.GetAssetPath(obj);
                    if (!favoriteAssetPaths.Contains(path))
                        favoriteAssetPaths.Add(path);
                }
                SaveFavorites(); // Save after adding
                e.Use();
            }
        }
    }

    private void ShowContextMenu()
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Remove"), false, RemoveSelectedAsset);
        menu.AddItem(new GUIContent("Open in Explorer"), false, OpenInExplorer);
        menu.ShowAsContext();
    }

    private void RemoveSelectedAsset()
    {
        if (contextMenuAsset != null)
        {
            string path = AssetDatabase.GetAssetPath(contextMenuAsset);
            favoriteAssetPaths.Remove(path);
            contextMenuAsset = null;
            SaveFavorites(); // Save after removal
            Repaint(); // Refresh UI
        }
    }

    private void OpenInExplorer()
    {
        if (contextMenuAsset != null)
        {
            EditorUtility.RevealInFinder(AssetDatabase.GetAssetPath(contextMenuAsset));
        }
    }

    private void HandleAssetClick(Object asset)
    {
        float timeSinceLastClick = Time.realtimeSinceStartup - lastClickTime;
        lastClickTime = Time.realtimeSinceStartup;

        lastClickedAsset = asset; // Highlight last clicked asset

        if (timeSinceLastClick < doubleClickTime)
        {
            // Double-click → Open asset
            AssetDatabase.OpenAsset(asset);
        }
        else
        {
            // Single click → Focus on asset
            EditorGUIUtility.PingObject(asset);
        }

        Repaint(); // Refresh UI to update highlight
    }

    private void SaveFavorites()
    {
        EditorPrefs.SetString(PREF_KEY, string.Join(";", favoriteAssetPaths));
    }

    private void LoadFavorites()
    {
        if (EditorPrefs.HasKey(PREF_KEY))
        {
            string savedData = EditorPrefs.GetString(PREF_KEY);
            favoriteAssetPaths = savedData.Split(';').ToList();
        }
    }
}
