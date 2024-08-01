using Godot;
using System;

public partial class BaseNPC : CharacterBody2D
{
    private Sprite2D _sprite;
    private AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("Sprite2D");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }
}
