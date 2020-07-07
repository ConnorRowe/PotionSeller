using Godot;
using System;

public class Player : KinematicBody2D
{
    public enum Direction
    {
        Right,
        Down,
        Left,
        Up
    }

    private JoystickButton _joystick = null;
    private AnimatedSprite _playerSprite = null;
    private Direction _playerDirection = Direction.Right;

    private const float _MaxSpeed = 100f;
    private const float _Acceleration = 1000f;
    private Vector2 _motion = Vector2.Zero;

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

    public override void _Ready()
    {
        _joystick = GetParent().GetNodeOrNull("CanvasLayer").GetChild(0).GetChild<JoystickButton>(0);
        _playerSprite = GetNode<AnimatedSprite>("AnimatedSprite");
    }

    public override void _PhysicsProcess(float delta)
    {
        Vector2 axis = _joystick.GetValue().Normalized();

        if (axis == Vector2.Zero)
        {
            // Not moving
            ApplyFriction(_Acceleration * delta);
        }
        else
        {
            //Moving
            ApplyMovement(axis * _Acceleration * delta);
        }

        _motion = MoveAndSlide(_motion);

        if (_motion.Length() > 0f)
        {
            bool vertical = Mathf.Abs(_motion.y) > Mathf.Abs(_motion.x);
            bool positive = Mathf.Sign(vertical ? _motion.y : _motion.x) > 0f;

            PlayerDirection = vertical ? (positive ? Direction.Down : Direction.Up) : (positive ? Direction.Right : Direction.Left);
            _playerSprite.SpeedScale = 1.0f;
        }
        else
        {
            _playerSprite.SpeedScale = 0.0f;
            _playerSprite.Frame = 0;

        }
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

    private void ApplyFriction(float amount)
    {
        if (_motion.Length() > amount)
        {
            _motion -= _motion.Normalized() * amount;
        }
        else
        {
            _motion = Vector2.Zero;
        }
    }

    private void ApplyMovement(Vector2 amount)
    {
        _motion += amount;
        if (_motion.Length() > _MaxSpeed)
        {
            _motion = _motion.Clamped(_MaxSpeed);
        }
    }
}
