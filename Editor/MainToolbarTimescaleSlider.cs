using System;
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class MainToolbarTimescaleSlider
    {
        private const float KMinTimeScale = 0f;
        private const float KMaxTimeScale = 5f;

        [MainToolbarElement("Time/Controls", defaultDockPosition = MainToolbarDockPosition.Middle)]
        public static MainToolbarElement TimeControls()
        {
            var content = new MainToolbarContent("Time Controls");
            var element = new MainToolbarButton(content, () => { });

            MainToolbarElementStyler.StyleElement<VisualElement>("Time/Controls", (parent) =>
            {
                parent.Clear();
                parent.style.flexDirection = FlexDirection.Row;
                parent.style.alignItems = Align.Center;
                parent.style.justifyContent = Justify.Center;

                ToolbarButton timeScaleBtn = null;
                timeScaleBtn = new ToolbarButton(() =>
                {
                    Vector2 mouseScreenPos = Vector2.zero;
                    if (Event.current != null)
                        mouseScreenPos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);

                    Rect popupRect = new Rect(mouseScreenPos.x - 110, mouseScreenPos.y + 14, 220, 0);

                    UnityEditor.PopupWindow.Show(popupRect, new TimeScalePopup((newScale) => {
                        Time.timeScale = newScale;
                        timeScaleBtn.text = $"Time Scale: {newScale:0.00}x";
                    }));
                })
                {
                    tooltip = "Click to adjust Time Scale",
                    style =
                    {
                         unityTextAlign = TextAnchor.MiddleCenter,
                         width = 130
                    }
                };
                
                // Poll to keep text updated (in case changed via scripts or inspector)
                timeScaleBtn.schedule.Execute(() => {
                     timeScaleBtn.text = $"Time Scale: {Time.timeScale:0.00}x";
                }).Every(200);

                parent.Add(timeScaleBtn);
            });

            return element;
        }
    }

    public class TimeScalePopup : PopupWindowContent
    {
        private Action<float> _onScaleChanged;
        
        public TimeScalePopup(Action<float> onScaleChanged)
        {
            _onScaleChanged = onScaleChanged;
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(220, 150);
        }

        public override void OnGUI(Rect rect)
        {
            if (Event.current.type == EventType.Repaint)
            {
                EditorStyles.helpBox.Draw(rect, false, false, false, false);
            }

            GUILayout.BeginVertical();
            GUILayout.Space(8);
            
            GUILayout.BeginHorizontal();
            GUILayout.Space(12);
            GUILayout.Label($"Current: {Time.timeScale:0.00}x", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(6);

            GUILayout.BeginHorizontal();
            GUILayout.Space(6);
            EditorGUI.BeginChangeCheck();
            float newVal = EditorGUILayout.Slider(Time.timeScale, 0f, 5f);
            if (EditorGUI.EndChangeCheck())
            {
                _onScaleChanged?.Invoke(newVal);
            }
            GUILayout.Space(6);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            
            GUILayout.BeginHorizontal();
            GUILayout.Space(12);
            GUILayout.Label("Presets", EditorStyles.miniLabel);
            GUILayout.EndHorizontal();

            GUILayout.Space(2);
            DrawPresetRow(0f, 0.1f, 0.5f);
            DrawPresetRow(1f, 2f, 5f);
            
            GUILayout.EndVertical();
        }

        private void DrawPresetRow(params float[] values)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(8);
            foreach (var v in values)
            {
                var label = v == 0f ? "Pause" : $"{v}x";
                if (v == 1f) label = "Normal";
                
                var style = new GUIStyle(EditorStyles.miniButton);
                if (Mathf.Approximately(Time.timeScale, v))
                {
                    style.fontStyle = FontStyle.Bold;
                }

                if (GUILayout.Button(label, style))
                {
                    _onScaleChanged?.Invoke(v);
                    editorWindow.Close();
                }
            }
            GUILayout.Space(8);
            GUILayout.EndHorizontal();
        }
    }
}