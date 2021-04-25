using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityTemplateProjects;

public class Manager : MonoBehaviour
{
    [SerializeField] private bool resetDb = false;

    [SerializeField] private QueryableDropdownField classDropdown;
    [SerializeField] private QueryableDropdownField familyDropdown;
    [SerializeField] private QueryableInputField creatureInput;
    [SerializeField] private QueryableInputField traitInput;
    [SerializeField] private QueryableInputField descriptionInput;

    [SerializeField] private DropdownData classDropdownData;
    [SerializeField] private DropdownData familyDropdownData;

    [SerializeField] private CreatureGridAdapter gridAdapter;

    private DatabaseManager dbManager;
    private CreatureQuery creatureQuery;

    public void Start()
    {
        creatureQuery = new CreatureQuery();
        dbManager = new DatabaseManager(resetDb);

        InitializeDatabase();
        InitializeDropdownLists();

        creatureInput.Initialize();
        traitInput.Initialize();
        descriptionInput.Initialize();
    }

    public void RegisterChange()
    {
        creatureQuery.MClass = classDropdown.GetCurrentValue();
        creatureQuery.Family = familyDropdown.GetCurrentValue();
        creatureQuery.SetCreatures(creatureInput.GetCurrentValue());
        creatureQuery.Trait = traitInput.GetCurrentValue();
        creatureQuery.SetDescription(descriptionInput.GetCurrentValue());
    }

    //TODO: can probably move code from RegisterChange into Search()
    public void Search()
    {
        var monsters = dbManager.QueryForCreatures(creatureQuery);
        gridAdapter.OnDataChanged(monsters);
    }

    public void Clear()
    {
        creatureQuery.Clear();
        classDropdown.Clear();
        familyDropdown.Clear();
        creatureInput.Clear();
        traitInput.Clear();
        descriptionInput.Clear();
    }

    private void InitializeDatabase()
    {
        if (!resetDb) return;
        var json = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "traits.json"));
        var monsters = JsonConvert.DeserializeObject<IEnumerable<CreatureModel>>(json);

        if (monsters == null)
        {
            Debug.LogError("No data found!");
            return;
        }

        foreach (var monster in monsters)
        {
            dbManager.Insert(monster);
        }
    }

    private void InitializeDropdownLists()
    {
        if (resetDb || classDropdownData.Data == null || classDropdownData.Data.Count <= 0)
        {
            var classes = dbManager.QueryForColumn(Query
                .SELECT_FIELD_FROM_MONSTERS
                .Replace("{field}", "distinct class"));
            classes.Sort((s1, s2) => string.Compare(s1, s2, StringComparison.Ordinal));
            classes.Insert(0, "");
            classDropdownData.Data = classes;
        }

        classDropdown.Initialize(classDropdownData.Data);


        if (resetDb || familyDropdownData.Data == null || familyDropdownData.Data?.Count <= 0)
        {
            var families = dbManager.QueryForColumn(Query
                .SELECT_FIELD_FROM_MONSTERS
                .Replace("{field}", "distinct family"));
            families.Sort((s1, s2) => string.Compare(s1, s2, StringComparison.Ordinal));
            families.Insert(0, "");
            familyDropdownData.Data = families;
        }

        familyDropdown.Initialize(familyDropdownData.Data);
    }
}