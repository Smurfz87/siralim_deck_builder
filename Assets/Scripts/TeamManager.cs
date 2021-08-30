using System;
using System.Collections;
using System.Collections.Generic;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamManager : MonoBehaviour
{

    [SerializeField] private GameObject[] team;
    [SerializeField] private CompositeCreature[] creatures;
    
    private Manager manager;
    
    private Creature teamMember;
    
    public void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
    }

    public void RegisterClickedTeamPosition(Creature teamSlot)
    {
        teamMember = teamSlot;
    }

    public void ChangeTeamMember(CreatureModel model)
    { 
        teamMember.SetCreature(model);
        /**
        var image = teamMember.transform.Find("Image").GetComponent<Image>();
        SetCreatureImage(image);

        var trait = teamMember.transform.Find("Name");
        var description = trait.Find("Description");

        trait.GetComponent<TMP_Text>().text = model.TraitName;
        description.GetComponent<TMP_Text>().text = model.TraitDescription;

        setClassAndFamilyType();
        **/

    }

    // TODO: replace with creature sprites once public
    private void SetCreatureImage(Image image)
    {
        ColorUtility.TryParseHtmlString(teamMember != null ? "#44CC72": "#909090", out var color);
        image.color = color;
    }

    private void setClassAndFamilyType()
    {
        var creatureTypingPath = teamMember.transform.parent.parent.GetChild(0);

        var clazz = creatureTypingPath.GetChild(0).GetChild(0).GetComponent<Image>();
        SetCreatureImage(clazz);
        var family = creatureTypingPath.GetChild(1).GetChild(0).GetComponent<Image>();
        SetCreatureImage(family);
    }
}
