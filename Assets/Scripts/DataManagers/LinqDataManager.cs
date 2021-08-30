using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Constants;
using UnityEngine;

namespace DataManagers
{
    public class LinqDataManager : IDataManager
    {
        private static List<CreatureModel> Creatures { get; set; }

        public void Insert(CreatureModel model)
        {
            Creatures.Add(model);
        }

        public IEnumerable<CreatureModel> QueryForCreatures(CreatureQueryModel queryModel)
        {
            var queryResult = Creatures;

            //queryResult = queryResult.Where(c => !unobtainableClasses.Contains(c.CreatureClass)).ToList();

            if (!string.IsNullOrEmpty(queryModel.MClass))
            {
                queryResult = queryResult.Where(c =>
                        c.CreatureClass.Equals(queryModel.MClass, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(queryModel.Family))
            {
                queryResult = queryResult.Where(c =>
                        c.Family.Equals(queryModel.Family, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(queryModel.Trait))
            {
                var regEx = new Regex("(" + queryModel.Trait + ")", RegexOptions.IgnoreCase);
                queryResult = queryResult.Where(c => regEx.IsMatch(c.TraitName)).ToList();
            }

            if (queryModel.GetCreatures()?.Any() == true)
            {
                var regEx = new Regex(PatternFromList(queryModel.GetCreatures()), RegexOptions.IgnoreCase);
                queryResult = queryResult.Where(c => regEx.IsMatch(c.CreatureName)).ToList();
            }

            if (queryModel.GetDescription()?.Any() == true)
            {
                var regEx = new Regex(PatternFromList(queryModel.GetDescription()), RegexOptions.IgnoreCase);
                queryResult = queryResult.Where(c => regEx.IsMatch(c.TraitDescription)).ToList();
            }

            return queryResult;
        }

        public List<string> QueryDistinctClass()
        {
            return Creatures
                .GroupBy(c => c.CreatureClass)
                .Select(g => g.First().CreatureClass)
                .Where(c => !Settings.UnobtainableClasses.Contains(c))
                .OrderBy(s => s)
                .Prepend("")
                .ToList();
        }

        public List<string> QueryDistinctFamily()
        {
            return Creatures
                .GroupBy(c => c.Family)
                .Select(g => g.First().Family)
                .OrderBy(s => s)
                .Prepend("")
                .ToList();
        }

        public void Initialize(IEnumerable<CreatureModel> creatures)
        {
            var creatureList = creatures.ToList();
            Debug.Log("Populating LinqDataManager list with: " + creatureList.Count + " creatures");        
            Creatures = creatureList.ToList();
        }

        private static string PatternFromList(IEnumerable<string> parameters)
        {
            return "(" + string.Join("|", parameters) + ")";
        }
    }
}