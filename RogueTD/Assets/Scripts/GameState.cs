using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class GameState
{
    private static string SAVE_FOLDER;
    private static string SAVE_FILE_PATH;
    private static string BUILDINGS_SAVE_PATH;
    private static string TREE_SAVE_PATH;

    private static bool pathsInitialized = false;

    public static GameState Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameState();
                InitializePaths();
            }
            return _instance;
        }
    }
    private static GameState _instance;
    
    private bool isANewRun;
    private ResearchTree.TreeSaveData treeSaveData;
    private List<BuildingSaveData> buildings = new List<BuildingSaveData>();
    private int _currency;
    private int _wave;
    private EnemyWave currentWave;
    
    private GameState() 
    {
        isANewRun = true;
    }
    
    private static void InitializePaths()
    {
        if (pathsInitialized) return;
        
        #if UNITY_EDITOR
            SAVE_FOLDER = Path.Combine(Application.dataPath, "Saves");
        #else
            SAVE_FOLDER = Path.Combine(Application.persistentDataPath, "Saves");
        #endif
        
        SAVE_FILE_PATH = Path.Combine(SAVE_FOLDER, "gamestate.json");
        BUILDINGS_SAVE_PATH = Path.Combine(SAVE_FOLDER, "buildings.json");
        TREE_SAVE_PATH = Path.Combine(SAVE_FOLDER, "research_tree.json");
        
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
        
        pathsInitialized = true;
    }
    
    public bool IsANewRun 
    { 
        get => isANewRun; 
        set => isANewRun = value;
    }
    
    public ResearchTree.TreeSaveData TreeSaveData 
    { 
        get => treeSaveData; 
        set => treeSaveData = value;
    }
    
    public EnemyWave CurrentWave
    {
        get => currentWave;
        set => currentWave = value;
    }
    
    public List<BuildingSaveData> Buildings 
    { 
        get => buildings; 
        set => buildings = value ?? new List<BuildingSaveData>(); 
    }

    public int Wave
    {
        get => _wave;
        set
        {
            if (value <= 0) return;
            
            _wave = value;
            OnWaveChanged?.Invoke(_wave);
        }
    }

    public int Currency
    {
        get => _currency;
        private set
        {
            _currency = value;
            OnCurrencyChanged?.Invoke(_currency);
        }
    }

    public Action<int> OnWaveChanged;
    public Action<int> OnCurrencyChanged;


    public void ChangeWave(int newWave)
    {
        if (newWave < 0) return;
        
        _wave = newWave;
        OnWaveChanged?.Invoke(_wave);
    }
    
    public void IncrementWave()
    {
        Wave = _wave + 1;
    }
    
    public void ResetWave()
    {
        Wave = 0;
    }

    public void ChangeCurrency(int amount)
    {
        int newCurrency = _currency + amount;
        _currency = newCurrency;
        OnCurrencyChanged?.Invoke(_currency);
    }
    
    public void AddCurrency(int amount)
    {
        if (amount < 0) return;
        ChangeCurrency(amount);
    }
    
    public bool SpendCurrency(int amount)
    {
        if (amount < 0) return false;
        
        if (HasEnoughCurrency(amount))
        {
            ChangeCurrency(-amount);
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public bool HasEnoughCurrency(int amount)
    {
        return _currency >= amount;
    }
    
    public void ResetGameState()
    {
        Buildings = new List<BuildingSaveData>();
        CurrentWave = null;
        _currency = 0;
        _wave = 1;
        isANewRun = true;
        treeSaveData = null;
        
        DeleteAllSaveFiles();
    }
    
    public void Initialize(int startingCurrency = 100, int startingWave = 0)
    {
        _currency = Mathf.Max(0, startingCurrency);
        _wave = Mathf.Max(0, startingWave);
    }

    [Serializable]
    private class GameStateSaveData
    {
        public int Currency;
        public int Wave;
        public bool IsANewRun;
        public string CurrentWaveJson;
    }

    [Serializable]
    private class BuildingsSaveData
    {
        public List<BuildingSaveData> Buildings = new List<BuildingSaveData>();
    }

    private JsonSerializerSettings GetJsonSettings()
    {
        return new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Auto,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            Converters = new List<JsonConverter>
            {
                new Vector2Converter() 
            }
        };
    }

    public void SaveGameState()
    {
        try
        {
            InitializePaths();
            
            var saveData = new GameStateSaveData
            {
                Currency = _currency,
                Wave = _wave,
                IsANewRun = isANewRun,
                CurrentWaveJson = currentWave != null ? JsonConvert.SerializeObject(currentWave, GetJsonSettings()) : ""
            };

            string json = JsonConvert.SerializeObject(saveData, GetJsonSettings());
            File.WriteAllText(SAVE_FILE_PATH, json);
            Debug.Log($"GameState saved to: {SAVE_FILE_PATH}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving GameState: {e.Message}");
        }
    }

    public void SaveBuildings()
    {
        try
        {
            InitializePaths();
            
            UpdateBuildingsHealth();
        
            var saveData = new BuildingsSaveData
            {
                Buildings = buildings
            };

            string json = JsonConvert.SerializeObject(saveData, GetJsonSettings());
            File.WriteAllText(BUILDINGS_SAVE_PATH, json);
            Debug.Log($"Buildings saved to: {BUILDINGS_SAVE_PATH}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving buildings: {e.Message}");
        }
    }

    private void UpdateBuildingsHealth()
    {
        foreach (var saveData in buildings)
        {
            if (ConstructionGridManager.BuildingsPos.TryGetValue(saveData.Position, out Building building))
            {
                saveData.CurrentHealth = building.CurrentHealthPoints;
            }
        }
    }

    public void SaveResearchTree()
    {
        try
        {
            InitializePaths();
            
            if (treeSaveData == null) return;

            string json = JsonConvert.SerializeObject(treeSaveData, GetJsonSettings());
            File.WriteAllText(TREE_SAVE_PATH, json);
            Debug.Log($"Research Tree saved to: {TREE_SAVE_PATH}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving Research Tree: {e.Message}");
        }
    }

    public void SaveAll()
    {
        SaveGameState();
        SaveBuildings();
        SaveResearchTree();
    }

    public bool LoadGameState()
    {
        InitializePaths();
        
        if (!File.Exists(SAVE_FILE_PATH))
        {
            Debug.Log($"GameState file not found: {SAVE_FILE_PATH}");
            return false;
        }

        try
        {
            string json = File.ReadAllText(SAVE_FILE_PATH);
            var saveData = JsonConvert.DeserializeObject<GameStateSaveData>(json, GetJsonSettings());
            
            if (saveData != null)
            {
                _currency = saveData.Currency;
                _wave = saveData.Wave;
                isANewRun = saveData.IsANewRun;
                
                if (!string.IsNullOrEmpty(saveData.CurrentWaveJson))
                {
                    currentWave = JsonConvert.DeserializeObject<EnemyWave>(saveData.CurrentWaveJson, GetJsonSettings());
                }
                
                Debug.Log($"GameState loaded from: {SAVE_FILE_PATH}");
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading GameState: {e.Message}");
        }
        
        return false;
    }

    public bool LoadBuildings()
    {
        InitializePaths();
        
        if (!File.Exists(BUILDINGS_SAVE_PATH))
        {
            Debug.Log($"Buildings file not found: {BUILDINGS_SAVE_PATH}");
            return false;
        }

        try
        {
            string json = File.ReadAllText(BUILDINGS_SAVE_PATH);
            var saveData = JsonConvert.DeserializeObject<BuildingsSaveData>(json, GetJsonSettings());
            
            if (saveData != null)
            {
                buildings = saveData.Buildings ?? new List<BuildingSaveData>();
                ConstructionGridManager.SavePoses = Buildings;
                return true;
            }
            
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading Buildings: {e.Message}");
        }
        
        return false;
    }

    public bool LoadResearchTree()
    {
        InitializePaths();
        
        if (!File.Exists(TREE_SAVE_PATH))
        {
            Debug.Log($"Research Tree file not found: {TREE_SAVE_PATH}");
            return false;
        }

        try
        {
            string json = File.ReadAllText(TREE_SAVE_PATH);
            treeSaveData = JsonConvert.DeserializeObject<ResearchTree.TreeSaveData>(json, GetJsonSettings());
            
            Debug.Log($"Research Tree loaded from: {TREE_SAVE_PATH}");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading Research Tree: {e.Message}");
        }
        
        return false;
    }

    public bool LoadAll()
    {
        bool gameStateLoaded = LoadGameState();
        bool buildingsLoaded = LoadBuildings();
        bool treeLoaded = LoadResearchTree();
        
        return gameStateLoaded || buildingsLoaded || treeLoaded;
    }

    public void DeleteAllSaveFiles()
    {
        InitializePaths();
        
        TryDeleteFile(SAVE_FILE_PATH);
        TryDeleteFile(BUILDINGS_SAVE_PATH);
        TryDeleteFile(TREE_SAVE_PATH);
        Debug.Log("All save files deleted");
    }

    private void TryDeleteFile(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log($"File deleted: {path}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error deleting file {path}: {e.Message}");
        }
    }

    public bool HasSavedData()
    {
        InitializePaths();
        
        return File.Exists(SAVE_FILE_PATH) || 
               File.Exists(BUILDINGS_SAVE_PATH) || 
               File.Exists(TREE_SAVE_PATH);
    }

    public string GetSaveFolderPath()
    {
        InitializePaths();
        return SAVE_FOLDER;
    }
}