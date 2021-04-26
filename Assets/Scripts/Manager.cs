using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityTemplateProjects;

public class Manager : MonoBehaviour
{
    [SerializeField] private bool resetDb = false;
    [SerializeField] private bool useSqlite = false;

    [SerializeField] private QueryableDropdownField classDropdown;
    [SerializeField] private QueryableDropdownField familyDropdown;
    [SerializeField] private QueryableInputField creatureInput;
    [SerializeField] private QueryableInputField traitInput;
    [SerializeField] private QueryableInputField descriptionInput;

    [SerializeField] private DropdownData classDropdownData;
    [SerializeField] private DropdownData familyDropdownData;

    [SerializeField] private CreatureGridAdapter gridAdapter;
    [SerializeField] private UIManager uiManager;

    private IDataManager dataManager;
    private CreatureQueryModel creatureQueryModel;

    public void Start()
    {
        creatureQueryModel = new CreatureQueryModel();
        if (useSqlite && Application.platform != RuntimePlatform.WebGLPlayer)
        {
            dataManager = new SqliteDataManager();
        }
        else
        {
            dataManager = new LinqDataManager();
            useSqlite = false;
        }

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Debug.Log("Is WebGL");
            StartCoroutine(InitializeWebGL());
        }
        else
        {
            var data = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "traits.json"));
            Initialize(data);
        }
    }

    private IEnumerator InitializeWebGL()
    {
        var request = UnityWebRequest.Get(Application.dataPath + "/traits.json");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            throw new WebException("Failed to load file");
        }

        Debug.Log("Successfully got file from server");

        Initialize(request.downloadHandler.text);
    }

    private void Initialize(string data)
    {
        InitializeDataManager(data);
        Debug.Log("Initialized data manager: " + dataManager.GetType().Name);
        InitializeDropdownLists();
        Debug.Log("Initialized UI dropdown lists");
        creatureInput.Initialize();
        traitInput.Initialize();
        descriptionInput.Initialize();
        Debug.Log("Initialized UI inputs");
    }

    public void RegisterChange()
    {
        creatureQueryModel.MClass = classDropdown.GetCurrentValue();
        creatureQueryModel.Family = familyDropdown.GetCurrentValue();
        creatureQueryModel.SetCreatures(creatureInput.GetCurrentValue());
        creatureQueryModel.Trait = traitInput.GetCurrentValue();
        creatureQueryModel.SetDescription(descriptionInput.GetCurrentValue());
    }

    //TODO: can probably move code from RegisterChange into Search()
    public void Search()
    {
        var monsters = dataManager.QueryForCreatures(creatureQueryModel).ToList();
        Debug.Log("Search results: " + monsters.Count);
        uiManager.ShowMessage("Found " + monsters.Count + " matching creatures!");
        gridAdapter.OnDataChanged(monsters);
    }

    public void Clear()
    {
        creatureQueryModel.Clear();
        classDropdown.Clear();
        familyDropdown.Clear();
        creatureInput.Clear();
        traitInput.Clear();
        descriptionInput.Clear();
    }

    private void InitializeDataManager(string json)
    {
        //FIXME: Ensure commented out before release, prints content of file
        //Debug.Log(json);
        if (!resetDb && useSqlite) return;
        var creatures = JsonConvert.DeserializeObject<IEnumerable<CreatureModel>>(json);
        Debug.Log("Deserialized JSON to Creature list");

        if (creatures == null)
        {
            Debug.LogError("No data found!");
            throw new InvalidDataException("No data found!");
        }

        dataManager.Initialize(creatures);
    }

    private void InitializeDropdownLists()
    {
        if (resetDb || classDropdownData.Data == null || classDropdownData.Data.Count <= 0)
        {
            var classes = dataManager.QueryDistinctClass();
            classDropdownData.Data = classes;
        }

        classDropdown.Initialize(classDropdownData.Data);


        if (resetDb || familyDropdownData.Data == null || familyDropdownData.Data?.Count <= 0)
        {
            var families = dataManager.QueryDistinctFamily();
            familyDropdownData.Data = families;
        }

        familyDropdown.Initialize(familyDropdownData.Data);
    }
}