using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityTemplateProjects;

public class SearchResultUIManager : MonoBehaviour
{
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private GameObject cardPrefab;

    private List<GameObject> rows = new List<GameObject>();
    private float horizontalSpacing;
    private Vector2 cardDimensions;

    void Start()
    {
        horizontalSpacing = rowPrefab.GetComponent<HorizontalLayoutGroup>().spacing;
        cardDimensions = cardPrefab.GetComponent<RectTransform>().sizeDelta;

        //TODO: verify setting x this way does not fuck up size
        rowPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(50, cardDimensions.y);
    }

    public void DrawSearchResult(List<Monster> monsters)
    {
        DestroyAllChildren();
        
        var columnWidth = GetComponent<RectTransform>().sizeDelta.x;
        var cardsPerRow = Mathf.FloorToInt(columnWidth / (cardDimensions.x - horizontalSpacing / 2));

        var numOfRows = Mathf.CeilToInt((float) monsters.Count / cardsPerRow);

        var position = 0;
        for (var i = 0; i < numOfRows; i++)
        {
            var row = Instantiate(rowPrefab, transform, false);
            for (var j = 0; j < cardsPerRow; j++)
            {
                if (position >= monsters.Count) break;
                InstantiateCreatureFromPrefab(monsters[position++], row);
            }
            rows.Add(row);
        }
    }

    private void InstantiateCreatureFromPrefab(Monster monster, GameObject row)
    {
        var template = Instantiate(cardPrefab, row.transform, false);
        template.transform.Find("Name").GetComponent<TMP_Text>().text = monster.Creature;
        template.transform.Find("Trait").GetComponent<TMP_Text>().text = monster.TraitName;
        template.transform.Find("DescriptionBackground").GetComponentInChildren<TMP_Text>().text = monster.TraitDescription;
    }

    private void DestroyAllChildren()
    {
        foreach (var row in rows)
        {
            Destroy(row);
        }
    }
}
