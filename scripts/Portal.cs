using Godot;
using System;

public partial class Portal : Area2D
{
	[Export]
    public string TargetScenePath { get; set; } = "res://scenes/Testing/Game2.tscn";

	[Export]
    public Vector2 TargetPosition { get; set; } = Vector2.Zero;

	[Export]
    public float TransitionDelay { get; set; } = 1.0f;

	private Timer _transitionTimer;

    public override void _Ready()
    {
		_transitionTimer = GetNode<Timer>("Timer");
        _transitionTimer.WaitTime = TransitionDelay;
        _transitionTimer.OneShot = true;
        _transitionTimer.Connect("timeout", Callable.From(() => OnTransitionTimeout()));
    }

    private void OnBodyEntered(Node body)
    {
		GD.Print("Collided with " + body.Name);
        if (body is PlayerController)
        {
            // Start the timer to delay the scene change
            _transitionTimer.Start();
        }
    }

	private void OnTransitionTimeout()
    {
        // Change the scene and teleport the player after the delay
        GetTree().ChangeSceneToFile(TargetScenePath);
        GameController.Instance.CallDeferred(nameof(GameController.TeleportPlayer), TargetPosition);
    }
}
