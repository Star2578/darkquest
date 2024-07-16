using Godot;
using System;

namespace DarkQuest.scripts.Global
{
    public static class Config
    {
        public const int GridSize = 16;

        // Tilemap Layers
        public const int WallLayer = 1;

        // Layers
        public const string BlockableLayerName = "Blockable";
        public const string InteractableLayerName = "Interactable";
    }
}