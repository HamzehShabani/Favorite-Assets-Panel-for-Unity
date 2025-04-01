# Favorite Assets Editor Window

## Overview
The **Favorite Assets Editor Window** is a Unity Editor tool that allows users to mark assets as favorites for quick access. This tool provides a list view of favorite assets and supports various interactions such as drag-and-drop addition, right-click actions, and double-click opening.

## Features
- **Add Favorite Assets**: Drag assets into the window to add them to the favorite list.
- **Remove Favorite Assets**: Right-click an asset and select "Remove" to delete it from the list.
- **Open in Explorer**: Right-click an asset and select "Open in Explorer" to reveal it in the file system.
- **Quick Selection**: Single-click an asset to highlight and ping it in the Project window.
- **Double-Click to Open**: Open an asset directly in Unity by double-clicking it.
- **Auto-Save Favorites**: The list of favorite assets is saved between Unity sessions.
- **Highlight Last Clicked Asset**: The last clicked asset is visually highlighted.

## How to Use
### Opening the Window
1. In Unity, navigate to **Window > Favorite Assets** to open the tool.

### Adding Assets
1. Drag any asset from the **Project** window into the **Favorite Assets** window.
2. The asset will be added to the list and saved automatically.

### Managing Favorites
- **Click** on an asset to highlight and locate it in the Project window.
- **Double-click** on an asset to open it in Unity.
- **Right-click** on an asset to open a context menu with the following options:
  - **Remove**: Deletes the asset from the favorites list.
  - **Open in Explorer**: Opens the asset’s location in the file explorer.

## Technical Details
- The list of favorite assets is stored in Unity’s `EditorPrefs` under the key `FavoriteAssets`.
- The window UI is built using `EditorGUILayout` with a scrollable list.
- The script uses `AssetDatabase.LoadAssetAtPath` to reference assets by path.
- Handles drag-and-drop operations using `DragAndDrop` events.
- Uses `EditorUtility.RevealInFinder` for opening assets in the file system.
- Keeps track of the last clicked asset for highlighting.
- Supports a **double-click time** threshold of **0.4 seconds** to differentiate between single and double clicks.

## Installation
1. Add the `FavoriteAssetsWindow.cs` script to an `Editor` folder in your Unity project.
2. Open Unity and access the tool via **Window > Favorite Assets**.

## Notes
- This tool is for **editor use only** and does not affect runtime behavior.
- Ensure that the `EditorPrefs` system is working correctly to save and load favorites properly.

## License
This tool is open-source and can be modified freely for personal or commercial projects.

