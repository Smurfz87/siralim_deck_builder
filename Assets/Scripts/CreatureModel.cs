using System.Text;
using Newtonsoft.Json;

public class CreatureModel
{
    [JsonProperty("class")]
    public string CreatureClass { set; get; }
    [JsonProperty("family")]
    public string Family { set; get; }
    [JsonProperty("creature")]
    public string CreatureName { set; get; }
    [JsonProperty("trait_name")]
    public string TraitName { set; get; }
    [JsonProperty("trait_description")]
    public string TraitDescription { set; get; }
    [JsonProperty("material_name")]
    public string MaterialName { set; get; }

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