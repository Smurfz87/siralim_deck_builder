
using System.Text;
using Newtonsoft.Json;

namespace UnityTemplateProjects
{
    public class Monster
    {
        [JsonProperty("class")]
        public string MonsterClass { set; get; }
        [JsonProperty("family")]
        public string Family { set; get; }
        [JsonProperty("creature")]
        public string Creature { set; get; }
        [JsonProperty("trait_name")]
        public string TraitName { set; get; }
        [JsonProperty("trait_description")]
        public string TraitDescription { set; get; }
        [JsonProperty("material_name")]
        public string MaterialName { set; get; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class: ");
            sb.Append(MonsterClass);
            sb.Append("; family: ");
            sb.Append(Family);
            sb.Append("; Creature: ");
            sb.Append(Creature);
            sb.Append("; Trait: ");
            sb.Append(TraitName);
            sb.Append("; Description: ");
            sb.Append(TraitDescription);
            sb.Append("; Material: ");
            sb.Append(MaterialName);
            return sb.ToString();
        }
    }
}