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

                // --- Liquid Glass Container ---
                // Translucent bright background with a cool tint for glass effect
                parent.style.backgroundColor = new StyleColor(new Color(0.85f, 0.9f, 1f, 0.08f)); 
                
                // Glass edges (Highlights on Top/Left, Shadows on Bottom/Right)
                parent.style.borderTopWidth = 1;
                parent.style.borderBottomWidth = 1;
                parent.style.borderLeftWidth = 1;
                parent.style.borderRightWidth = 1;
                parent.style.borderTopColor = new StyleColor(new Color(1f, 1f, 1f, 0.15f));    // Shine
                parent.style.borderLeftColor = new StyleColor(new Color(1f, 1f, 1f, 0.15f));   // Shine
                parent.style.borderBottomColor = new StyleColor(new Color(0f, 0f, 0f, 0.3f));  // Shadow
                parent.style.borderRightColor = new StyleColor(new Color(0f, 0f, 0f, 0.3f));   // Shadow
                
                parent.style.borderTopLeftRadius = 6;
                parent.style.borderTopRightRadius = 6;
                parent.style.borderBottomLeftRadius = 6;
                parent.style.borderBottomRightRadius = 6;
                
                parent.style.paddingLeft = 6;
                parent.style.paddingRight = 6;
                parent.style.paddingTop = 3;
                parent.style.paddingBottom = 3;
                parent.style.marginTop = 1;
                parent.style.marginBottom = 1;

                // 1. Precise Value Field (Defined first)
                var valueField = new FloatField
                {
                    value = Time.timeScale,
                    isDelayed = true,
                    style =
                    {
                        width = 36,
                        height = 18,
                        marginRight = 8,
                        marginLeft = 0,
                    }
                };

                // Style the inner input text box
                var input = valueField.Q(className: "unity-float-field__input");
                if (input != null)
                {
                    // Dark semi-transparent background for contrast
                    input.style.backgroundColor = new StyleColor(new Color(0f, 0f, 0f, 0.3f)); 
                    
                    // Inner shadow effect via borders inverted
                    input.style.borderTopWidth = 1;
                    input.style.borderBottomWidth = 0; // Flat bottom for 'embedded' look
                    input.style.borderLeftWidth = 1;
                    input.style.borderRightWidth = 1;
                    
                    input.style.borderTopColor = new StyleColor(new Color(0f, 0f, 0f, 0.5f)); 
                    input.style.borderBottomColor = new StyleColor(new Color(1f, 1f, 1f, 0.05f)); // Bottom highlight
                    input.style.borderLeftColor = new StyleColor(new Color(0f, 0f, 0f, 0.5f));
                    input.style.borderRightColor = new StyleColor(new Color(1f, 1f, 1f, 0.05f));
                    
                    input.style.borderTopLeftRadius = 4;
                    input.style.borderTopRightRadius = 4;
                    input.style.borderBottomLeftRadius = 4;
                    input.style.borderBottomRightRadius = 4;
                    
                    input.style.unityTextAlign = TextAnchor.MiddleCenter;
                    input.style.color = new StyleColor(new Color(1f, 1f, 1f, 0.9f));
                    input.style.paddingRight = 0;
                    input.style.paddingLeft = 0;
                    input.style.fontSize = 10;
                }

                // 2. Button for Dropdown Menu (Contains Slider & Presets)
                ToolbarButton titleBtn = null;
                titleBtn = new ToolbarButton(() =>
                {
                    // Open Popup
                    var rect = titleBtn.worldBound;
                    // Approximate screen position or use Event.current if available?
                    // UIElements Button click action might not give us Event.current correct for PopupWindow?
                    // Let's try to deduce a reasonable Rect. UIElements worldBound is panel-relative.
                    // For now, rely on standard PopupWindow behavior which might need screen coordinates.
                    // Actually, let's use a workaround if needed, but ActivatorRect usually works with GUI coordinates.
                    // Trying Event.current first which works for MouseUp events usually.
                    
                    Rect popupRect = (Event.current != null) 
                        ? new Rect(GUIUtility.GUIToScreenPoint(Event.current.mousePosition), Vector2.zero)
                        : new Rect(0, 0, 0, 0);

                    UnityEditor.PopupWindow.Show(popupRect, new TimeScalePopup((newScale) => {
                        Time.timeScale = newScale;
                        valueField.value = newScale;
                    }));
                })
                {
                    text = "Time Scale ▾",
                    tooltip = "Click to set Time Scale",
                    style =
                    {
                        fontSize = 11,
                        marginRight = 6,
                        marginLeft = 0,
                        paddingLeft = 0,
                        paddingRight = 0,
                        paddingTop = 0,
                        paddingBottom = 0,
                        color = new StyleColor(new Color(1f, 1f, 1f, 0.7f)), // Milky white text
                        backgroundColor = Color.clear,
                        borderTopWidth = 0,
                        borderBottomWidth = 0,
                        borderLeftWidth = 0,
                        borderRightWidth = 0,
                        unityTextAlign = TextAnchor.MiddleLeft
                    }
                };

                // 3. Reset Button (Floating Icon)
                var resetBtn = new ToolbarButton(() =>
                {
                    Time.timeScale = 1f;
                    valueField.value = 1f;
                })
                {
                    tooltip = "Reset to 1.0x",
                    style =
                    {
                        width = 18,
                        height = 18,
                        marginLeft = 0,
                        paddingLeft = 0,
                        paddingRight = 0,
                        paddingTop = 0,
                        paddingBottom = 0,
                        backgroundColor = Color.clear,
                        borderTopWidth = 0,
                        borderBottomWidth = 0,
                        borderLeftWidth = 0,
                        borderRightWidth = 0,
                        backgroundSize = new BackgroundSize(14, 14),
                        backgroundPositionX = new BackgroundPosition(BackgroundPositionKeyword.Center),
                        backgroundPositionY = new BackgroundPosition(BackgroundPositionKeyword.Center),
                        backgroundRepeat = new BackgroundRepeat(Repeat.NoRepeat, Repeat.NoRepeat),
                        opacity = 0.8f 
                    }
                };

                var icon = EditorGUIUtility.IconContent("Refresh").image as Texture2D;
                if (icon != null) resetBtn.style.backgroundImage = icon;

                // Sync Logic (Only for Field now)
                valueField.RegisterValueChangedCallback(evt =>
                {
                    Time.timeScale = evt.newValue; 
                });

                parent.Add(titleBtn); // Correct order in UI: Label -> Field -> Reset
                parent.Add(valueField);
                parent.Add(resetBtn);
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
            return new Vector2(220, 160);
        }

        public override void OnGUI(Rect rect)
        {
            GUILayout.BeginArea(rect);
            GUILayout.Space(5);
            
            // Header
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.Label($"Current Time Scale: {Time.timeScale:0.00}x", EditorStyles.boldLabel);
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            // Slider
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUI.BeginChangeCheck();
            float newVal = EditorGUILayout.Slider(Time.timeScale, 0f, 5f);
            if (EditorGUI.EndChangeCheck())
            {
                _onScaleChanged?.Invoke(newVal);
            }
            GUILayout.Space(10);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            
            // Presets
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.Label("Presets", EditorStyles.miniLabel);
            GUILayout.EndHorizontal();

            DrawPresetRow(0f, 0.1f, 0.5f);
            DrawPresetRow(1f, 2f, 5f);
            
            GUILayout.EndArea();
        }

        private void DrawPresetRow(params float[] values)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            foreach (var v in values)
            {
                var label = v == 0f ? "Pause" : $"{v}x";
                if (v == 1f) label = "Normal (1x)";
                
                // Highlight active
                var style = new GUIStyle(EditorStyles.miniButton);
                if (Mathf.Approximately(Time.timeScale, v))
                {
                    style.fontStyle = FontStyle.Bold;
                    style.normal.textColor = Color.cyan;
                }

                if (GUILayout.Button(label, style))
                {
                    _onScaleChanged?.Invoke(v);
                    editorWindow.Close();
                }
            }
            GUILayout.Space(10);
            GUILayout.EndHorizontal();
        }
    }
}