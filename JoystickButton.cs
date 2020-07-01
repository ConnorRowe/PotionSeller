using Godot;
using System;

public class JoystickButton : TouchScreenButton
{
    private const float _Constraint = 16f;
    private const float _Threshold = 1f;
    private const float _ReturnAccel = 20f;

    private Vector2 _radius = new Vector2(8f, 8f);
    private int _ongoingDrag = -1;

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (_ongoingDrag == -1)
        {
            Vector2 positionDiff = (_radius * -1) - Position;
            Position += positionDiff * _ReturnAccel * delta;
        }
    }

    public override void _Input(InputEvent evt)
    {
        Node2D parent = GetParent<Node2D>();

        if (evt is InputEventScreenTouch)
        {
            //Move joystick to the finger
            if (_ongoingDrag == -1)
            {
                //parent.Position = ((InputEventScreenTouch)evt).Position;
            }
        }

        if (evt is InputEventScreenDrag)
        {
            float eventDistFromCentre = (((InputEventScreenDrag)evt).Position - parent.GlobalPosition).Length();

            //Move joystick to the finger
            if (_ongoingDrag == -1)
            {
                //parent.Position = ((InputEventScreenDrag)evt).Position;
            }

            if (eventDistFromCentre <= _Constraint * GlobalScale.x || ((InputEventScreenDrag)evt).Index == _ongoingDrag)
            {
                GlobalPosition = ((InputEventScreenDrag)evt).Position - _radius * GlobalScale;

                if (GetButtonPos().Length() > _Constraint)
                {
                    Position = GetButtonPos().Normalized() * _Constraint - _radius;
                }

                _ongoingDrag = ((InputEventScreenDrag)evt).Index;
            }
        }

        if (evt is InputEventScreenTouch && !((InputEventScreenTouch)evt).Pressed && ((InputEventScreenTouch)evt).Index == _ongoingDrag)
        {
            _ongoingDrag = -1;
        }
    }

    private Vector2 GetButtonPos()
    {
        return Position + _radius;
    }

    public Vector2 GetValue()
    {
        if (GetButtonPos().Length() > _Threshold)
            return GetButtonPos() / _radius;

        return Vector2.Zero;
    }
}
