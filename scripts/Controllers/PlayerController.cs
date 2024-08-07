using DarkQuest.scripts.Global;
using Godot;
using System;
using System.Collections.Generic;

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
		private Vector2I direction;

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
			#region --- MOVEMENT CONTROL ---
			if (Position.DistanceTo(_targetPosition) < MoveThreshold && !IsInAction())
			{
				var colliding = IsAnyRaycastCollidingWithLayer(Config.BlockableLayerName);

				if (Input.IsActionPressed(Config.MoveUpInput) && !colliding.raycast.Contains(_rayCastUp))
				{
					_targetPosition.Y -= _gridSize;
					direction = Vector2I.Up;
				}
				else if (Input.IsActionPressed(Config.MoveDownInput) && !colliding.raycast.Contains(_rayCastDown))
				{
					_targetPosition.Y += _gridSize;
					direction = Vector2I.Down;
				}
				else if (Input.IsActionPressed(Config.MoveLeftInput) && !colliding.raycast.Contains(_rayCastLeft))
				{
					_targetPosition.X -= _gridSize;
					direction = Vector2I.Left;
				}
				else if (Input.IsActionPressed(Config.MoveRightInput) && !colliding.raycast.Contains(_rayCastRight))
				{
					_targetPosition.X += _gridSize;
					direction = Vector2I.Right;
				}
			}
			#endregion

			#region --- DIALOGUE CONTROL ---
			if (Input.IsActionJustPressed(Config.InteractInput) && !GameController.Instance.IsInteracting)
			{
				var colliding = IsAnyRaycastCollidingWithLayer(Config.InteractableLayerName);
				if (colliding.result)
				{
					GD.Print("Try Interact");
					GodotObject collider;


					if (direction == Vector2I.Up)
					{
						collider = colliding.raycast.Find(x => x == _rayCastUp)?.GetCollider();
					}
					else if (direction == Vector2I.Down)
					{
						collider = colliding.raycast.Find(x => x == _rayCastDown)?.GetCollider();
					}
					else if (direction == Vector2I.Right)
					{
						collider = colliding.raycast.Find(x => x == _rayCastRight)?.GetCollider();
					}
					else if (direction == Vector2I.Left)
					{
						collider = colliding.raycast.Find(x => x == _rayCastLeft)?.GetCollider();
					}
					else
					{
						return;
					}

					GameController.Instance.IsInteracting = true;

					if (collider is BaseNPC)
					{
						GD.Print("Is npc");
						collider.Call("Interact");
					}
					else
					{
						GameController.Instance.IsInteracting = false;
					}
				}
				else
				{
					GD.Print("Nothing");
				}
			}
			#endregion

			// Smoothly move the player towards the target position
			Position = Position.Lerp(_targetPosition, LerpSpeed);
		}

		private (bool result, List<RayCast2D> raycast) IsAnyRaycastCollidingWithLayer(string layerName)
		{
			List<RayCast2D> colliding = new List<RayCast2D>();
			bool IsColliding = false;

			if (_rayCastUp.IsColliding())
			{
				IsColliding = true;
				colliding.Add(_rayCastUp);
			}
			if (_rayCastDown.IsColliding())
			{
				IsColliding = true;
				colliding.Add(_rayCastDown);
			}
			if (_rayCastLeft.IsColliding())
			{
				IsColliding = true;
				colliding.Add(_rayCastLeft);
			}
			if (_rayCastRight.IsColliding())
			{
				IsColliding = true;
				colliding.Add(_rayCastRight);
			}

			return (result: IsColliding, raycast: colliding);
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
