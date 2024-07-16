using Godot;
using System;

public partial class CameraController : Camera2D
{
	private GameController _gameController;
    private Node2D _player;

    public override void _Ready()
    {
        CallDeferred(nameof(Initialize));
        GD.Print("Init cam");
    }

	private void Initialize()
    {
		_gameController = GameController.Instance;
        _player = GameController.Instance.Player;
    }

    public override void _Process(double delta)
    {
        LimitTop = (int)_gameController.CameraBoundY.X;
        LimitBottom = (int)_gameController.CameraBoundY.Y;
        LimitLeft = (int)_gameController.CameraBoundX.X;
        LimitRight = (int)_gameController.CameraBoundX.Y;
    }
}
