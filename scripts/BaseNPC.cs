using DarkQuest.scripts.Controllers;
using Godot;
using System;

public partial class BaseNPC : CharacterBody2D
{
    private Sprite2D _sprite;
    private AnimationPlayer _animationPlayer;

    [Signal]
    public delegate void DialogueOpenEventHandler();

    [Signal]
    public delegate void DialogueEndEventHandler();

    [Export(PropertyHint.File, "*.json")]
	public string DialogueFilePath;

    public bool IsDialogueExhausted = false;

    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("Sprite2D");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        DialogueOpen += OnDialogueOpen;
        DialogueEnd += OnDialogueEnd;
    }

    private void OnDialogueOpen()
    {
        GD.Print("Dialogue Opened");
        GameController.Instance.guiController.DialogueGroup.LoadDialogue(DialogueFilePath);
        GameController.Instance.guiController.ToggleDialogueVisibility();
    }

    private void OnDialogueEnd()
    {
        GD.Print("Dialogue End");
        IsDialogueExhausted = true;
    }

    public void Interact()
    {
        EmitSignal(SignalName.DialogueOpen);
    }
}
