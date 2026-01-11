# Toolbar Extender

A Unity Editor extension that allows for customization of the main Unity toolbar. This tool provides a framework for adding custom buttons, labels, and functional elements directly into the editor's main toolbar UI.

## Features

- **Custom Toolbar Elements**: Add buttons, toggles, and labels to the main toolbar.
- **Docking Support**: Position elements on the Left, Middle, or Right sections of the toolbar.
- **Auto-Refresh**: Elements can react to editor events like hierarchy changes.
- **Styling**: Support for custom icons and styling via `MainToolbarElementStyler`.

## Dependencies

Simply download the library into your Unity project and access the utilities across your scripts or import it in Unity with package manager using this URL:

`https://github.com/adammyhre/Unity-Utils.git`

**With Git URL**

Add the following line to the dependencies section of your project's `manifest.json` file.

```json
"com.gitamend.unityutils": "https://github.com/adammyhre/Unity-Utils.git"
```

## Installation

1. Import the package into your Unity project.
2. Ensure the script files are located within an `Editor` folder (e.g., `Assets/Editor`).

## Usage

Create a static class within the `Editor` namespace to define new toolbar elements. Use the `[MainToolbarElement]` attribute to register a static method that returns a `MainToolbarElement`.

### Example: Scene Object Counter

The following example demonstrates how to add a button that displays the total number of GameObjects in the active scene.

```csharp
using System.Linq;
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public static class MainToolbarSceneObjectCount
    {
        private static int _lastCount;

        [MainToolbarElement("Scene/Object Count", defaultDockPosition = MainToolbarDockPosition.Middle)]
        public static MainToolbarElement SceneObjectCount()
        {
            CalculateCount();

            // Create content with label and tooltip
            var content = new MainToolbarContent($"Objects: {_lastCount}",
                "Count of GameObjects in the active scene. Click to force refresh.");
                
            // Create a button that triggers an update on click
            var button = new MainToolbarButton(content, UpdateCount);

            // Subscribe to hierarchy changes to keep count updated
            EditorApplication.hierarchyChanged -= UpdateCount;
            EditorApplication.hierarchyChanged += UpdateCount;

            return button;
        }

        private static void CalculateCount()
        {
            var count = 0;
            var scene = SceneManager.GetActiveScene();
            if (scene.IsValid())
            {
                var roots = scene.GetRootGameObjects();
                count += roots.Sum(root => root.GetComponentsInChildren<Transform>(true).Length);
            }

            _lastCount = count;
        }

        private static void UpdateCount()
        {
            var prevCount = _lastCount;
            CalculateCount();

            // Refresh the UI if the value has changed
            if (_lastCount != prevCount)
            {
                MainToolbar.Refresh("Scene/Object Count");
            }
        }
    }
}
```

## Styling and Icons

To ensure icons render sharply on high-DPI screens or different resolutions, ensure your icon assets are set to appropriate dimensions. You can use the `MainToolbarElementStyler` to apply specific styles or adjust icon sizes programmatically if necessary.

## API Reference

### Attributes

*   `[MainToolbarElement(string id, MainToolbarDockPosition defaultDockPosition)]`: Marks a method as a toolbar element provider.

### Classes

*   `MainToolbar`: Central class for refreshing and managing toolbar state.
*   `MainToolbarContent`: Holds the text, icon, and tooltip for an element.
*   `MainToolbarButton`: Represents a clickable button on the toolbar.

