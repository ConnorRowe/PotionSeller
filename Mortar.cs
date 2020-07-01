using Godot;
using System;

public class Mortar : StaticBody2D
{
    private Pestle _pestle;
    private PotionCircle _potionCircle;
    private Alchemy _alchemy;
    private bool _canHit = true;

    public Alchemy @Alchemy { get { return _alchemy; } set { _alchemy = value; } }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _pestle = GetParent().GetNode<Pestle>("Pestle");
        _pestle.Connect("body_entered", this, nameof(_OnPestleBodyEntered));

        _potionCircle = GetParent().GetNode<PotionCircle>("PotionCircle");
    }

    public void _OnPestleBodyEntered(Node body)
    {
        if (_canHit && body.Name == "Mortar" && Mathf.IsEqualApprox(_pestle.LocalCollisionPos.y, Position.y + 15f, 10f))
        {
            _canHit = false;
            GetTree().CreateTimer(1f).Connect("timeout", this, nameof(HitCoolDown));

            if (_pestle.CollisionVelocityLen < 10f)
                return;

            _alchemy.PestleHitMortar(_pestle.CollisionVelocityLen);
        }
    }

    private void HitCoolDown()
    {
        _canHit = true;
    }
}
