using DarkQuest.scripts.Model;
using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class DialogueController : Control
{
	public Label NameDisplay;
	public Label TextDisplay;
	public VBoxContainer DialogueChoicesContainer;

	public PackedScene ChoiceButtonScene;
	public List<Button> ChoiceButtons;

	public string DialogueFilePath;
	private DialogueList _dialogueList;
	private int _dialogueIndex;


	public override void _Ready()
	{
		NameDisplay = GetNode<Label>("NameDisplay");
		TextDisplay = GetNode<Label>("TextDisplay");
		DialogueChoicesContainer = GetNode<VBoxContainer>("DialogueChoices");

		ChoiceButtonScene = GD.Load<PackedScene>("res://scenes/ChoiceButton.tscn");
		ChoiceButtons = new List<Button>();
	}

	public void LoadDialogue(string filePath)
	{
		try
		{
			var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
			var jsonText = file.GetAsText();
			file.Close();

			_dialogueList = JsonConvert.DeserializeObject<DialogueList>(jsonText);
			_dialogueIndex = 0;

			UpdateDialogueText(_dialogueList);
		}
		catch (System.Exception ex)
		{
			NameDisplay.Text = "error";
			TextDisplay.Text = ex.Message;
		}
	}

	public void ClearDialogue()
	{
		TextDisplay.Text = "";
		foreach (var choice in ChoiceButtons)
		{
			DialogueChoicesContainer.RemoveChild(choice);
		}
		ChoiceButtons.Clear();
	}

	public void UpdateDialogueText(DialogueList dialogueList)
	{
		NameDisplay.Text = dialogueList.dialogue[_dialogueIndex].name;
		TextDisplay.Text = dialogueList.dialogue[_dialogueIndex].text;

		foreach (var choice in dialogueList.dialogue[_dialogueIndex].choices)
		{
			AddChoice(choice);
		}
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

		// TODO : Choice impact

		NextDialogue();
	}

	public void NextDialogue()
	{
		_dialogueIndex++;

		if (_dialogueIndex >= _dialogueList.dialogue.Length)
		{
			GameController.Instance.guiController.ToggleDialogueVisibility();
			GD.Print("interacting status: ", GameController.Instance.IsInteracting);
			return;
		}

		ClearDialogue();
		UpdateDialogueText(_dialogueList);
	}
}
