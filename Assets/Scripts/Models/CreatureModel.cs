using System;
using System.Text;
using Newtonsoft.Json;

public class CreatureModel
{
    [JsonProperty("class")]
    public string CreatureClass { get; set; }

    [JsonProperty("family")]
    public string Family { get; set; }

    [JsonProperty("creature")]
    public string CreatureName { get; set; }

    [JsonProperty("trait_name")]
    public string TraitName { get; set; }

    [JsonProperty("trait_description")]
    public string TraitDescription { get; set; }

    [JsonProperty("material_name")]
    public string MaterialName { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("Class: ");
        sb.Append(CreatureClass);
        sb.Append("; Family: ");
        sb.Append(Family);
        sb.Append("; Creature: ");
        sb.Append(CreatureName);
        sb.Append("; Trait: ");
        sb.Append(TraitName);
        sb.Append("; Description: ");
        sb.Append(TraitDescription);
        sb.Append("; Material: ");
        sb.Append(MaterialName);
        return sb.ToString();
    }
}