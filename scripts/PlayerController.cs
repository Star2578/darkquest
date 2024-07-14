using DarkQuest.scripts.Global;
using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
	private Vector2 _gridSize = new Vector2(Config.GridSize, Config.GridSize);
	private Vector2 _targetPosition;

    private RayCast2D _rayCastUp;
    private RayCast2D _rayCastDown;
    private RayCast2D _rayCastLeft;
    private RayCast2D _rayCastRight;

    private const float MoveThreshold = 0.1f; // Threshold to determine if the player has reached the target position
    private const float LerpSpeed = 0.2f; // Speed of the lerp movement
	
	public override void _Ready()
	{
		_targetPosition = Position;
        _rayCastUp = GetNode<RayCast2D>("RayCastUp");
        _rayCastDown = GetNode<RayCast2D>("RayCastDown");
        _rayCastLeft = GetNode<RayCast2D>("RayCastLeft");
        _rayCastRight = GetNode<RayCast2D>("RayCastRight");

        // Set the raycast lengths to match the grid size
        _rayCastUp.TargetPosition = new Vector2(0, -_gridSize.Y);
        _rayCastDown.TargetPosition = new Vector2(0, _gridSize.Y);
        _rayCastLeft.TargetPosition = new Vector2(-_gridSize.X, 0);
        _rayCastRight.TargetPosition = new Vector2(_gridSize.X, 0);
	}

	public override void _Process(double delta)
	{
        if (Position.DistanceTo(_targetPosition) < MoveThreshold)
        {
            // Check for input and set target position
            if (Input.IsActionPressed("move_up") && !_rayCastUp.IsColliding())
            {
                _targetPosition.Y -= _gridSize.Y;
            }
            else if (Input.IsActionPressed("move_down") && !_rayCastDown.IsColliding())
            {
                _targetPosition.Y += _gridSize.Y;
            }
            else if (Input.IsActionPressed("move_left") && !_rayCastLeft.IsColliding())
            {
                _targetPosition.X -= _gridSize.X;
            }
            else if (Input.IsActionPressed("move_right") && !_rayCastRight.IsColliding())
            {
                _targetPosition.X += _gridSize.X;
            }
        }

        // Smoothly move the player towards the target position
        Position = Position.Lerp(_targetPosition, LerpSpeed);
	}
}
