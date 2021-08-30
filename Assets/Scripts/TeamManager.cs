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
    
    private Manager manager;

    private GameObject teamMember;
    private int position;
    
    public void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
    }

    public void SetTeamMemberBeingChanged(GameObject teamMember, int position)
    {
        this.teamMember = teamMember;
        this.position = position;
    }

    public void ChangeTeamMember(CreatureModel model)
    {
        var image = teamMember.transform.Find("Image").GetComponent<Image>();
        SetCreatureImage(image);

        var trait = teamMember.transform.Find("Name");
        var description = trait.Find("Description");

        trait.GetComponent<TMP_Text>().text = model.TraitName;
        description.GetComponent<TMP_Text>().text = model.TraitDescription;
    }

    // TODO: replace with creature sprites once public
    private void SetCreatureImage(Image image)
    {
        ColorUtility.TryParseHtmlString(teamMember != null ? "#44CC72": "#909090", out var color);
        image.color = color;
    }
}
