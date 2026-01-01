using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UIElements;

public class MainToolbarTimescaleSlider
{
    private const float k_minTimeScale = 0f;
    private const float k_maxTimeScale = 5f;
    private const float k_padding = 10f;

    [MainToolbarElement("Time/Timescale Slider", defaultDockPosition = MainToolbarDockPosition.Middle)]
    public static MainToolbarElement TimeScaleSlider()
    {
        var content = new MainToolbarContent("Time Scale", "Adjust the time scale of the editor.");
        var slider = new MainToolbarSlider(content, Time.timeScale, k_minTimeScale, k_maxTimeScale, OnSliderValueChanged);

        slider.populateContextMenu = (menu) =>
        {
            menu.AppendAction("Reset", _ =>
            {
                Time.timeScale = 1f;
                MainToolbar.Refresh("Time/Timescale Slider");
            });
        };

        MainToolbarElementStyler.StyleElement<VisualElement>("Time/Timescale Slider", (element) =>
        {
            element.style.paddingLeft = k_padding;
        });

        return slider;
    }

    private static void OnSliderValueChanged(float newValue)
    {
        Time.timeScale = newValue;
    }

}