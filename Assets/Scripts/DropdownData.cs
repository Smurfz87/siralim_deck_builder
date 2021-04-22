using System.Collections.Generic;
using UnityEngine;

namespace UnityTemplateProjects
{
    [CreateAssetMenu(fileName = "DropdownData_", menuName = "Data/List", order = 0)]
    public class DropdownData : ScriptableObject
    {
        public List<string> Data { set; get; }
    }
}