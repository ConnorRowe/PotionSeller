using Godot;
using System;

public class Pestle : RigidBody2D
{
    private const float _DragScale = 10f;
    private bool _picked = false;
    private Vector2 _touchPos = Vector2.Zero;
    private Label _label;
    private int _ongoingDrag = -1;
    private Vector2 _offset = new Vector2(0f, 30f);
    private const float _VelocityLimit = 500f;
    private Vector2 _localCollisionPos = Vector2.Zero;
    private float _collisionVelocityLen = 0f;
    private const float _acceleration = 200f;

    public Vector2 LocalCollisionPos { get { return _localCollisionPos; } }
    public float CollisionVelocityLen { get { return _collisionVelocityLen; } }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _label = GetNode<Label>("Label");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventScreenDrag && ((InputEventScreenDrag)@event).Index == _ongoingDrag)
        {
            _touchPos = ((InputEventScreenDrag)@event).Position;
        }
        if (@event is InputEventScreenTouch && ((InputEventScreenTouch)@event).Index == _ongoingDrag)
        {
            if (!((InputEventScreenTouch)@event).Pressed)
            {
                _picked = false;
            }
        }
    }

    public override void _InputEvent(Godot.Object viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventScreenTouch)
        {
            _picked = ((InputEventScreenTouch)@event).Pressed;
            _touchPos = ((InputEventScreenTouch)@event).Position;
        }
        if (@event is InputEventScreenDrag && _picked)
        {
            _ongoingDrag = ((InputEventScreenDrag)@event).Index;
            _touchPos = ((InputEventScreenDrag)@event).Position;
        }

        if (!_picked && _touchPos != Vector2.Zero)
            _touchPos = Vector2.Zero;

        //GravityScale = _picked ? 0f : 1f;
        //_label.Text = "_picked = " + _picked.ToString() + "\n_touchPos = " + _touchPos.ToString();
    }

    public override void _PhysicsProcess(float delta)
    {
        if (_picked)
        {
            var axis = (_touchPos - GlobalPosition + _offset.Rotated(GlobalRotation)).Normalized();

            LinearVelocity += axis * _acceleration * delta;

            if (LinearVelocity.Length() > _VelocityLimit)
                LinearVelocity = LinearVelocity.Clamped(_VelocityLimit);
        }
    }

    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {
        if (_picked)
        {
            var axis = (_touchPos - GlobalPosition + _offset.Rotated(GlobalRotation)).Normalized();

            // state.LinearVelocity =  new Vector2(Mathf.Min(Mathf.Abs(diff.x), _VelocityLimit) * Mathf.Sign(diff.x), Mathf.Min(Mathf.Abs(diff.y), _VelocityLimit) * Mathf.Sign(diff.y));
            // state.LinearVelocity *= _DragScale;
            state.AngularVelocity = GlobalRotation * -1.5f;
        }

        if (state.GetContactCount() >= 1)
        {
            _localCollisionPos = state.GetContactLocalPosition(0);
            _collisionVelocityLen = state.LinearVelocity.Length();
        }

        base._IntegrateForces(state);
    }
}
