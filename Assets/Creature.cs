using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Creature : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text name;
    [SerializeField] private TMP_Text description;

    public bool isSet;

    private CreatureModel creature;

    public void SetCreature(CreatureModel creatureModel)
    {
        isSet = true;
        creature = creatureModel;
        
        SetCreatureImage(image);
        name.text = creature.TraitName;
        description.text = creature.TraitDescription;
        gameObject.GetComponentInParent<CompositeCreature>().RegisterChange();
    }
    
    private void SetCreatureImage(Image image)
    {
        ColorUtility.TryParseHtmlString(isSet ? "#44CC72": "#909090", out var color);
        image.color = color;
    }

    
}
