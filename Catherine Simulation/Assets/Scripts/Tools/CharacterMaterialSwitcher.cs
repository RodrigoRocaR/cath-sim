using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public static class CharacterMaterialSwitcher
    {
        private static readonly Dictionary<PlayerIdentity, string> _playerSkins = new Dictionary<PlayerIdentity, string>
        {
            { PlayerIdentity.Player, "Materials/Player"},
            { PlayerIdentity.AI, "Materials/AI1"}
        };

        public static void Switch(GameObject player, PlayerIdentity pi)
        {
            Switch(player, _playerSkins[pi]);
        }
        
        
        private static void Switch(GameObject receiver, string materialPath)
        {
            if (materialPath == "") return;
            
            // Load the material from the specified path
            Material material = Resources.Load<Material>(materialPath);

            if (material != null)
            {
                // Get the GameObject's renderer component
                Renderer renderer = receiver.GetComponentInChildren<SkinnedMeshRenderer>();
                if (renderer == null)
                {
                    Debug.LogError("The game object does not have a rendered to switch materials");
                    return;
                }
                renderer.material = material;
            }
            else
            {
                Debug.LogError("Material not found at the specified path under resources: " + materialPath);
            }
        }
    }
}