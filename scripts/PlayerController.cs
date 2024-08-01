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

	private bool isMoving;
	private CollisionShape2D _collider;

	public override void _Ready()
	{
		CallDeferred(nameof(Initialize));

		_targetPosition = Position;
		_collider = GetNode<CollisionShape2D>("CollisionShape2D");
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

	private void Initialize()
	{
		GameController.Instance.Player = this;
		GD.Print("SetPlayer");
	}

	public override void _Process(double delta)
	{
		if (Position.DistanceTo(_targetPosition) < MoveThreshold)
		{
			isMoving = false;
			// Check for input and set target position
			if (Input.IsActionPressed("move_up") && !IsRaycastCollidingWithLayer(_rayCastUp, Config.BlockableLayerName))
			{
				_targetPosition.Y -= _gridSize.Y;
				isMoving = true;
			}
			else if (Input.IsActionPressed("move_down") && !IsRaycastCollidingWithLayer(_rayCastDown, Config.BlockableLayerName))
			{
				_targetPosition.Y += _gridSize.Y;
				isMoving = true;
			}
			else if (Input.IsActionPressed("move_left") && !IsRaycastCollidingWithLayer(_rayCastLeft, Config.BlockableLayerName))
			{
				_targetPosition.X -= _gridSize.X;
				isMoving = true;
			}
			else if (Input.IsActionPressed("move_right") && !IsRaycastCollidingWithLayer(_rayCastRight, Config.BlockableLayerName))
			{
				_targetPosition.X += _gridSize.X;
				isMoving = true;
			}

			// GD.Print("is moving ", isMoving);
		}

		if (Input.IsKeyPressed(Key.Enter))
		{
			if (IsAnyRaycastCollidingWithLayer(Config.InteractableLayerName))
			{
				GD.Print("Interact!");
			}
			else
			{
				GD.Print("Nothing");
			}
		}

		// Smoothly move the player towards the target position
		Position = Position.Lerp(_targetPosition, LerpSpeed);
	}

	private bool IsRaycastCollidingWithLayer(RayCast2D raycast, string layerName)
	{
		if (raycast.IsColliding())
		{
			return Util.GetRaycast2DCollideResult(raycast, layerName);
		}
		return false;
	}

	private bool IsAnyRaycastCollidingWithLayer(string layerName)
	{
		if (_rayCastUp.IsColliding())
		{
			return Util.GetRaycast2DCollideResult(_rayCastUp, layerName);
		}
		if (_rayCastDown.IsColliding())
		{
			return Util.GetRaycast2DCollideResult(_rayCastDown, layerName);
		}
		if (_rayCastLeft.IsColliding())
		{
			return Util.GetRaycast2DCollideResult(_rayCastLeft, layerName);
		}
		if (_rayCastRight.IsColliding())
		{
			return Util.GetRaycast2DCollideResult(_rayCastRight, layerName);
		}

		return false;
	}

	public void ResetTargetPos()
	{
		_targetPosition = Position;
	}
}
