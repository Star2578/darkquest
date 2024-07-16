using Godot;
using System;

public partial class GameController : Node
{
	public static GameController Instance { get; private set; }

    public Node2D Player { get; set; }

	public Vector2 CameraBoundX { get; set; } = new Vector2(-200, 200); // Left, Right
    public Vector2 CameraBoundY { get; set; } = new Vector2(-140, 140); // Top, Bottom

    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;

			GD.Print("Instantiated a GameController");
        }
        else
        {
            QueueFree();
        }
    }
}
