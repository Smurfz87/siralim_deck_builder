using System.Collections.Generic;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using Com.TheFallenGames.OSA.DataHelpers;
using frame8.Logic.Misc.Other.Extensions;
using SO;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

// There is 1 important callback you need to implement, apart from Start(): UpdateCellViewsHolder()
// See explanations below
public class CreatureGridAdapter : GridAdapter<GridParams, CreatureGridItemViewsHolder>
{
	// Helper that stores data and notifies the adapter when items count changes
	// Can be iterated and can also have its elements accessed by the [] operator
	private SimpleDataHelper<CreatureModel> Data { get; set; }

	[SerializeField] private SpriteDictionary classSprites;
	[SerializeField] private SpriteDictionary familySprites;
	
	[SerializeField] private TeamManager teamManager;

	#region GridAdapter implementation
	protected override void Start()
	{
		Data = new SimpleDataHelper<CreatureModel>(this);

		// Calling this initializes internal data and prepares the adapter to handle item count changes
		base.Start();
	}
	
	
	
	// This is called anytime a previously invisible item become visible, or after it's created, 
	// or when anything that requires a refresh happens
	// Here you bind the data from the model to the item's views
	protected override void UpdateCellViewsHolder(CreatureGridItemViewsHolder newOrRecycled)
	{
			var model = Data[newOrRecycled.ItemIndex];

			newOrRecycled.model = model;

			newOrRecycled.nameText.text = model.CreatureName;
			newOrRecycled.traitText.text = model.TraitName;
			newOrRecycled.descriptionText.text = model.TraitDescription;
			// newOrRecycled.healthValueText.text = "UNDEF";
			// newOrRecycled.attackValueText.text = "UNDEF";
			// newOrRecycled.intelligenceValueText.text = "UNDEF";
			// newOrRecycled.defenceValueText.text = "UNDEF";
			// newOrRecycled.speedValueText.text = "UNDEF";
			ColorUtility.TryParseHtmlString("#515161", out var color);
			newOrRecycled.classBGImage.color = color;
			newOrRecycled.familyBGImage.color = color;
			newOrRecycled.classBGImage.gameObject.GetComponent<MouseOverTooltip>().SetText(model.CreatureClass);
			newOrRecycled.familyBGImage.gameObject.GetComponent<MouseOverTooltip>().SetText(model.Family);

			ChangeSpriteIfPresent(classSprites, newOrRecycled.classImage, model.CreatureClass);
			ChangeSpriteIfPresent(familySprites, newOrRecycled.familyImage, model.Family);
	}

	private static void ChangeSpriteIfPresent(SpriteDictionary dictionary, Image image, string value)
	{
		if (dictionary.sprites.ContainsKey(value))
		{
			image.sprite = dictionary.sprites[value];
			image.enabled = true;
		}
		else
		{
			image.enabled = false;
		}
	}

	protected override void OnCellViewsHolderCreated(CreatureGridItemViewsHolder cellVh,
		CellGroupViewsHolder<CreatureGridItemViewsHolder> cellGroup)
	{
		base.OnCellViewsHolderCreated(cellVh, cellGroup);
		cellVh.button.onClick.AddListener(() =>
		{
			Debug.Log(UnityEngine.Time.unscaledTime + ": clicked: " + cellVh.ItemIndex + " Data: " + Data[cellVh.ItemIndex]);
			teamManager.ChangeTeamMember(Data[cellVh.ItemIndex]);
		});
	}
	#endregion

	public void OnDataChanged(IEnumerable<CreatureModel> data)
	{
		Data.List.Clear();
		Data.List.AddRange(data);
		Data.NotifyListChangedExternally();
	}
}


// This class keeps references to an item's views.
// Your views holder should extend BaseItemViewsHolder for ListViews and CellViewsHolder for GridViews
public class CreatureGridItemViewsHolder : CellViewsHolder
{
	public TMP_Text nameText;
	public TMP_Text traitText;
	public TMP_Text descriptionText;
	// public TMP_Text healthValueText;
	// public TMP_Text attackValueText;
	// public TMP_Text intelligenceValueText;
	// public TMP_Text defenceValueText;
	// public TMP_Text speedValueText;
	public Image classBGImage;
	public Image classImage;
	public Image familyBGImage;
	public Image familyImage;

	public Button button;
	public CreatureModel model;


	// Retrieving the views from the item's root GameObject
	public override void CollectViews()
	{
		base.CollectViews();

		var mainPanel = views;
		var root = "Panel/";
		mainPanel.GetComponentAtPath(root + "Name", out nameText);
		mainPanel.GetComponentAtPath(root + "Trait", out traitText);
		mainPanel.GetComponentAtPath(root + "DescriptionBackground/Description", out descriptionText);
		// FIXME: disable stat values for now, and modify prefab
		/*mainPanel.GetComponentAtPath("Stats/Health/Value", out healthValueText);
		mainPanel.GetComponentAtPath("Stats/Attack/Value", out attackValueText);
		mainPanel.GetComponentAtPath("Stats/Intelligence/Value", out intelligenceValueText);
		mainPanel.GetComponentAtPath("Stats/Defence/Value", out defenceValueText);
		mainPanel.GetComponentAtPath("Stats/Speed/Value", out speedValueText);*/
		
		mainPanel.GetComponentAtPath(root + "Images/ClassBG", out classBGImage);
		mainPanel.GetComponentAtPath(root + "Images/ClassBG/Image", out classImage);
		mainPanel.GetComponentAtPath(root + "Images/FamilyBG", out familyBGImage);
		mainPanel.GetComponentAtPath(root + "Images/FamilyBG/Image", out familyImage);

		button = mainPanel.parent.GetComponent<Button>();
	}
}