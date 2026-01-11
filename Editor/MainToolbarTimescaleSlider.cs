using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class MainToolbarTimescaleSlider
    {
        private const float KMinTimeScale = 0f;
        private const float KMaxTimeScale = 5f;
        private const float KPadding = 10f;

        [MainToolbarElement("Time/Timescale Slider", defaultDockPosition = MainToolbarDockPosition.Middle)]
        public static MainToolbarElement TimeScaleSlider()
        {
            var content = new MainToolbarContent("Time Scale", "Adjust the time scale of the editor.");
            var slider = new MainToolbarSlider(content, Time.timeScale, KMinTimeScale, KMaxTimeScale,
                OnSliderValueChanged)
            {
                populateContextMenu = (menu) =>
                {
                    menu.AppendAction("Reset", _ =>
                    {
                        Time.timeScale = 1f;
                        MainToolbar.Refresh("Time/Timescale Slider");
                    });
                }
            };

            MainToolbarElementStyler.StyleElement<VisualElement>("Time/TimescaleSlider",
                (element) => { element.style.paddingLeft = KPadding; });

            return slider;
        }

        private static void OnSliderValueChanged(float newValue)
        {
            Time.timeScale = newValue;
        }
    }
}