using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class ResearchTree : MonoBehaviour
{
    [SerializeField] public int _maxRank = 7;
    [SerializeField] private int rootCount = 4;
    
    static List<TreeNodeConfig> allAvailableConfigs = new List<TreeNodeConfig>();
    private Dictionary<TreeNodeConfig, float> _weightedNodes = new Dictionary<TreeNodeConfig, float>();
    private HashSet<TreeNodeConfig> _usedUniqueConfigs = new HashSet<TreeNodeConfig>();
    private HashSet<Enums.GroupTags> OpenedGroups  = new HashSet<Enums.GroupTags>();
    [System.Serializable]
    public class TreeSaveData
    {
        [System.Serializable]
        public class TreeSaveNode
        {
            public string currentNodeId;
            public string nodeToUpgradeId;
            public bool IsActive;
            public List<TreeSaveNode> nextSaveNodes;
            public List<TreeSaveNode> visitedNodes;
        
            public TreeSaveNode(TreeNodeConfig config, List<TreeSaveNode> nextNodes, List<TreeSaveNode> visited)
            {
                currentNodeId = config?.name ?? string.Empty;
                nodeToUpgradeId = string.Empty;
                IsActive = false;
                nextSaveNodes = nextNodes ?? new List<TreeSaveNode>();
                visitedNodes = visited ?? new List<TreeSaveNode>();
            }
        
            public TreeSaveNode() 
            {
                nextSaveNodes = new List<TreeSaveNode>();
                visitedNodes = new List<TreeSaveNode>();
            }
        
            [JsonIgnore]
            public TreeNodeConfig currentNodeConfig
            {
                get
                {
                    if (string.IsNullOrEmpty(currentNodeId))
                        return null;
                    
                    return ResourceManager.GetTreeNodeConfig(currentNodeId);
                }
                set
                {
                    currentNodeId = value?.name ?? string.Empty;
                }
            }
        
            [JsonIgnore]
            public TreeNode currentNode { get; set; }
            
            [JsonIgnore]
            public TreeSaveNode nodeToUpgrade { get; set; }
            
            [JsonIgnore]
            public TreeNodeConfig nodeToUpgradeConfig => nodeToUpgrade?.currentNodeConfig;
        }

        public List<TreeSaveNode> rootSaveNodes;
    }

    public void GenerateATree()
    {
        try
        {
            allAvailableConfigs.Clear();
            _usedUniqueConfigs.Clear();
            OpenedGroups.Clear();
            _weightedNodes.Clear();
            GameState.Instance.TreeSaveData = new TreeSaveData
            {
                rootSaveNodes = new List<TreeSaveData.TreeSaveNode>()
            };
            
            LoadAllConfigs();
            
            if (allAvailableConfigs.Count == 0)
            {
                Debug.LogError("No configs loaded from Resources!");
                return;
            }
            
            CreateRoots();
            
            if (GameState.Instance.TreeSaveData.rootSaveNodes == null || GameState.Instance.TreeSaveData.rootSaveNodes.Count == 0)
            {
                Debug.LogError("No root nodes created!");
                return;
            }
            
            CreateBranches();
            
            GameState.Instance.SaveResearchTree();
            Debug.Log($"Tree generated successfully with {GameState.Instance.TreeSaveData.rootSaveNodes.Count} root nodes");
            
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error generating research tree: {e.Message}\n{e.StackTrace}");
        }
    }

    private void CreateRoots()
    {
        GameState.Instance.TreeSaveData.rootSaveNodes.Clear();
        _weightedNodes.Clear();
        
        foreach (TreeNodeConfig config in allAvailableConfigs)
        {
            if (config.UtillityTags?.Contains(Enums.UtillityTags.Tower) == true && 
                config.MinRank == 0 &&
                !IsUniqueConfigUsed(config))
            {
                _weightedNodes[config] = 1;
            }
        }
        
        Debug.Log($"Found {_weightedNodes.Count} potential root configs");
        
        for (var i = 0; i < rootCount && _weightedNodes.Count > 0; i++)
        {
            var rootConfig = GetRandomConfig(_weightedNodes);
            if (rootConfig != null)
            {
                var rootSaveNode = new TreeSaveData.TreeSaveNode(
                    rootConfig, 
                    new List<TreeSaveData.TreeSaveNode>(), 
                    new List<TreeSaveData.TreeSaveNode>()
                );
                
                GameState.Instance.TreeSaveData.rootSaveNodes.Add(rootSaveNode);
                _weightedNodes.Remove(rootConfig);
                
                Debug.Log($"Added root node: {rootConfig.name}");
            }
        }
    }

    public void CreateBranches()
    {
        var currentRank = GameState.Instance.TreeSaveData.rootSaveNodes;
        var nextRank = new List<TreeSaveData.TreeSaveNode>();
        
        for (var rank = 1; rank < _maxRank; rank++)
        {
            var currentRankCopy = new List<TreeSaveData.TreeSaveNode>(currentRank);
            
            foreach (var node in currentRankCopy)
            {
                CreateBranchNode(node, nextRank, rank);
                
                var shouldCreateSideBranch = UnityEngine.Random.value < GetSideBranchProbability(rank);
                if (shouldCreateSideBranch)
                {
                    CreateBranchNode(node, nextRank, rank);
                }
            }
            
            currentRank = nextRank;
            nextRank = new List<TreeSaveData.TreeSaveNode>();
        }
    }

    private void CreateBranchNode(TreeSaveData.TreeSaveNode parentNode, List<TreeSaveData.TreeSaveNode> nextRank, int rank)
    {
        var visited = new List<TreeSaveData.TreeSaveNode>(parentNode.visitedNodes ?? new List<TreeSaveData.TreeSaveNode>());
        visited.Add(parentNode);

        var nextNode = new TreeSaveData.TreeSaveNode(null, new List<TreeSaveData.TreeSaveNode>(), visited);

        parentNode.nextSaveNodes = parentNode.nextSaveNodes ?? new List<TreeSaveData.TreeSaveNode>();
        parentNode.nextSaveNodes.Add(nextNode);
        nextRank.Add(nextNode);

        _weightedNodes.Clear();
        
        // Однопроходный поиск подходящих конфигов
        foreach (var config in allAvailableConfigs)
        {
            if (parentNode.currentNodeConfig?.GroupTags != null && config?.IncompatabillityTags != null)
            {
                if (parentNode.currentNodeConfig.GroupTags.Intersect(config.IncompatabillityTags).Any())
                {
                    continue; 
                }
            }

            if (config.GroupTags != null && OpenedGroups.Intersect(config.GroupTags).Any())
            {
                var skip = true;
                foreach (var tag in OpenedGroups.Intersect(config.GroupTags))
                {
                    foreach (var node in visited)
                    {
                        if (node.currentNodeConfig?.GroupTags?.Contains(tag) == true)
                        {
                            skip = false;
                            break;        
                        }
                    }
                    if (!skip) break;
                }
                if (skip) { continue; }
            }

            if (!IsUniqueConfigUsed(config))
            {
                float weight = CalculateBaseWeight(config, parentNode.currentNodeConfig);
                if (config is UpgradeTreeNodeConfig upgradeConfig)
                {
                    TreeSaveData.TreeSaveNode bestTowerNode = null;
                    float bestTowerWeight = 0f;
                    
                    // Проверяем все посещенные ноды включая родительскую
                    foreach (var visitedNode in visited)
                    {
                        if (upgradeConfig.CheckCompatability(visitedNode.currentNodeConfig))
                        {
                                // Вес башни: ближе к текущей ноде = больше вес
                            float towerWeight = 1f;
                            if (visitedNode == parentNode) towerWeight = 3f; // Родительская башня
                            else if (visitedNode == visited?.LastOrDefault())  towerWeight = 2f; // Последняя посещенная
                                
                            if (towerWeight > bestTowerWeight)
                            { 
                                bestTowerWeight = towerWeight;
                                bestTowerNode = visitedNode;
                            }
                        }
                        
                    }
                    
                    if (bestTowerNode != null)
                    {
                        weight += bestTowerWeight * 2f; // Бонус за наличие подходящей башни
                        nextNode.nodeToUpgrade = bestTowerNode;
                    }
                    else
                    {
                        continue; // Пропускаем апгрейд без подходящей башни
                    }
                }
                
                _weightedNodes[config] = weight;
            }
        }

        if (_weightedNodes.Count > 0)
        {
            var selectedConfig = GetRandomConfig(_weightedNodes);
            if (selectedConfig)
            {
                nextNode.currentNodeConfig = selectedConfig;
                
                // Если это апгрейд-нода, nodeToUpgrade уже должен быть установлен выше
                Debug.Log($"Created branch node: {selectedConfig.name} {(nextNode.nodeToUpgrade != null ? $"-> Upgrading: {nextNode.nodeToUpgrade.currentNodeConfig?.name}" : "")}");
            }
        }
    }
    
    private float CalculateBaseWeight(TreeNodeConfig config, TreeNodeConfig parentConfig)
    {
        if (!parentConfig || !config) return 1f;
        
        float weight = 1f;
        
        if (parentConfig.DirectUpgradeOf == config)
        {
            weight += allAvailableConfigs.Count * 0.25f;
        }
        
        if (parentConfig.GroupTags != null && config.GroupTags != null)
        {
            int commonTags = parentConfig.GroupTags.Intersect(config.GroupTags).Count();
            weight += commonTags * 3f;
        }
        
        return weight;
    }

    private float GetSideBranchProbability(int rank)
    {
        switch (rank)
        {
            case 1: return 0.50f;
            case 2: return 0.40f;
            case 3: return 0.30f;
            case 4: return 0.20f;
            case 5: return 0.15f;
            case 6: return 0.1f;
            default: return 0.05f;
        }
    }
    
    private void LoadAllConfigs()
    {
        allAvailableConfigs.Clear();
        _usedUniqueConfigs.Clear();
        OpenedGroups.Clear();
        var configs = Resources.LoadAll<TreeNodeConfig>("Nodes");
        allAvailableConfigs.AddRange(configs);
        allAvailableConfigs.RemoveAll(config => IsUniqueConfigUsed(config));
        Debug.Log($"Loaded {allAvailableConfigs.Count} configs from Resources");
    }

    private bool IsUniqueConfigUsed(TreeNodeConfig config)
    {
        return config.UtillityTags?.Contains(Enums.UtillityTags.Unique) == true && _usedUniqueConfigs.Contains(config);
    }
     
    private TreeNodeConfig GetRandomConfig(Dictionary<TreeNodeConfig, float> weightedConfigs)
    {
        if (weightedConfigs == null || weightedConfigs.Count == 0)
            return null;

        var totalWeight = weightedConfigs.Values.Sum();
        
        if (totalWeight <= 0)
            return null;

        var randomValue = UnityEngine.Random.Range(0f, totalWeight);
        var accumulatedWeight = 0f;

        foreach (var kvp in weightedConfigs)
        {
            accumulatedWeight += kvp.Value;
            if (randomValue < accumulatedWeight)
            {
                var selectedConfig = kvp.Key;
                if (selectedConfig.UtillityTags?.Contains(Enums.UtillityTags.Unique) == true)
                {
                    _usedUniqueConfigs.Add(selectedConfig);
                    allAvailableConfigs.Remove(selectedConfig);
                }
                if (selectedConfig.GroupTags != null)
                {
                    foreach (var tag in selectedConfig.GroupTags)
                    { 
                        OpenedGroups.Add(tag);
                    }
                }
                return selectedConfig;
            }
        }

        var configToReturn = weightedConfigs.Keys.Last();
        if (configToReturn.UtillityTags?.Contains(Enums.UtillityTags.Unique) == true)
        {
            _usedUniqueConfigs.Add(configToReturn);
            allAvailableConfigs.Remove(configToReturn);
        }
        if (configToReturn.GroupTags != null)
        {
            foreach (var tag in configToReturn.GroupTags)
            { 
                OpenedGroups.Add(tag);
            }
        }
        return configToReturn;
    }
}