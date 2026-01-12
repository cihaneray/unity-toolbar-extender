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

                // Create a "Container" look to show connection
                parent.style.backgroundColor = new StyleColor(new Color(0.1f, 0.1f, 0.1f, 0.2f)); // Subtle dark overlay
                parent.style.borderTopWidth = 1;
                parent.style.borderBottomWidth = 1;
                parent.style.borderLeftWidth = 1;
                parent.style.borderRightWidth = 1;
                parent.style.borderTopColor = new StyleColor(new Color(0f, 0f, 0f, 0.4f));
                parent.style.borderBottomColor = new StyleColor(new Color(0f, 0f, 0f, 0.4f));
                parent.style.borderLeftColor = new StyleColor(new Color(0f, 0f, 0f, 0.4f));
                parent.style.borderRightColor = new StyleColor(new Color(0f, 0f, 0f, 0.4f));
                parent.style.borderTopLeftRadius = 4;
                parent.style.borderTopRightRadius = 4;
                parent.style.borderBottomLeftRadius = 4;
                parent.style.borderBottomRightRadius = 4;
                parent.style.paddingLeft = 4; // Reduced padding
                parent.style.paddingRight = 4;
                parent.style.paddingTop = 2;
                parent.style.paddingBottom = 2;
                parent.style.marginTop = 1;
                parent.style.marginBottom = 1;

                // 0. Label
                var titleLabel = new Label("Time Scale")
                {
                    style =
                    {
                        fontSize = 11, // Small but readable
                        marginRight = 6,
                        color = new StyleColor(new Color(0.8f, 0.8f, 0.8f)), // Subtle
                        unityTextAlign = TextAnchor.MiddleLeft
                    }
                };
                
                // 1. Precise Value Field
                var valueField = new FloatField
                {
                    value = Time.timeScale,
                    isDelayed = true,
                    style =
                    {
                        width = 32, // Compact
                        height = 18,
                        marginRight = 4, // Reduced margin
                        marginLeft = 0,
                    }
                };

                // Style the inner input text box
                var input = valueField.Q(className: "unity-float-field__input");
                if (input != null)
                {
                    input.style.backgroundColor = new StyleColor(new Color(0.15f, 0.15f, 0.15f));
                    input.style.borderTopWidth = 1;
                    input.style.borderBottomWidth = 1;
                    input.style.borderLeftWidth = 1;
                    input.style.borderRightWidth = 1;
                    input.style.borderTopColor = new StyleColor(new Color(0.3f, 0.3f, 0.3f));
                    input.style.borderBottomColor = new StyleColor(new Color(0.3f, 0.3f, 0.3f));
                    input.style.borderLeftColor = new StyleColor(new Color(0.3f, 0.3f, 0.3f));
                    input.style.borderRightColor = new StyleColor(new Color(0.3f, 0.3f, 0.3f));
                    input.style.borderTopLeftRadius = 4; // Slightly tighter radius
                    input.style.borderTopRightRadius = 4;
                    input.style.borderBottomLeftRadius = 4;
                    input.style.borderBottomRightRadius = 4;
                    input.style.unityTextAlign = TextAnchor.MiddleCenter;
                    input.style.color = new StyleColor(new Color(0.9f, 0.9f, 0.9f));
                    input.style.paddingRight = 0;
                    input.style.paddingLeft = 0;
                    input.style.fontSize = 10; // Smaller font
                }

                // 2. Custom Slider
                var slider = new Slider(KMinTimeScale, KMaxTimeScale)
                {
                    value = Time.timeScale,
                    tooltip = "Adjust Time Scale",
                    focusable = false, 
                    style =
                    {
                        width = 80, // Much more compact slider
                        flexGrow = 0,
                        marginRight = 4, // Reduced margin
                        justifyContent = Justify.Center 
                    }
                };

                // Style the Track
                var tracker = slider.Q(className: "unity-base-slider__tracker");
                if (tracker != null)
                {
                    tracker.style.height = 6; // Thicker track
                    tracker.style.marginTop = -3; // Center vertically (half height)
                    tracker.style.backgroundColor = new StyleColor(new Color(0.35f, 0.35f, 0.35f)); // Medium grey
                    tracker.style.borderTopWidth = 0;
                    tracker.style.borderBottomWidth = 0;
                    tracker.style.borderLeftWidth = 0;
                    tracker.style.borderRightWidth = 0;
                    tracker.style.borderTopLeftRadius = 3;
                    tracker.style.borderTopRightRadius = 3;
                    tracker.style.borderBottomLeftRadius = 3;
                    tracker.style.borderBottomRightRadius = 3;
                }

                // Style the Dragger (Handle)
                var dragger = slider.Q(className: "unity-base-slider__dragger");
                if (dragger != null)
                {
                    dragger.style.width = 14;
                    dragger.style.height = 14;
                    dragger.style.marginTop = -7; // Center vertically
                    dragger.style.backgroundColor = new StyleColor(new Color(0.7f, 0.7f, 0.7f)); // Lighter grey
                    dragger.style.backgroundImage = null; // Remove default Unity knob sprite
                    
                    // Kill borders to prevent blue focus ring
                    dragger.style.borderTopWidth = 0;
                    dragger.style.borderBottomWidth = 0;
                    dragger.style.borderLeftWidth = 0;
                    dragger.style.borderRightWidth = 0;
                    dragger.style.borderTopColor = Color.clear;
                    dragger.style.borderBottomColor = Color.clear;
                    dragger.style.borderLeftColor = Color.clear;
                    dragger.style.borderRightColor = Color.clear;

                    dragger.style.borderTopLeftRadius = 7; // Circle
                    dragger.style.borderTopRightRadius = 7;
                    dragger.style.borderBottomLeftRadius = 7;
                    dragger.style.borderBottomRightRadius = 7;
                }

                // 3. Reset Button
                var resetBtn = new ToolbarButton(() =>
                {
                    Time.timeScale = 1f;
                    slider.value = 1f;
                    valueField.value = 1f;
                })
                {
                    tooltip = "Reset to 1.0x",
                    style =
                    {
                        width = 20,
                        height = 20,
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
                        // Pixel perfect icon rendering
                        backgroundSize = new BackgroundSize(16, 16),
                        backgroundPositionX = new BackgroundPosition(BackgroundPositionKeyword.Center),
                        backgroundPositionY = new BackgroundPosition(BackgroundPositionKeyword.Center),
                        backgroundRepeat = new BackgroundRepeat(Repeat.NoRepeat, Repeat.NoRepeat)
                    }
                };

                var icon = EditorGUIUtility.IconContent("Refresh").image as Texture2D;
                if (icon != null) resetBtn.style.backgroundImage = icon;

                // Sync Logic
                slider.RegisterValueChangedCallback(evt =>
                {
                    Time.timeScale = evt.newValue;
                    valueField.SetValueWithoutNotify(evt.newValue);
                });

                valueField.RegisterValueChangedCallback(evt =>
                {
                    Time.timeScale = evt.newValue; 
                    slider.SetValueWithoutNotify(evt.newValue);
                });

                parent.Add(titleLabel); // Added title
                parent.Add(valueField);
                parent.Add(slider);
                parent.Add(resetBtn);
            });

            return element;
        }
    }
}