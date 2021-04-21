using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UnityTemplateProjects
{
    [CreateAssetMenu(fileName = "QDropdownField_", menuName = "field/queryableDropdown", order = 0)]
    public class QueryableDropdownField : ScriptableObject
    {
        public string labelText;

        public GameObject parent;

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

        public IEnumerator Clear()
        {
            TmpDropdown.Select();
            yield return new WaitForEndOfFrame();
            TmpDropdown.value = 0;
            TmpDropdown.RefreshShownValue();
        }
    }
}