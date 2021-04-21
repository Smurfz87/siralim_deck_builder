using TMPro;
using UnityEngine;

namespace UnityTemplateProjects
{
    //[CreateAssetMenu(fileName = "QInputField_", menuName = "field/queryableInput", order = 0)]
    public class QueryableInputField : MonoBehaviour
    {
        public string labelText;

        public GameObject parent;
        
        private TMP_Text TmpText { set; get; }
        private TMP_InputField TmpInputField { set; get; }

        public void Initialize()
        {
            TmpText = parent.GetComponentInChildren<TMP_Text>();
            TmpText.SetText(labelText);

            TmpInputField = parent.GetComponentInChildren<TMP_InputField>();
        }

        public void Clear()
        {
            TmpInputField.text = "";
        }

        public string GetCurrentValue()
        {
            return TmpInputField.text;
        }
    }
}