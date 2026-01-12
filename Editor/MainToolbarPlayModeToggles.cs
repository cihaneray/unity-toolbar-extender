using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace Editor
{
    public static class MainToolbarPlayModeToggles
    {
        [MainToolbarElement("PlayMode/Controls", defaultDockPosition = MainToolbarDockPosition.Middle)]
        public static MainToolbarElement PlayModeToolbar()
        {
            var content = new MainToolbarContent("Controls");
            var element = new MainToolbarButton(content, () => {});

            MainToolbarElementStyler.StyleElement<VisualElement>("PlayMode/Controls", (parent) =>
            {
                parent.Clear();
                parent.style.flexDirection = FlexDirection.Row;
                parent.style.alignItems = Align.Center;
                parent.style.backgroundColor = new StyleColor(Color.clear);
                parent.style.borderTopWidth = 0;
                parent.style.borderBottomWidth = 0;
                parent.style.borderLeftWidth = 0;
                parent.style.borderRightWidth = 0;

                parent.Add(PlayFocusedToggle());
                parent.Add(PauseToggle());
                parent.Add(MaximizeOnPlayToggle());
                parent.Add(MuteAudioToggle());
                parent.Add(ErrorPauseToggle());
                parent.Add(ReloadSceneButton());
            });

            Action<VisualElement> styleToggle = (toggle) =>
            {
                toggle.style.width = 16;
                toggle.style.height = 16;
                toggle.style.marginRight = 2;
                toggle.style.marginLeft = 2;
                toggle.style.paddingLeft = 0;
                toggle.style.paddingRight = 0;
                toggle.style.paddingTop = 0;
                toggle.style.paddingBottom = 0;
                toggle.style.alignSelf = Align.Center;
                toggle.style.backgroundRepeat = new BackgroundRepeat(Repeat.NoRepeat, Repeat.NoRepeat);
                toggle.style.backgroundPositionX = new BackgroundPosition(BackgroundPositionKeyword.Center);
                toggle.style.backgroundPositionY = new BackgroundPosition(BackgroundPositionKeyword.Center);
                toggle.style.backgroundSize = new BackgroundSize(BackgroundSizeType.Contain);
            };

            MainToolbarElementStyler.StyleElement("PlayFocusedToggle", styleToggle);
            MainToolbarElementStyler.StyleElement("PauseToggle", styleToggle);
            MainToolbarElementStyler.StyleElement("MaximizeOnPlayToggle", styleToggle);
            MainToolbarElementStyler.StyleElement("MuteAudioToggle", styleToggle);
            MainToolbarElementStyler.StyleElement("ErrorPauseToggle", styleToggle);
            MainToolbarElementStyler.StyleElement("ReloadSceneButton", styleToggle);

            return element;
        }

        private static VisualElement PlayFocusedToggle()
        {
            var toggle = new ToolbarToggle
            {
                name = "PlayFocusedToggle"
            };
            var icon = EditorGUIUtility.IconContent("d_PlayButton").image as Texture2D;
            if (icon == null) icon = EditorGUIUtility.IconContent("PlayButton").image as Texture2D;
            
            if (icon != null) toggle.style.backgroundImage = icon;
            toggle.tooltip = "Play Focused";
            
            toggle.SetValueWithoutNotify(EditorApplication.isPlaying);
            
            toggle.RegisterValueChangedCallback(evt => 
            {
                EditorApplication.isPlaying = evt.newValue;
                if (!evt.newValue) return;
                var window = GetGameView();
                if (window != null) window.Focus();
            });

            Action<PlayModeStateChange> onChange = _ => toggle.SetValueWithoutNotify(EditorApplication.isPlaying);
            EditorApplication.playModeStateChanged += onChange;
            toggle.RegisterCallback<DetachFromPanelEvent>(_ => EditorApplication.playModeStateChanged -= onChange);

            return toggle;
        }

        private static VisualElement PauseToggle()
        {
            var toggle = new ToolbarToggle
            {
                name = "PauseToggle"
            };
            var icon = EditorGUIUtility.IconContent("d_PauseButton").image as Texture2D;
            if (icon == null) icon = EditorGUIUtility.IconContent("PauseButton").image as Texture2D;
            
            if (icon != null) toggle.style.backgroundImage = icon;
            toggle.tooltip = "Pause";
            
            toggle.SetValueWithoutNotify(EditorApplication.isPaused);
            
            toggle.RegisterValueChangedCallback(evt => 
            {
                EditorApplication.isPaused = evt.newValue;
            });

            Action<PlayModeStateChange> onChange = _ => toggle.SetValueWithoutNotify(EditorApplication.isPaused);
            EditorApplication.playModeStateChanged += onChange;
            toggle.RegisterCallback<DetachFromPanelEvent>(_ => EditorApplication.playModeStateChanged -= onChange);

            return toggle;
        }

        private static VisualElement MaximizeOnPlayToggle()
        {
            var toggle = new ToolbarToggle
            {
                name = "MaximizeOnPlayToggle"
            };

            var icon = EditorGUIUtility.IconContent("d_FullScreen").image as Texture2D;
            if (icon == null) icon = EditorGUIUtility.IconContent("FullScreen").image as Texture2D;
            if (icon == null) icon = EditorGUIUtility.IconContent("d_ScaleTool").image as Texture2D;
            if (icon == null) icon = EditorGUIUtility.IconContent("ScaleTool").image as Texture2D;
            if (icon == null) icon = EditorGUIUtility.IconContent("d_ViewToolZoom").image as Texture2D;
            
            if (icon != null) toggle.style.backgroundImage = icon;
            else toggle.text = "Max";

            toggle.tooltip = "Maximize on Play";
            
            toggle.SetValueWithoutNotify(GetMaximizeOnPlay());
            
            toggle.RegisterValueChangedCallback(evt => 
            {
                SetMaximizeOnPlay(evt.newValue);
            });

            return toggle;
        }

        private static VisualElement MuteAudioToggle()
        {
            var toggle = new ToolbarToggle
            {
                name = "MuteAudioToggle"
            };
            var icon = EditorGUIUtility.IconContent("d_SceneViewAudio").image as Texture2D;
            if (icon == null) icon = EditorGUIUtility.IconContent("SceneViewAudio").image as Texture2D;
            
            if (icon != null) toggle.style.backgroundImage = icon;
            toggle.tooltip = "Mute Audio";
            
            toggle.SetValueWithoutNotify(EditorUtility.audioMasterMute);
            
            toggle.RegisterValueChangedCallback(evt => 
            {
                EditorUtility.audioMasterMute = evt.newValue;
            });

            return toggle;
        }

        private static VisualElement ErrorPauseToggle()
        {
            var toggle = new ToolbarToggle
            {
                name = "ErrorPauseToggle" 
            };

            var icon = EditorGUIUtility.IconContent("d_Console.ErrorIcon").image as Texture2D;
            if (icon == null) icon = EditorGUIUtility.IconContent("Console.ErrorIcon").image as Texture2D;
            
            if (icon != null) toggle.style.backgroundImage = icon;
            toggle.tooltip = "Error Pause";
            
            toggle.SetValueWithoutNotify(GetErrorPause());
            
            toggle.RegisterValueChangedCallback(evt => 
            {
                SetErrorPause(evt.newValue);
            });

            return toggle;
        }

        private static VisualElement ReloadSceneButton()
        {
            var button = new ToolbarButton(() =>
            {
                if (Application.isPlaying)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                else
                {
                    Debug.Log("Reloading scene is only available during Play Mode.");
                }
            })
            {
                name = "ReloadSceneButton",
                tooltip = "Reload Active Scene in Play Mode"
            };

            var icon = EditorGUIUtility.IconContent("RotateTool").image as Texture2D;
            if (icon != null) button.style.backgroundImage = icon;

            return button;
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
