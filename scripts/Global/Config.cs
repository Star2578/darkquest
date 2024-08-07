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

        // Input map
        public const string MoveUpInput = "move_up";
        public const string MoveDownInput = "move_down";
        public const string MoveLeftInput = "move_left";
        public const string MoveRightInput = "move_right";
        public const string InteractInput = "interact";
        public const string DialogueInput = "dialogue_interact";
    }
}