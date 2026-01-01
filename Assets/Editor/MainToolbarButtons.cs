using UnityEditor;
using UnityEditor.Toolbars;  
using UnityEngine;
using UnityEngine.UIElements;

public class MainToolbarButtons
{
    [MainToolbarElement("Project/Open Project Settings", defaultDockPosition = MainToolbarDockPosition.Middle)]
    public static MainToolbarElement ProjectSettingsButton()
    {
        var icon = EditorGUIUtility.IconContent("SettingsIcon").image as Texture2D;
        var content = new MainToolbarContent(icon);
        return new MainToolbarButton(content, () => { SettingsService.OpenProjectSettings(); });
    }
    
    [MainToolbarElement("Time/Reset Time Scale", defaultDockPosition = MainToolbarDockPosition.Middle)]
    public static MainToolbarElement ResetTimeScaleButton()
    {
        var icon = EditorGUIUtility.IconContent("Refresh").image as Texture2D;
        var content = new MainToolbarContent(icon, "Reset");
        var button = new MainToolbarButton(content, () =>
        {
            Time.timeScale = 1f;
            MainToolbar.Refresh("Time/Timescale Slider");
        });

        return button;
    }
}