using DarkQuest.scripts.Global;
using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
	private Vector2 _gridSize = new Vector2(Config.GridSize, Config.GridSize);
	private Vector2 _targetPosition;
    private const float MoveThreshold = 0.1f; // Threshold to determine if the player has reached the target position
    private const float LerpSpeed = 0.2f; // Speed of the lerp movement
	
	public override void _Ready()
	{
		_targetPosition = Position;
	}

	public override void _Process(double delta)
	{
        if (Position.DistanceTo(_targetPosition) < MoveThreshold)
        {
            // Check for input and set target position
            if (Input.IsActionPressed("move_up"))
                _targetPosition.Y -= _gridSize.Y;
            else if (Input.IsActionPressed("move_down"))
                _targetPosition.Y += _gridSize.Y;
            else if (Input.IsActionPressed("move_left"))
                _targetPosition.X -= _gridSize.X;
            else if (Input.IsActionPressed("move_right"))
                _targetPosition.X += _gridSize.X;
        }

        // Smoothly move the player towards the target position
        Position = Position.Lerp(_targetPosition, LerpSpeed);
	}
}
