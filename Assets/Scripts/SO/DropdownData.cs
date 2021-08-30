using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropdownData_", menuName = "Data/List", order = 0)]
public class DropdownData : ScriptableObject
{
    public List<string> Data { set; get; }
}