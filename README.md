# Unity Toolbar Extenders

A collection of ready-to-use toolbar buttons and utilities for the Unity Editor. These scripts are designed to simply drop into your project to enhance your workflow with Play Mode controls, Scene switching, and more.

> **Note:** This repository is a collection of extension scripts. It relies on the **Unity-Utils** package to function.

## Prerequisites

These extenders use the Toolbar system provided by `Unity-Utils`. You must install this package for the scripts to work.

**Install via Package Manager (Git URL):**
1. Open the Package Manager in Unity.
2. Click the `+` button in the top left.
3. Select **Add package from git URL...**
4. Enter: `https://github.com/adammyhre/Unity-Utils.git`

**Or via manifest.json:**
Add the following line to your `Packages/manifest.json`:
```
"com.gitamend.unityutils": "https://github.com/adammyhre/Unity-Utils.git"
```

## Installation

1. Ensure the **Prerequisites** are met.
2. Open the Package Manager in Unity.
3. Click the `+` button in the top left.
4. Select **Add package from git URL...**
5. Enter the git URL of this repository:
   `https://github.com/your-username/unity-toolbar-extender.git`

**Or via manifest.json:**
Add the following line to your `Packages/manifest.json`:
```json
"com.your-username.unity-toolbar-extender": "https://github.com/your-username/unity-toolbar-extender.git"
```

## Included Extenders

The collection includes several useful tools located in the `Editor` folder.

### Play Mode Controls
**File:** `MainToolbarPlayModeToggles.cs`
**Dock Position:** Middle

A comprehensive set of toggles and buttons for Play Mode interactions, grouped together in the toolbar:
- **Focused Start**: Enters Play Mode and automatically focuses the Game View.
- **Pause/Continue**: Toggles the pause state of the editor.
- **Maximize on Play**: Toggles the "Maximize on Play" setting.
- **Mute Audio**: Toggles the "Mute Audio" setting.
- **Error Pause**: Toggles "Error Pause" (pause execution when an exception occurs).

### Scene Tools

**Scene Switcher**
- **File:** `MainToolbarSceneSwitcher.cs`
- **Dock Position:** Left
- **Description:** A dropdown menu to quickly switch between scenes that are enabled in the Build Settings.

**Scene Object Counter**
- **File:** `MainToolbarSceneObjectCount.cs`
- **Dock Position:** Middle
- **Description:** Displays the current number of GameObjects in the active scene. Updates automatically when the hierarchy changes.

### Time Management

**Timescale Slider**
- **File:** `MainToolbarTimescaleSlider.cs`
- **Dock Position:** Middle
- **Description:** A slider to adjust `Time.timeScale` dynamically from the toolbar (range: 0x to 5x). Includes a context menu (right-click) to reset.

**Reset Time Scale**
- **File:** `MainToolbarButtons.cs`
- **Dock Position:** Middle
- **Description:** A button to instantly reset the timescale to 1.0.

### Project Utilities

**Project Settings**
- **File:** `MainToolbarButtons.cs`
- **Dock Position:** Middle
- **Description:** A shortcut button to open the Project Settings window.

## Creating Custom Extenders

You can create your own toolbar elements using the `[MainToolbarElement]` attribute.

```csharp
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace Editor
{
    public static class MyCustomToolbar
    {
        [MainToolbarElement("MyTools/SayHello", defaultDockPosition = MainToolbarDockPosition.Right)]
        public static MainToolbarElement HelloButton()
        {
            return new MainToolbarButton(
                new MainToolbarContent("Hello"), 
                () => Debug.Log("Hello World!")
            );
        }
    }
}
```

## Styling

For advanced styling, the included `MainToolbarElementStyler.cs` helper allows you to manipulate the visual elements (VisualElements) of the toolbar buttons, useful for adjusting padding, icons, or layout.

