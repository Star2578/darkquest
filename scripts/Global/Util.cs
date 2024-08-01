using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace DarkQuest.scripts.Global
{
    public static class Util
    {
        public static bool GetRaycast2DCollideResult(RayCast2D raycast, string layerName)
        {
            var collider = raycast.GetCollider();

            if (collider is CollisionObject2D collisionObject)
            {
                int layerIndex = GetLayerIndex(layerName);

                // GD.Print("layer index ", layerIndex);

                return (collisionObject.CollisionLayer & layerIndex) != 0;
            }
            else if (collider is TileMap tileMap)
            {
                return true;
            }

            return false;
        }


        public static int GetLayerIndex(string layerName)
        {
            for (int i = 0; i < 32; i++)
            {
                if ((string)ProjectSettings.GetSetting($"layer_names/2d_physics/layer_{i}") == layerName)
                    return i;
            }
            GD.Print("Not found layer");
            return -1; // Return -1 if layer not found
        }
    }
}