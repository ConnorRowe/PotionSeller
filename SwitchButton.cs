using Godot;
using System;

public class SwitchButton : TouchScreenButton
{

    public override void _Ready()
    {
        Connect("released", this, nameof(ButtonReleased));
    }

    private void ButtonReleased()
    {
        string nextScene = Owner.Name == "AlchemyRoot" ? "res://World.tscn" : "res://Alchemy.tscn";
        GetTree().ChangeScene(nextScene);
    }

}
