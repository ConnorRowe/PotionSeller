using Godot;
using System;

public class CameraScript : Camera2D
{
	private JoystickButton _joystick = null;
	private const float _moveSpeed = 100f;
	
	public override void _Ready()
	{
		_joystick = GetParent().GetNodeOrNull("CanvasLayer").GetChild(0).GetChild<JoystickButton>(0);
	}
	
	public override void _Process(float delta)
	{
		Vector2 axis = _joystick.GetValue();
		Position += axis * delta * _moveSpeed;
	}
}
