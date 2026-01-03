using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace Editor
{
    public static class MainToolbarPlayModeToggles
    {
        [MainToolbarElement("PlayMode/Play Focused", defaultDockPosition = MainToolbarDockPosition.Right)]
        public static MainToolbarElement PlayFocusedToggle()
        {
            var icon = EditorGUIUtility.IconContent("d_PlayButton").image as Texture2D;
            if (icon == null) icon = EditorGUIUtility.IconContent("PlayButton").image as Texture2D;
            
            var content = new MainToolbarContent(icon, "Play Focused");
            
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

            return new MainToolbarToggle(content, EditorApplication.isPlaying, (newValue) =>
            {
                EditorApplication.isPlaying = newValue;
                if (!newValue) return;
                var window = GetGameView();
                if (window != null) window.Focus();
            });
        }

        [MainToolbarElement("PlayMode/Pause", defaultDockPosition = MainToolbarDockPosition.Right)]
        public static MainToolbarElement PauseToggle()
        {
            var icon = EditorGUIUtility.IconContent("d_PauseButton").image as Texture2D;
            if (icon == null) icon = EditorGUIUtility.IconContent("PauseButton").image as Texture2D;
            
            var content = new MainToolbarContent(icon, "Pause");
            
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

            return new MainToolbarToggle(content, EditorApplication.isPaused, (newValue) =>
            {
                EditorApplication.isPaused = newValue;
            });
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            MainToolbar.Refresh("PlayMode/Play Focused");
            MainToolbar.Refresh("PlayMode/Pause");
        }

        [MainToolbarElement("PlayMode/Maximize On Play", defaultDockPosition = MainToolbarDockPosition.Right)]
        public static MainToolbarElement MaximizeOnPlayToggle()
        {
            var icon = EditorGUIUtility.IconContent("d_winbtn_win_max").image as Texture2D;
            if (icon == null) icon = EditorGUIUtility.IconContent("winbtn_win_max").image as Texture2D;
            if (icon == null) icon = EditorGUIUtility.IconContent("d_ViewToolZoom").image as Texture2D;

            var content = icon != null ? new MainToolbarContent(icon, "Maximize on Play") : new MainToolbarContent("Max", "Maximize on Play");

            return new MainToolbarToggle(content, GetMaximizeOnPlay(), SetMaximizeOnPlay);
        }

        [MainToolbarElement("PlayMode/Mute Audio", defaultDockPosition = MainToolbarDockPosition.Right)]
        public static MainToolbarElement MuteAudioToggle()
        {
            var icon = EditorGUIUtility.IconContent("d_SceneViewAudio").image as Texture2D;
            var content = new MainToolbarContent(icon, "Mute Audio");
            return new MainToolbarToggle(content, EditorUtility.audioMasterMute, (newValue) =>
            {
                EditorUtility.audioMasterMute = newValue;
            });
        }

        [MainToolbarElement("PlayMode/Error Pause", defaultDockPosition = MainToolbarDockPosition.Right)]
        public static MainToolbarElement ErrorPauseToggle()
        {
            var icon = EditorGUIUtility.IconContent("Console.ErrorIcon").image as Texture2D;
            var content = new MainToolbarContent(icon, "Error Pause");
            return new MainToolbarToggle(content, GetErrorPause(), SetErrorPause);
        }

        private static bool GetMaximizeOnPlay()
        {
            var window = GetGameView();
            if (window == null) return false;
            var prop = window.GetType().GetProperty("maximizeOnPlay", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (prop == null) return false;
            return (bool)prop.GetValue(window);
        }

        private static void SetMaximizeOnPlay(bool enabled)
        {
            var window = GetGameView();
            if (window == null)
            {
                var gameViewType = Type.GetType("UnityEditor.GameView,UnityEditor");
                if (gameViewType != null)
                    window = EditorWindow.GetWindow(gameViewType, false, "Game", false);
            }

            if (window == null) return;
            var prop = window.GetType().GetProperty("maximizeOnPlay", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (prop != null)
                prop.SetValue(window, enabled);
        }

        private static EditorWindow GetGameView()
        {
            var gameViewType = Type.GetType("UnityEditor.GameView,UnityEditor");
            if (gameViewType == null) return null;
            var objects = Resources.FindObjectsOfTypeAll(gameViewType);
            return (EditorWindow)(objects.Length > 0 ? objects[0] : null);
        }

        private static bool GetErrorPause()
        {
            return EditorPrefs.GetBool("ConsoleErrorPause", false);
        }

        private static void SetErrorPause(bool enabled)
        {
            EditorPrefs.SetBool("ConsoleErrorPause", enabled);
            var consoleWindowType = Type.GetType("UnityEditor.ConsoleWindow,UnityEditor");
            if (consoleWindowType == null) return;
            var window = EditorWindow.GetWindow(consoleWindowType, false, "Console", false);
            var method = consoleWindowType.GetMethod("SetConsoleErrorPause", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (method != null)
            {
                method.Invoke(window, new object[] { enabled });
            }
        }
    }
}
