using DarkQuest.scripts.Global;
using Godot;
using System;

namespace DarkQuest.scripts.Controllers
{
	public partial class PlayerController : CharacterBody2D
	{
		private int _gridSize = Config.GridSize;
		private Vector2 _targetPosition;

		private RayCast2D _rayCastUp;
		private RayCast2D _rayCastDown;
		private RayCast2D _rayCastLeft;
		private RayCast2D _rayCastRight;

		private const float MoveThreshold = 0.1f; // Threshold to determine if the player has reached the target position
		private const float LerpSpeed = 0.2f;

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
			_rayCastUp.TargetPosition = new Vector2(0, -_gridSize);
			_rayCastDown.TargetPosition = new Vector2(0, _gridSize);
			_rayCastLeft.TargetPosition = new Vector2(-_gridSize, 0);
			_rayCastRight.TargetPosition = new Vector2(_gridSize, 0);
		}

		private void Initialize()
		{
			GameController.Instance.Player = this;
			GD.Print("SetPlayer");
		}

		public override void _Process(double delta)
		{
			#region --- PLAYER MOVEMENT ---
			if (Position.DistanceTo(_targetPosition) < MoveThreshold && !IsInAction())
			{
				var colliding = IsAnyRaycastCollidingWithLayer(Config.BlockableLayerName);

				if (Input.IsActionPressed("move_up") && colliding.raycast != _rayCastUp)
				{
					_targetPosition.Y -= _gridSize;
				}
				else if (Input.IsActionPressed("move_down") && colliding.raycast != _rayCastDown)
				{
					_targetPosition.Y += _gridSize;
				}
				else if (Input.IsActionPressed("move_left") && colliding.raycast != _rayCastLeft)
				{
					_targetPosition.X -= _gridSize;
				}
				else if (Input.IsActionPressed("move_right") && colliding.raycast != _rayCastRight)
				{
					_targetPosition.X += _gridSize;
				}
			}
			#endregion

			if (Input.IsActionJustPressed("interact") && !GameController.Instance.IsInteracting)
			{
				var colliding = IsAnyRaycastCollidingWithLayer(Config.InteractableLayerName);
				if (colliding.result)
				{
					GD.Print("Interact!");
					var collider = colliding.raycast.GetCollider();

					if (collider is BaseNPC)
					{
						GD.Print("Is npc");
						collider.Call("Interact");
					}

					GameController.Instance.IsInteracting = true;
				}
				else
				{
					GD.Print("Nothing");
				}
			}

			// Smoothly move the player towards the target position
			Position = Position.Lerp(_targetPosition, LerpSpeed);
		}

		private (bool result, RayCast2D raycast) IsAnyRaycastCollidingWithLayer(string layerName)
		{
			if (_rayCastUp.IsColliding())
			{
				return (result: Util.GetRaycast2DCollideResult(_rayCastUp, layerName), raycast: _rayCastUp);
			}
			if (_rayCastDown.IsColliding())
			{
				return (result: Util.GetRaycast2DCollideResult(_rayCastDown, layerName), raycast: _rayCastDown);
			}
			if (_rayCastLeft.IsColliding())
			{
				return (result: Util.GetRaycast2DCollideResult(_rayCastLeft, layerName), raycast: _rayCastLeft);
			}
			if (_rayCastRight.IsColliding())
			{
				return (result: Util.GetRaycast2DCollideResult(_rayCastRight, layerName), raycast: _rayCastRight);
			}

			return (false, null);
		}

		public void ResetTargetPos()
		{
			_targetPosition = Position;
		}

		public bool IsInAction()
		{
			return GameController.Instance.IsInteracting;
		}
	}
}
