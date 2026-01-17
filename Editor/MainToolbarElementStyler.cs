using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityUtils;

namespace Editor
{
    public static class MainToolbarElementStyler
    {
        public static void StyleElement<T>(string elementName, Action<T> styleAction) where T : VisualElement
        {
            EditorApplication.delayCall += () =>
            {
                ApplyStyle(elementName, (element) =>
                {
                    T targetElement;

                    if (element is T typedElement)
                    {
                        targetElement = typedElement;
                    }
                    else
                    {
                        targetElement = element.Query<T>().First();
                    }

                    if (targetElement != null)
                    {
                        styleAction(targetElement);
                    }
                });
            };
        }

        private static void ApplyStyle(string elementName, Action<VisualElement> styleCallback)
        {
            var element = FindElementByName(elementName);
            if (element != null)
            {
                styleCallback(element);
            }
        }

        private static VisualElement FindElementByName(string name)
        {
            var windows = Resources.FindObjectsOfTypeAll<EditorWindow>();
            foreach (var window in windows)
            {
                try
                {
                    var root = window.rootVisualElement;
                    if (root == null)
                        continue;
                    VisualElement element;
                    if ((element = root.FindElementByName(name)) != null ||
                        (element = root.FindElementByTooltip(name)) != null) return element;
                }
                catch
                {
                    // Safe iteration: ignore windows that might be in an invalid state/rebuild
                }
            }


            return null;
        }
    }
}