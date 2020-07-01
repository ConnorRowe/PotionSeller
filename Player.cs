using Godot;
using System;

public class Player : Area2D
{
    public enum Direction
    {
        Right,
        Down,
        Left,
        Up
    }

    private JoystickButton _joystick = null;
    private const float _moveSpeed = 50f;
    private AnimatedSprite _playerSprite = null;
    private Direction _playerDirection = Direction.Right;

    public Direction PlayerDirection
    {
        get { return _playerDirection; }
        set
        {
            if (_playerDirection != value)
            {
                _playerDirection = value;
                DirectionUpdated();
            }
        }
    }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _joystick = GetParent().GetNodeOrNull("CanvasLayer").GetChild(0).GetChild<JoystickButton>(0);
        _playerSprite = GetNode<AnimatedSprite>("AnimatedSprite");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Vector2 axis = _joystick.GetValue().Normalized();

        if (axis.Length() > 0f)
        {
            bool vertical = Mathf.Abs(axis.y) > Mathf.Abs(axis.x);
            bool positive = Mathf.Sign(vertical ? axis.y : axis.x) > 0f;

            PlayerDirection = vertical ? (positive ? Direction.Down : Direction.Up) : (positive ? Direction.Right : Direction.Left);

            _playerSprite.SpeedScale = 1.0f;
        }
        else
        {
            _playerSprite.SpeedScale = 0.0f;
            _playerSprite.Frame = 0;
        }

        Position += axis * _moveSpeed * delta;
    }

    private void DirectionUpdated()
    {
        switch (_playerDirection)
        {
            case Direction.Down:
                _playerSprite.Animation = "run_towards";
                _playerSprite.FlipH = false;
                break;
            case Direction.Left:
                _playerSprite.Animation = "run_right_left";
                _playerSprite.FlipH = true;
                break;
            case Direction.Right:
                _playerSprite.Animation = "run_right_left";
                _playerSprite.FlipH = false;
                break;
            case Direction.Up:
                _playerSprite.Animation = "run_away";
                _playerSprite.FlipH = false;
                break;
        }
    }
}
