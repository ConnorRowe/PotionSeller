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
    private Color[] _reagentColours = new Color[4];
    private Color _averageColour;
    private Color _currentParticleColour = new Color();

    public Color[] ReagentColours { get { return _reagentColours; } }
    public Color AverageColour { get { return _averageColour; } set { _averageColour = value; } }
    public Color CurrentParticleColour { get { return _currentParticleColour; } }

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
        t *= _reagentColours.Length;
        t = Mathf.Clamp(t, 0f, (float)_reagentColours.Length);

        Color newColour = _reagentColours[0];
        for (int i = 1; i < _reagentColours.Length; i++)
        {
            newColour = newColour.LinearInterpolate(_reagentColours[i], t - i);
            _currentParticleColour = newColour;
        }

        return newColour;
    }
}
