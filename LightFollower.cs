using Godot;
using System;

public class LightFollower : Sprite
{
    [Export]
    private NodePath _nodePath;
    private Node2D _followNode;

    public override void _Ready()
    {
        if(GetNodeOrNull(_nodePath) is Node2D gotNode2D)
            _followNode = gotNode2D;
        else
            return;
        
        Vector2 posDiff = (_followNode.Position - Position).Normalized();
        Position = _followNode.Position + (posDiff * 30f);
    }

    public override void _Process(float delta)
    {
        Vector2 posDiff = (_followNode.Position - Position);
        if(posDiff.Length() > 30f)
            Position += posDiff.Normalized() * 75f * delta;
    }
}
