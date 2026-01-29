using System;
using System.Linq;

public abstract class UpgradeTreeNodeConfig : TreeNodeConfig
{
    public abstract bool CheckCompatability(TreeNodeConfig treeNodeConfig);

    
    protected bool HasEffectOfType<TResource>(TreeNodeConfig config, params Type[] types ) where TResource : Resource
    {
        foreach (var type in types)
        {
            if (config.GetType() == type)
            {
                var resources = config.GetConfigResources();
                return resources.Any(r => r is TResource);
            }
        }
        return false;
    }
}