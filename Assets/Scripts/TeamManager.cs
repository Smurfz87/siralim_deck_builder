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
    }

    // TODO: replace with creature sprites once public
    private void SetCreatureImage(Image image)
    {
        ColorUtility.TryParseHtmlString(teamMember != null ? "#44CC72": "#909090", out var color);
        image.color = color;
    }
}
