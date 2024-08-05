using DarkQuest.scripts.Controllers;
using Godot;
using System;

public partial class BaseNPC : CharacterBody2D
{
    private Sprite2D _sprite;
    private AnimationPlayer _animationPlayer;

    [Signal]
    public delegate void DialogueOpenEventHandler();

    [Export(PropertyHint.File, "*.json")]
	public string DialogueFilePath;

    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("Sprite2D");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        DialogueOpen += ShowDialogue;
    }

    private void ShowDialogue()
    {
        GD.Print("Dialogue Opened");
        GameController.Instance.guiController.DialogueGroup.LoadDialogue(DialogueFilePath);
        GameController.Instance.guiController.ToggleDialogueVisibility();
    }

    public void Interact()
    {
        EmitSignal(SignalName.DialogueOpen);
    }
}
