using Godot;
using System;

public partial class GuiController : CanvasLayer
{
	public Control MobileDpad;
	public Control MobileInteract;
	
	public NinePatchRect DialogueBox;

	public override void _Ready()
	{
		MobileDpad = GetNode<Control>("Control/MobileDpad");
		MobileInteract = GetNode<Control>("Control/MobileInteract");
		DialogueBox = GetNode<NinePatchRect>("Control/DialogueGroup/DialogueBox");
	}

	public override void _Process(double delta)
	{
	}

	public void ToggleMobileVisibility()
	{
		MobileDpad.Visible = !MobileDpad.Visible;
		MobileInteract.Visible = !MobileInteract.Visible;
	}

	public void ToggleDialogueBoxVisibility()
	{
		DialogueBox.Visible = !DialogueBox.Visible;
	}
}
