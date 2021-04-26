using System;
using UnityEngine.UI;

namespace UnityTemplateProjects
{
    public static class UIExtensions
    {
        public static void AddClickListener<T>(this Button button, T param, Action<T> onClick)
        {
            button.onClick.AddListener(delegate
            {
                onClick(param);
            });
        }
    }
}