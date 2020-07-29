using Godot;
using System;

public class BouncingItem : KinematicBody2D, IInteractable
{
    private Sprite _itemSprite;

    private PackedScene _interactParticle;

    private Item.ItemStack _itemStack;
    private bool _onGround = false;
    private Vector2 _velocity;
    private float _heightVelocity = 0;
    private float _prevHeightVelocity;
    private const float _Gravity = 98f;

    public Item.ItemStack ItemStack { get { return _itemStack; } set { _itemStack = value; UpdateSpriteFromItemStack(); } }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _itemSprite = GetNode<Sprite>("Item");

        _interactParticle = GD.Load<PackedScene>("res://particle/ArcaneBurst.tscn");

        ItemStack = new Item.ItemStack(Items.ELDERBERRIES, 1);
    }

    public void Initialise(Vector2 velocity, float heightVelocity)
    {
        _velocity = velocity;
        _heightVelocity = heightVelocity;
        _prevHeightVelocity = heightVelocity;
    }

    public override void _Process(float delta)
    {
        UpdateFakeHeight(delta);
    }

    private void UpdateSpriteFromItemStack()
    {
        if (_itemStack != null)
        {
            _itemSprite.Texture = _itemStack.item.IconTex;
            _itemSprite.NormalMap = _itemStack.item.NormalMap;
        }
    }

    public virtual bool Interact(Player player)
    {
        player.PickupItem(_itemStack);

        return true;
    }

    private void UpdateFakeHeight(float delta)
    {
        if (!_onGround)
        {
            _heightVelocity += _Gravity * delta;
            _itemSprite.Position += new Vector2(0f, _heightVelocity) * delta;
            Position += _velocity * delta;

            ZIndex = Mathf.FloorToInt(Position.y - 1);
            _itemSprite.ZIndex = Mathf.FloorToInt(_itemSprite.Position.y);
        }


        if (!_onGround && _itemSprite.Position.y > 0)
        {
            _onGround = true;
            _itemSprite.Position = Vector2.Zero;
            FakeBounce();
        }
    }

    private void FakeBounce()
    {
        Initialise(_velocity * .5f, _prevHeightVelocity * .5f);
        if (_heightVelocity < -0.1f)
            _onGround = false;
    }

    public bool HasInteractParticle()
    {
        return true;
    }

    public Particles2D GetInteractParticles()
    {
        return (Particles2D)_interactParticle.Instance();
    }
}
