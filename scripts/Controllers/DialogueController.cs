using DarkQuest.scripts.Global;
using DarkQuest.scripts.Models;
using Godot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DarkQuest.scripts.Controllers
{
	public partial class DialogueController : Control
	{
		[Signal]
		public delegate void DialogueEndEventHandler();

		public Label NameDisplay;
		public Label TextDisplay;
		public VBoxContainer DialogueChoicesContainer;

		public PackedScene ChoiceButtonScene;
		public List<Button> ChoiceButtons;

		public string DialogueFilePath;
		private DialogueList _dialogueList;
		private int _dialogueIndex;

		private bool _IsExhausted;
		private int _dialogueExhaustedIndex;

		private bool _IsChoicePicked;

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

		public void LoadDialogue(string filePath, bool exhausted)
		{
			try
			{
				var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
				var jsonText = file.GetAsText();
				file.Close();

				_dialogueList = JsonConvert.DeserializeObject<DialogueList>(jsonText);
				_dialogueIndex = 0;
				_dialogueExhaustedIndex = 0;

				_IsExhausted = exhausted;

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

		#region --- DIALOGUE LOGIC ---
		public void UpdateDialogueText(DialogueList dialogueList)
		{
			if (_IsExhausted)
			{
				NameDisplay.Text = dialogueList.Dialogue_Exhausted[_dialogueExhaustedIndex].Name;
				TextDisplay.Text = dialogueList.Dialogue_Exhausted[_dialogueExhaustedIndex].Text;

				AnimateText();

				foreach (var choice in dialogueList.Dialogue_Exhausted[_dialogueExhaustedIndex].Choices)
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
			string code;
			GD.Print(choiceIndex);

			if (_IsExhausted)
			{
				code = _dialogueList.Dialogue_Exhausted[_dialogueExhaustedIndex].Choices_Impact[choiceIndex];
			}
			else
			{
				code = _dialogueList.Dialogue[_dialogueIndex].Choices_Impact[choiceIndex];
			}

			// TODO : Choice impact
			_IsChoicePicked = true;

			ChoiceImpact.Impact(code);

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

			if (ChoiceButtons.Any() && !_IsChoicePicked)
			{
				GD.Print("Choose an option to proceed");
				return;
			}
			else
			{
				_IsChoicePicked = false;
			}

			if (!_IsExhausted)
				_dialogueIndex++;
			else
				_dialogueExhaustedIndex++;

			if (
				_dialogueIndex >= _dialogueList.Dialogue.Length ||
				_dialogueExhaustedIndex >= _dialogueList.Dialogue_Exhausted.Length)
			{
				GameController.Instance.guiController.ToggleDialogueVisibility();
				GameController.Instance.IsInteracting = false;
				EmitSignal(SignalName.DialogueEnd);
				return;
			}

			ClearDialogue();
			UpdateDialogueText(_dialogueList);
		}
		#endregion
	}
}
