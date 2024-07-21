using Godot;
using System;

public partial class MobileControl : Button
{
	[Export]
    public string ActionName { get; set; } = "";

    public override void _Ready()
    {
		this.ButtonDown += Move;
		this.ButtonUp += Stop;
    }

    public void Move()
    {
        Input.ActionPress(ActionName);
    }
    public void Stop()
    {
        Input.ActionRelease(ActionName);
    }
}
