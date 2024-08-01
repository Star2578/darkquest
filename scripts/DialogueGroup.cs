using Godot;
using System;
using System.Collections.Generic;

public partial class DialogueGroup : Control
{
	public Label NameDisplay;
	public Label TextDisplay;
	public VBoxContainer DialogueChoicesContainer;

	public PackedScene ChoiceButtonScene;
	public List<Button> ChoiceButtons;

    public override void _Ready()
    {
		NameDisplay = GetNode<Label>("NameDisplay");
		TextDisplay = GetNode<Label>("TextDisplay");
		DialogueChoicesContainer = GetNode<VBoxContainer>("DialogueChoices");

		ChoiceButtonScene = GD.Load<PackedScene>("res://scenes/ChoiceButton.tscn");
		ChoiceButtons = new List<Button>();

		AddChoice("Testing Choice 1");
		AddChoice("Testing Choice 2");
    }

	public void ClearDialogue()
	{
		TextDisplay.Text = "";
		foreach (var choice in ChoiceButtons)
		{
			RemoveChild(choice);
		}
		ChoiceButtons.Clear();
	}

    public void UpdateText(string text)
	{
		TextDisplay.Text = text;
	}

	public void AddChoice(string text)
	{
		ChoiceButton buttonObject = (ChoiceButton)ChoiceButtonScene.Instantiate();

		buttonObject.ChoiceIndex = ChoiceButtons.Count;
		ChoiceButtons.Add(buttonObject);
		buttonObject.Text = text;
		buttonObject.ChoiceSelected += (choiceIndex) => OnChoiceSelected(choiceIndex);

		DialogueChoicesContainer.AddChild(buttonObject);
	}

	private void OnChoiceSelected(int choiceIndex)
	{
		GD.Print(choiceIndex);
	}
}
