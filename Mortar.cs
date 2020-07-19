using Godot;
using System;

public class Mortar : StaticBody2D
{
    private Pestle _pestle;
    private PotionCircle _potionCircle;
    private Alchemy _alchemy;
    private TextureProgress _potionProgress;

    private bool _canHit = true;
    private Particles2D _mortarSplash;
    private Color _averageColour;
    private Color _currentParticleColour = new Color();

    private Gradient _crushGradient = new Gradient();

    public Color AverageColour { get { return _averageColour; } set { _averageColour = value; } }
    public Color CurrentParticleColour { get { return _currentParticleColour; } }
    public Gradient CrushGradient { get { return _crushGradient; } set { _crushGradient = value; } }

    public override void _Ready()
    {
        _alchemy = GetParent().GetParent<Alchemy>();
        _mortarSplash = GetParent().GetNode<Particles2D>("Crush/MortarSplash");
        _pestle = GetParent().GetNode<Pestle>("Pestle");
        _potionCircle = GetParent().GetNode<PotionCircle>("Crush/PotionCircle");
        _potionProgress = GetParent().GetNode<TextureProgress>("Crush/PotionProgress");

        _pestle.Connect("body_entered", this, nameof(_OnPestleBodyEntered));
    }

    public void _OnPestleBodyEntered(Node body)
    {
        if (_canHit && body.Name == "Mortar" && Mathf.IsEqualApprox(_pestle.LocalCollisionPos.y, Position.y + 15f, 10f))
        {
            _canHit = false;
            GetTree().CreateTimer(1f).Connect("timeout", this, nameof(HitCoolDown));

            if (_pestle.CollisionVelocityLen < 10f)
                return;

            _mortarSplash.Emitting = true;
            (_mortarSplash.ProcessMaterial as ParticlesMaterial).Color = LerpReagentColours((float)(_potionProgress.Value / (_potionProgress.MaxValue - 20f)));

            GetTree().CreateTimer(.1f).Connect("timeout", this, nameof(DisableMortarSplash));

            _alchemy.PestleHitMortar(_pestle.CollisionVelocityLen);
        }
    }

    private void HitCoolDown()
    {
        _canHit = true;
    }

    private void DisableMortarSplash()
    {
        _mortarSplash.Emitting = false;
    }

    private Color LerpReagentColours(float t)
    {
        _currentParticleColour = _crushGradient.Interpolate(t);

        return _currentParticleColour;
    }
}
