using Godot;
using System;

public partial class GameController : Node
{
	public static GameController Instance { get; private set; }

	[Export]
	public string PlayerScenePath { get; set; } = "res://scenes/Player.tscn";

	public Node2D Player { get; set; }

	public Vector2 CameraBoundX { get; set; } = new Vector2(-200, 200); // Left, Right
	public Vector2 CameraBoundY { get; set; } = new Vector2(-140, 140); // Top, Bottom

	public GuiController guiController { get; set; }

	public bool IsInteracting = false;

	public override void _Ready()
	{
		if (Instance == null)
		{
			Instance = this;

			// Load the player scene and add it as a child of GameController
			PackedScene playerScene = GD.Load<PackedScene>(PlayerScenePath);
			Player = playerScene.Instantiate<Node2D>();
            AddChild(Player);

			guiController = GetNode<GuiController>("UI");

			GD.Print("Instantiated a GameController");
		}
		else
		{
			QueueFree();
		}
	}

	public void TeleportPlayer(Vector2 position)
	{
		if (Player != null)
		{
			Player.Position = position;
            var playerController = Player as PlayerController;
            playerController.ResetTargetPos();
            GD.Print("tp -> " + Player.Position);
		}
	}
}
