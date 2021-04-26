using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using Com.TheFallenGames.OSA.DataHelpers;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// There is 1 important callback you need to implement, apart from Start(): UpdateCellViewsHolder()
// See explanations below
public class CreatureGridAdapter : GridAdapter<GridParams, CreatureGridItemViewsHolder>
{
	// Helper that stores data and notifies the adapter when items count changes
	// Can be iterated and can also have its elements accessed by the [] operator
	public SimpleDataHelper<CreatureModel> Data { get; private set; }

		
	#region GridAdapter implementation
	protected override void Start()
	{
		Data = new SimpleDataHelper<CreatureModel>(this);

		// Calling this initializes internal data and prepares the adapter to handle item count changes
		base.Start();

		// Retrieve the models from your data source and set the items count
		/*
			RetrieveDataAndUpdate(1500);
			*/
	}
	
	
	
	// This is called anytime a previously invisible item become visible, or after it's created, 
	// or when anything that requires a refresh happens
	// Here you bind the data from the model to the item's views
	protected override void UpdateCellViewsHolder(CreatureGridItemViewsHolder newOrRecycled)
	{
			var model = Data[newOrRecycled.ItemIndex];

			newOrRecycled.nameText.text = model.CreatureName;
			newOrRecycled.traitText.text = model.TraitName;
			newOrRecycled.descriptionText.text = model.TraitDescription;
			newOrRecycled.healthValueText.text = "UNDEF";
			newOrRecycled.attackValueText.text = "UNDEF";
			newOrRecycled.intelligenceValueText.text = "UNDEF";
			newOrRecycled.defenceValueText.text = "UNDEF";
			newOrRecycled.speedValueText.text = "UNDEF";
			
			newOrRecycled.classImage.color = Color.cyan;
			newOrRecycled.familyImage.color = Color.cyan;
	}

	protected override void OnCellViewsHolderCreated(CreatureGridItemViewsHolder cellVh,
		CellGroupViewsHolder<CreatureGridItemViewsHolder> cellGroup)
	{
		base.OnCellViewsHolderCreated(cellVh, cellGroup);
		cellVh.button.onClick.AddListener(() => { Debug.Log("clicked: " + cellVh.ItemIndex); });
	}

	// This is the best place to clear an item's views in order to prepare it from being recycled, but this is not always needed, 
	// especially if the views' values are being overwritten anyway. Instead, this can be used to, for example, cancel an image 
	// download request, if it's still in progress when the item goes out of the viewport.
	// <newItemIndex> will be non-negative if this item will be recycled as opposed to just being disabled
	// *For the method's full description check the base implementation
	/*
		protected override void OnBeforeRecycleOrDisableCellViewsHolder(CreatureGridItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
		{
			base.OnBeforeRecycleOrDisableCellViewsHolder(inRecycleBinOrVisible, newItemIndex);
		}
		*/
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
	public TMP_Text healthValueText;
	public TMP_Text attackValueText;
	public TMP_Text intelligenceValueText;
	public TMP_Text defenceValueText;
	public TMP_Text speedValueText;
	public Image classImage;
	public Image familyImage;

	public Button button;


	// Retrieving the views from the item's root GameObject
	public override void CollectViews()
	{
		base.CollectViews();

		var mainPanel = views;
		mainPanel.GetComponentAtPath("Name", out nameText);
		mainPanel.GetComponentAtPath("Trait", out traitText);
		mainPanel.GetComponentAtPath("DescriptionBackground/Description", out descriptionText);
		// FIXME: disable stat values for now, and modify prefab
		mainPanel.GetComponentAtPath("Stats/Health/Value", out healthValueText);
		mainPanel.GetComponentAtPath("Stats/Attack/Value", out attackValueText);
		mainPanel.GetComponentAtPath("Stats/Intelligence/Value", out intelligenceValueText);
		mainPanel.GetComponentAtPath("Stats/Defence/Value", out defenceValueText);
		mainPanel.GetComponentAtPath("Stats/Speed/Value", out speedValueText);
		
		mainPanel.GetComponentAtPath("Images/Class", out classImage);
		mainPanel.GetComponentAtPath("Images/Family", out familyImage);

		button = mainPanel.parent.GetComponent<Button>();
	}
}