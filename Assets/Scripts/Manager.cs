using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace UnityTemplateProjects
{
    public class Manager : MonoBehaviour
    {
        [SerializeField] private bool resetDb = false;
        [SerializeField] private QueryObject queryObject;

        [SerializeField] private QueryableDropdownField classDropdown;
        [SerializeField] private QueryableDropdownField familyDropdown;
        [SerializeField] private QueryableInputField creatureInput;
        [SerializeField] private QueryableInputField traitInput;
        [SerializeField] private QueryableInputField descriptionInput;

        [SerializeField] private SearchResultUIManager searchResultPanel;

        [SerializeField] private DropdownData classDropdownData;
        [SerializeField] private DropdownData familyDropdownData;

        private DatabaseManager dbManager;
        
        public void Start()
        {
            dbManager = new DatabaseManager(resetDb);

            InitializeDatabase();
            InitializeDropdownLists();
            
            creatureInput.Initialize();
            traitInput.Initialize();
            descriptionInput.Initialize();
        }

        public void RegisterChange()
        {
            queryObject.MClass = classDropdown.GetCurrentValue();
            queryObject.Family = familyDropdown.GetCurrentValue();
            queryObject.SetCreatures(creatureInput.GetCurrentValue());
            queryObject.Trait = traitInput.GetCurrentValue();
            queryObject.SetDescription(descriptionInput.GetCurrentValue());
        }

        //TODO: can probably move code from RegisterChange into Search()
        public void Search()
        {
            var monsters = dbManager.QueryForMonsters(queryObject);
            searchResultPanel.DrawSearchResult(monsters);
        }

        public void Clear()
        {
            queryObject.Clear();
            classDropdown.Clear();
            familyDropdown.Clear();
            creatureInput.Clear();
            traitInput.Clear();
            descriptionInput.Clear();
        }

        private void InitializeDatabase()
        {
            if (!resetDb) return;
            var json = File.ReadAllText(Application.dataPath + "/Data/traits.json");
            var monsters = JsonConvert.DeserializeObject<IEnumerable<Monster>>(json);

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
}