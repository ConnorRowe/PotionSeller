using Godot;
using System;

public class WavyGrass : Node2D, IInteractable
{
    private Sprite[] _sprites = new Sprite[3];
    private Tween _tween;
    private float rotateDegs = 6f;

    public override void _Ready()
    {
        GD.Randomize();


        _sprites[0] = GetNode<Sprite>("Offset/Sprite");
        if (GD.Randf() > .5f)
        {
            _sprites[0].Position = new Vector2(-1, 0);
            _sprites[1] = new Sprite()
            {
                Position = new Vector2(-3, -9),
                Texture = _sprites[0].Texture,
                NormalMap = _sprites[0].NormalMap,
                Material = _sprites[0].Material
            };
            _sprites[2] = new Sprite()
            {
                Position = new Vector2(5, -7),
                Texture = _sprites[0].Texture,
                NormalMap = _sprites[0].NormalMap,
                Material = _sprites[0].Material
            };

            var offset = GetNode("Offset");
            offset.AddChild(_sprites[2]);
            offset.AddChild(_sprites[1]);

            foreach (var sprite in _sprites)
            {
                sprite.ZIndex = Mathf.FloorToInt(sprite.GlobalPosition.y - 1);
            }
        }

        _tween = GetNode<Tween>("Tween");
        GetNode<Area2D>("Area").Connect("body_entered", this, nameof(BodyEntered));
    }

    private void BodyEntered(Node body)
    {
        for (int i = 0; i < _sprites.Length; i++)
        {
            float delayOffset = i * .075f;

            if (_sprites[i] != null)
            {
                _tween.InterpolateProperty(_sprites[i], "rotation_degrees", 0f, rotateDegs, .15f, Tween.TransitionType.Quart, Tween.EaseType.InOut, delayOffset);
                _tween.InterpolateProperty(_sprites[i], "rotation_degrees", rotateDegs, -rotateDegs, .3f, Tween.TransitionType.Quart, Tween.EaseType.InOut, .15f + delayOffset);
                _tween.InterpolateProperty(_sprites[i], "rotation_degrees", -rotateDegs, 0f, .15f, Tween.TransitionType.Quart, Tween.EaseType.InOut, .45f + delayOffset);
            }
        }

        _tween.Start();
    }

    public bool Interact(Player player)
    {
        BodyEntered(null);
        return true;
    }
}
