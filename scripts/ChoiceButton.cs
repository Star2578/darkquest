using Godot;
using System;

public partial class ChoiceButton : Button
{
	public int ChoiceIndex { get; set; }

	[Signal]
	public delegate void ChoiceSelectedEventHandler(int choiceIndex);

	public override void _Ready()
	{
		this.Pressed += () =>
		{
			EmitSignal(SignalName.ChoiceSelected, ChoiceIndex);
		};
	}
}
