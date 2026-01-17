using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public static class MainToolbarSceneSwitcher
    {
        [MainToolbarElement("Scene/Scene Switcher", defaultDockPosition = MainToolbarDockPosition.Left)]
        public static MainToolbarElement SceneSwitcher()
        {
            var content = new MainToolbarContent("Switch Scene ▾", "Click to switch scene");
            var button = new MainToolbarButton(content, ShowSceneMenu);

            return button;
        }

        private static void ShowSceneMenu()
        {
            var menu = new GenericMenu();
            var scenes = EditorBuildSettings.scenes;

            foreach (var scene in scenes)
            {
                if (!scene.enabled) continue;
                var name = Path.GetFileNameWithoutExtension(scene.path);
                var path = scene.path;
                var isCurrent = SceneManager.GetActiveScene().path == path;

                menu.AddItem(new GUIContent(name), isCurrent, () =>
                {
                    if (SceneManager.GetActiveScene().path == path) return;
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(path);
                    }
                });
            }

            if (scenes.Length == 0)
            {
                menu.AddDisabledItem(new GUIContent("No scenes in build settings"));
            }

            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Open Build Settings..."), false,
                () => { EditorWindow.GetWindow(typeof(BuildPlayerWindow)); });

            menu.ShowAsContext();
        }
    }
}