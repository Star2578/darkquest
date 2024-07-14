using DarkQuest.scripts.Global;
using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
	private Vector2 _gridSize = new Vector2(Config.GridSize, Config.GridSize);
	private Vector2 _targetPosition;
	
	public override void _Ready()
	{
		_targetPosition = Position;
	}

	public override void _Process(double delta)
	{
		// Check for input and set target position
        if (Input.IsActionJustPressed("move_up"))
            _targetPosition.Y -= _gridSize.Y;
        else if (Input.IsActionJustPressed("move_down"))
            _targetPosition.Y += _gridSize.Y;
        else if (Input.IsActionJustPressed("move_left"))
            _targetPosition.X -= _gridSize.X;
        else if (Input.IsActionJustPressed("move_right"))
            _targetPosition.X += _gridSize.X;

        // Smoothly move the player towards the target position
        Position = Position.Lerp(_targetPosition, 0.2f);
	}
}
