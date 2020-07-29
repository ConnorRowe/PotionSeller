using Godot;
using System;

public class ShakePlant : Node2D, IInteractable
{
    [Export]
    private NodePath _nodeToShakePath;
    [Export]
    private NodePath _nodeToHidePath;

    private Node2D _nodeToShake;
    private Node2D _nodeToHide;
    private Tween _tween;

    private PackedScene _bouncingItem;

    private bool _empty = false;
    private float rotateDegs = 6f;
    private Item.ItemStack _itemStack;


    public override void _Ready()
    {
        if (GetNodeOrNull(_nodeToShakePath) is Node2D shakeNode)
            _nodeToShake = shakeNode;
        if (GetNodeOrNull(_nodeToHidePath) is Node2D hideNode)
            _nodeToHide = hideNode;

        _bouncingItem = GD.Load<PackedScene>("res://BouncingItem.tscn");

        _tween = new Tween();
        AddChild(_tween);

        _itemStack = new Item.ItemStack(Items.HOLLY_BERRIES, 1);
    }

    public bool Interact(Player player)
    {
        if (!_empty)
        {
            _tween.InterpolateProperty(_nodeToShake, "rotation_degrees", 0f, rotateDegs, .15f, Tween.TransitionType.Quart, Tween.EaseType.InOut);
            _tween.InterpolateProperty(_nodeToShake, "rotation_degrees", rotateDegs, -rotateDegs, .3f, Tween.TransitionType.Quart, Tween.EaseType.InOut, .15f);
            _tween.InterpolateProperty(_nodeToShake, "rotation_degrees", -rotateDegs, 0f, .15f, Tween.TransitionType.Quart, Tween.EaseType.InOut, .45f);
            _tween.Start();

            BouncingItem newBounce = (BouncingItem)_bouncingItem.Instance();
            GetParent().AddChild(newBounce);

            newBounce.Initialise(new Vector2((GD.Randf() * 2) - 1f, (GD.Randf() * 2) - 1f) * 25f, (float)GD.RandRange(-40, -60));
            newBounce.Position = Position;
            newBounce.ItemStack = _itemStack;

            _empty = true;
            _nodeToHide.Visible = false;

        }

        return false;
    }

    public bool HasInteractParticle()
    {
        return false;
    }

    public Particles2D GetInteractParticles()
    {
        return null;
    }

}
