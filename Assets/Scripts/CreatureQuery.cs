using System;
using System.Collections.Generic;
using System.Linq;

public class CreatureQuery
{
    public string MClass { get; set; }
    public string Family { get; set; }
    public string Trait { get; set; }

    private List<string> creatures;
    private List<string> description;

    private static readonly string[] CharsToSplitOn = { ".", ";", "," };

    /// <summary>
    /// Transforms input to list by removing whitespaces
    /// and splitting the string by:
    ///     - full stop (.)
    ///     - semicolon (;)
    ///     - comma (,)
    /// </summary>
    /// <param name="creatureString"></param>
    public void SetCreatures(string creatureString)
    {
        creatures = SplitToList(RemoveWhitespace(creatureString));
    }

    public IEnumerable<string> GetCreatures()
    {
        return creatures;
    }

    /// <summary>
    /// Transforms input to list by splitting the string by:
    ///     - full stop (.)
    ///     - semicolon (;)
    ///     - comma (,)
    /// </summary>
    /// <param name="descriptionString"></param>
    public void SetDescription(string descriptionString)
    {
        description = SplitToList(descriptionString);
    }

    public IEnumerable<string> GetDescription()
    {
        return description;
    }

    private static List<string> SplitToList(string input)
    {
        return new List<string>(input.Split(CharsToSplitOn, StringSplitOptions.None));
    }

    private static string RemoveWhitespace(string input)
    {
        return string.Concat(input.Where(c => !char.IsWhiteSpace(c)));
    }

    public void Clear()
    {
        MClass = "";
        Family = "";
        Trait = "";
        creatures?.RemoveAll(s => true);
        description?.RemoveAll(s => true);
    }
}