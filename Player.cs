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

    // Nodes
    private JoystickButton _joystick = null;
    private Direction _playerDirection = Direction.Right;
    private Sprite _playerSprite;
    private Inventory _inventory;
    private DebugOverlay _debugOverlay;
    private ItemTooltip _itemTooltip;

    // Assets
    private Texture _runForwards;
    private Texture _runForwardsNormal;
    private Texture _runBackwards;
    private Texture _runBackwardsNormal;
    private Texture _runRight;
    private Texture _runRightNormal;
    private AudioStreamOGGVorbis _forestSong;
    private AudioStreamPlayer _audioPlayer;

    // Gameplay vars
    private const float _MaxSpeed = 100f;
    private const float _Acceleration = 1000f;
    private Vector2 _motion = Vector2.Zero;
    private const float _SpriteFPS = 8f;
    private bool _playerIsMoving = false;

    private System.Collections.Generic.List<Item.ItemStack> _invItems = new System.Collections.Generic.List<Item.ItemStack>();

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
        // Get nodes
        _joystick = GetParent().GetNode("CanvasLayer/Joystick").GetChild<JoystickButton>(0);
        _playerSprite = GetNode<Sprite>("PlayerSprite");
        _inventory = GetParent().GetNode<Inventory>("CanvasLayer/Inventory");
        _debugOverlay = GetParent().GetNode<DebugOverlay>("DebugOverlay");
        _audioPlayer = GetNode<AudioStreamPlayer>("AudioPlayer");
        _itemTooltip = (ItemTooltip)GD.Load<PackedScene>("res://ItemTooltip.tscn").Instance();
        GetParent().GetNode("CanvasLayer").AddChild(_itemTooltip);

        // Load assets
        _runForwards = GD.Load<Texture>("res://textures/Player_run_forwards.png");
        _runBackwards = GD.Load<Texture>("res://textures/Player_run_backwards.png");
        _runRight = GD.Load<Texture>("res://textures/Player_run_right.png");
        _runForwardsNormal = GD.Load<Texture>("res://textures/normal/Player_run_forwards_n.png");
        _runBackwardsNormal = GD.Load<Texture>("res://textures/normal/Player_run_backwards_n.png");
        _runRightNormal = GD.Load<Texture>("res://textures/normal/Player_run_right_n.png");
        _forestSong = GD.Load<AudioStreamOGGVorbis>("res://audio/music/enchanted_forest_longer.ogg");

        Timer spriteAnimTimer = GetNode<Timer>("SpriteAnimateTimer");
        spriteAnimTimer.WaitTime = 1f / _SpriteFPS;
        spriteAnimTimer.Connect("timeout", this, nameof(AnimateSprite));

        _invItems.AddRange(new Item.ItemStack[] { new Item.ItemStack(Items.BRIMSTONE, 1), new Item.ItemStack(Items.FLY_AGARIC, 3), new Item.ItemStack(Items.ELDERBERRIES, 5), new Item.ItemStack(Items.AQUA_FORTIS, 1), new Item.ItemStack(Items.HOLLY_BERRIES, 3), new Item.ItemStack(Items.AQUA_VITAE, 1) });
        GetParent().GetNode<Inventory>("CanvasLayer/Inventory").UpdateSlots(_invItems);

        _debugOverlay.TrackProperty(nameof(_motion), this);
        _debugOverlay.TrackFunc(nameof(DirectionAsString), this);
        _debugOverlay.TrackProperty(nameof(_inventory.Scale), _inventory, "Inventory Scale");

        _inventory.Connect("ItemSlotSelected", this, nameof(InventorySlotSelected));

        _audioPlayer.Stream = _forestSong;
        //_audioPlayer.Play();
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
            _playerIsMoving = true;
        }
        else
        {
            _playerIsMoving = false;
            _playerSprite.Frame = 0;
        }
    }

    //For testing only
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent && keyEvent.Pressed)
        {
            if ((KeyList)keyEvent.Scancode == KeyList.T)
            {
                PickupItem(new Item.ItemStack(Items.ORPIMENT, 1));
                _inventory.Update();
            }
        }
    }

    private void DirectionUpdated()
    {
        switch (_playerDirection)
        {
            case Direction.Down:
                _playerSprite.Texture = _runBackwards;
                _playerSprite.NormalMap = _runBackwardsNormal;
                _playerSprite.FlipH = false;
                break;
            case Direction.Left:
                _playerSprite.Texture = _runRight;
                _playerSprite.NormalMap = _runRightNormal;
                _playerSprite.FlipH = true;
                break;
            case Direction.Right:
                _playerSprite.Texture = _runRight;
                _playerSprite.NormalMap = _runRightNormal;
                _playerSprite.FlipH = false;
                break;
            case Direction.Up:
                _playerSprite.Texture = _runForwards;
                _playerSprite.NormalMap = _runForwardsNormal;
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

    private void AnimateSprite()
    {
        if (_playerIsMoving)
        {
            int i = _playerSprite.Frame;
            i++;
            if (i > 7)
                i = 0;
            _playerSprite.Frame = i;
        }
    }

    private void PickupItem(Item.ItemStack itemStack)
    {
        for (int i = 0; i < _invItems.Count; i++)
        {
            if (_invItems[i].item == itemStack.item)
            {
                _invItems[i] = new Item.ItemStack(itemStack.item, _invItems[i].stackCount + itemStack.stackCount);
                _inventory.Update();
                return;
            }
        }

        _invItems.Add(itemStack);
        _inventory.Update();
    }

    private void InventorySlotSelected(int slotId)
    {
        if (slotId >= 0)
            _itemTooltip.Open(_invItems[slotId].item, _inventory.GetSlotPosition(slotId));
        else
            _itemTooltip.Close();
    }

    public String DirectionAsString()
    {
        switch (_playerDirection)
        {
            case Direction.Up:
                return "Up";
            case Direction.Down:
                return "Down";
            case Direction.Left:
                return "Left";
            case Direction.Right:
                return "Right";
        }
        return "";
    }
}
