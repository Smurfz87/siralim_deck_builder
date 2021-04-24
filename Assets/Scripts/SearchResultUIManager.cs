using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityTemplateProjects;

public class SearchResultUIManager : MonoBehaviour
{
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private GameObject cardPrefab;

    private RectTransform containerPos;
    private Vector2 cardDimensions;
    private float horizontalSpacing;
    private float maxY;
    private float minY;

    private int cardsPerRow;
    private int rowsToKeep;

    private float previousY;
    
    private List<CreatureModel> fullList = new List<CreatureModel>();
    private List<CreatureModel> inView = new List<CreatureModel>();

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entered");
    }

    public void Start()
    {
        containerPos = GetComponent<RectTransform>();
        horizontalSpacing = rowPrefab.GetComponent<HorizontalLayoutGroup>().spacing;
        cardDimensions = cardPrefab.GetComponent<RectTransform>().sizeDelta;
        maxY = cardDimensions.y * 4;
        minY = cardDimensions.y * 2;

        // FIXME: make rowsToKeep reflect number of rows that fit on screen + 3 on each side
        rowsToKeep = Mathf.CeilToInt(cardDimensions.y + GetComponent<VerticalLayoutGroup>().spacing);
        rowPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(50, cardDimensions.y);
    }

    public void FixedUpdate()
    {
        if (!transform.hasChanged || transform.childCount == 0) return; //anchoredPosition
        var anchorPos = containerPos.anchoredPosition;
        var directionUp = anchorPos.y > previousY;
        if (anchorPos.y > maxY)
        {
            Destroy(transform.GetChild(0).gameObject);
            RemoveCreaturesInRow(cardsPerRow, true);
            var indexOf = fullList.IndexOf(inView.Last());
            InstantiateRow(indexOf + 1, false);

            containerPos.anchoredPosition = new Vector2(anchorPos.x, anchorPos.y -= cardDimensions.y);
        }
        else if (anchorPos.y < minY && transform.childCount >= 10)
        {
            var row = transform.GetChild(transform.childCount - 1).gameObject;
            Destroy(row);
            RemoveCreaturesInRow(cardsPerRow, false);
        }

  

        transform.hasChanged = false;
    }

    private void RemoveCreaturesInRow(int numOfChildren, bool atTop)
    {
        if (atTop)
        {
            inView.RemoveRange(0, numOfChildren);
        }
        else
        {
            inView.RemoveRange(
                inView.Count - numOfChildren - 1,
                numOfChildren);
        }
    }

    private int GetCardsPerRow()
    {
        var columnWidth = GetComponent<RectTransform>().rect.width;
        return Mathf.FloorToInt(columnWidth / (cardDimensions.x - horizontalSpacing / 2));
    }

    public void PopulateWithSearchResult(List<CreatureModel> creatures)
    {
        fullList = creatures;
        DestroyAllChildren();

        cardsPerRow = GetCardsPerRow();
        // FIXME: configure loop properly
        for (var i = 0; i < 10 * cardsPerRow; i += cardsPerRow)
        {
            InstantiateRow(i, false);
        }
        
    }

    public void DrawSearchResult(List<CreatureModel> monsters)
    {
        DestroyAllChildren();

        cardsPerRow = GetCardsPerRow();


        // FIXME: hardcoded for test
        var numOfRows = Mathf.CeilToInt((float) monsters.Count / cardsPerRow);
        //const int numOfRows = 10;

        var position = 0;
        for (var i = 0; i < numOfRows; i++)
        {
            var row = Instantiate(rowPrefab, transform, false);
            for (var j = 0; j < cardsPerRow; j++)
            {
                if (position >= monsters.Count) break;
                InstantiateCreatureFromPrefab(monsters[position], row, position++);
            }
        }
    }

    private void InstantiateRow(int creatureIndex, bool atTop)
    {
        var row = Instantiate(rowPrefab, transform, false);
        if (atTop)
        {
            row.transform.SetSiblingIndex(0);
        }

        for (var i = 0; i < cardsPerRow; i++)
        {
            var creature = fullList[creatureIndex];
            
            inView.Add(creature);
            InstantiateCreatureFromPrefab(creature, row, creatureIndex++);
        }
    }

    private void InstantiateCreatureFromPrefab(CreatureModel creatureModel, GameObject row, int position)
    {
        var template = Instantiate(cardPrefab, row.transform, false);
        template.transform.Find("Name").GetComponent<TMP_Text>().text = creatureModel.CreatureName;
        template.transform.Find("Trait").GetComponent<TMP_Text>().text = creatureModel.TraitName;
        template.transform
            .Find("DescriptionBackground")
            .GetComponentInChildren<TMP_Text>().text = creatureModel.TraitDescription;

        template.GetComponent<Button>().AddClickListener(position, ItemClickedLog);
    }

    private void ItemClickedLog(int index)
    {
        Debug.Log("clicked " + index);
    }

    private void DestroyAllChildren()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
    }
}