using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        //[SerializeField] private QueryableInputField traitInput;
        //[SerializeField] private QueryableInputField descriptionInput;

        private DatabaseManager dbManager;
        
        public void Start()
        {
            dbManager = new DatabaseManager(resetDb);

            InitializeDatabase(dbManager);
            InitializeDropdownLists(dbManager);
            
            creatureInput.Initialize();
            
        }

        public void RegisterChange()
        {
            queryObject.MClass = classDropdown.GetCurrentValue();
            queryObject.Family = familyDropdown.GetCurrentValue();
            queryObject.SetCreatures(creatureInput.GetCurrentValue());
            //queryObject.Trait = "";
        }

        //TODO: can probably move code from RegisterChange into Search()
        public void Search()
        {
            var monsters = dbManager.QueryForMonsters(queryObject);
            foreach (var monster in monsters)
            {
                Debug.Log(monster);
            }
        }

        public void Clear()
        {
            queryObject.Clear();
            classDropdown.Clear();
            familyDropdown.Clear();
            creatureInput.Clear();
        }

        private void InitializeDatabase(DatabaseManager dbManager)
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

        private void InitializeDropdownLists(DatabaseManager dbManager)
        {
            if (resetDb)
            {
                var classes = dbManager.QueryForColumn(Query
                    .SELECT_FIELD_FROM_MONSTERS
                    .Replace("{field}", "distinct class"));
                classes.Sort((s1, s2) => string.Compare(s1, s2, StringComparison.Ordinal));
                classes.Insert(0, "");
                classDropdown.Initialize(classes);
            }

            if (resetDb)
            {
                var families = dbManager.QueryForColumn(Query
                    .SELECT_FIELD_FROM_MONSTERS
                    .Replace("{field}", "distinct family"));
                families.Sort((s1, s2) => string.Compare(s1, s2, StringComparison.Ordinal));
                families.Insert(0, "");
                familyDropdown.Initialize(families);
            }
        }
    }
}