using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UnityTemplateProjects
{
    public class QueryableDropdownField : MonoBehaviour
    {
        [SerializeField]
        public string labelText;

        [SerializeField]
        private GameObject parent;

        private TMP_Text TmpText { set; get; }
        private TMP_Dropdown TmpDropdown { set; get; }

        public void Initialize(List<string> options)
        {
            TmpText = parent.GetComponentInChildren<TMP_Text>();
            TmpText.SetText(labelText);

            TmpDropdown = parent.GetComponentInChildren<TMP_Dropdown>();
            TmpDropdown.options.Clear();
            TmpDropdown.AddOptions(options);
            TmpDropdown.RefreshShownValue();
        }

        public string GetCurrentValue()
        {
            return TmpDropdown.options[TmpDropdown.value].text;
        }

        public void Clear()
        {
            TmpDropdown.Select();
            TmpDropdown.value = 0;
            TmpDropdown.RefreshShownValue();
        }
    }
}