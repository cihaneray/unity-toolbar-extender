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

        [MainToolbarElement("Scene/Object Count", defaultDockPosition = MainToolbarDockPosition.Right)]
        public static MainToolbarElement SceneObjectCount()
        {
            CalculateCount();

            var content = new MainToolbarContent($"Objects: {_lastCount}",
                "Count of GameObjects in the active scene. Click to force refresh.");
            var button = new MainToolbarButton(content, UpdateCount);

            // Prevent duplicate subscriptions
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

            if (_lastCount != prevCount)
            {
                MainToolbar.Refresh("Scene/Object Count");
            }
        }
    }

}