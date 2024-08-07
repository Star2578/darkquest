using Godot;
using System;
using DarkQuest.scripts.Global;

namespace DarkQuest.scripts.Controllers
{
	public partial class GuiController : CanvasLayer
	{
		public Control MobileDpad;
		public Control MobileInteract;

		public DialogueController DialogueGroup;

		public override void _Ready()
		{
			MobileDpad = GetNode<Control>("Control/MobileDpad");
			MobileInteract = GetNode<Control>("Control/MobileInteract");
			DialogueGroup = GetNode<DialogueController>("Control/DialogueGroup");
		}

		public override void _Process(double delta)
		{
			if (Input.IsActionJustPressed(Config.DialogueInput) && DialogueGroup.Visible)
			{
				GameController.Instance.guiController.DialogueGroup.NextDialogue();
			}
		}

		public void ToggleMobileVisibility()
		{
			MobileDpad.Visible = !MobileDpad.Visible;
			MobileInteract.Visible = !MobileInteract.Visible;
		}

		public void ToggleDialogueVisibility()
		{
			DialogueGroup.Visible = !DialogueGroup.Visible;
		}
	}
}

