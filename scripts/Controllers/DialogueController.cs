using DarkQuest.scripts.Models;
using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DarkQuest.scripts.Controllers
{
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

		private Tween _tween;
		private float _textAnimation = 1.0f;
		public bool IsAnimationRunning
		{
			get { return _tween.IsRunning(); }
		}

		public override void _Ready()
		{
			NameDisplay = GetNode<Label>("NameDisplay");
			TextDisplay = GetNode<Label>("TextDisplay");
			DialogueChoicesContainer = GetNode<VBoxContainer>("DialogueChoices");

			ChoiceButtonScene = GD.Load<PackedScene>("res://scenes/ChoiceButton.tscn");
			ChoiceButtons = new List<Button>();
		}

		public void LoadDialogue(string filePath, bool exhausted = false)
		{
			try
			{
				var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
				var jsonText = file.GetAsText();
				file.Close();

				_dialogueList = JsonConvert.DeserializeObject<DialogueList>(jsonText);
				_dialogueIndex = 0;

				UpdateDialogueText(_dialogueList, exhausted);
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

		#region --- DIALOGUE LOGIC ---
		public void UpdateDialogueText(DialogueList dialogueList, bool exhausted = false)
		{
			if (exhausted)
			{
				NameDisplay.Text = dialogueList.DialogueExhausted[_dialogueIndex].Name;
				TextDisplay.Text = dialogueList.DialogueExhausted[_dialogueIndex].Text;

				AnimateText();

				foreach (var choice in dialogueList.DialogueExhausted[_dialogueIndex].Choices)
				{
					AddChoice(choice);
				}

				return;
			}

			NameDisplay.Text = dialogueList.Dialogue[_dialogueIndex].Name;
			TextDisplay.Text = dialogueList.Dialogue[_dialogueIndex].Text;

			AnimateText();

			foreach (var choice in dialogueList.Dialogue[_dialogueIndex].Choices)
			{
				AddChoice(choice);
			}
		}

		private void AnimateText()
		{
			if (_tween != null)
				_tween.Kill(); // Abort the previous animation

			_tween = CreateTween();
			_tween.TweenProperty(TextDisplay, "visible_ratio", 1.0f, _textAnimation);

			TextDisplay.VisibleRatio = 0; // Reset VisibleRatio
			_tween.Play();
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
			if (IsAnimationRunning)
			{
				if (_tween != null)
					_tween.Kill(); // Abort the previous animation

				TextDisplay.VisibleRatio = 1;

				return;
			}

			_dialogueIndex++;

			if (_dialogueIndex >= _dialogueList.Dialogue.Length)
			{
				GameController.Instance.guiController.ToggleDialogueVisibility();
				GameController.Instance.IsInteracting = false;
				return;
			}

			ClearDialogue();
			UpdateDialogueText(_dialogueList);
		}
		#endregion
	}
}
