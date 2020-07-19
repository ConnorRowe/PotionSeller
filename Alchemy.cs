using Godot;
using System;

public class Alchemy : Node2D
{
    private enum AlchemyStage
    {
        MortarPestle,
        Calcinator,
        Retort,
        Alembic
    }

    private enum MortarPestleStage
    {
        PickReagents,
        Crush
    }

    private enum ReagentAnim
    {
        Fade,
        Hover,
        Fall,
        End
    }

    // State machines
    private AlchemyStage _alchemyStage = AlchemyStage.MortarPestle;
    private MortarPestleStage _mortarPestleStage = MortarPestleStage.PickReagents;
    private ReagentAnim _reagentAnimState = ReagentAnim.Fade;

    // Nodes
    private Mortar _mortar;
    private PotionCircle _potionCircle;
    private VBoxContainer _potionReagentsBox;
    private AcceptDialog _helpDialog;
    private Button _proceedToCrush;
    private Tween _tween;
    private Inventory _inventory;
    private ItemTooltip _itemTooltip;
    private DebugOverlay _debugOverlay;

    // Lists
    private System.Collections.Generic.List<Item.ItemStack> _itemList = new System.Collections.Generic.List<Item.ItemStack>();
    private System.Collections.Generic.List<Item> _potionReagents = new System.Collections.Generic.List<Item>();
    private Sprite[] _reagentAnimSprites = new Sprite[4];

    // Assets
    private Texture _itemBtnBg;
    private Texture _singleItemSlot;
    private DynamicFont _smallFont;

    public override void _Ready()
    {
        GD.Randomize();

        // Get references to all the nodes we need to manipulate
        _mortar = GetNode<Mortar>("MortarPestle/Mortar");
        _potionCircle = GetNode<PotionCircle>("MortarPestle/Crush/PotionCircle");
        _potionReagentsBox = GetNode<VBoxContainer>("MortarPestle/PickReagents/PotionReagentsBox");
        _helpDialog = GetNode<AcceptDialog>("HelpDialog");
        _proceedToCrush = GetNode<Button>("MortarPestle/PickReagents/ProceedToCrush");
        _tween = GetNode<Tween>("MortarPestle/PickReagents/Tween");
        _inventory = GetNode<Inventory>("MortarPestle/PickReagents/Inventory");
        _inventory.SetSize(4, 4);
        _inventory.DrawPosition = new Vector2(324f, 16f);
        _inventory.CanDeselect = false;
        _itemTooltip = GetNode<ItemTooltip>("MortarPestle/PickReagents/ItemTooltip");
        _debugOverlay = GetNode<DebugOverlay>("DebugOverlay");

        // Load assets needed
        _itemBtnBg = GD.Load<Texture>("res://textures/item_slot.png");
        _singleItemSlot = GD.Load<Texture>("res://textures/single_item_slot.png");
        _smallFont = GD.Load<DynamicFont>("res://font/small_font.tres");

        // Signal connections
        GetNode<TouchScreenButton>("HelpButton").Connect("released", this, nameof(DisplayHelpPopup));
        _proceedToCrush.Connect("pressed", this, nameof(ProceedToCrushPressed));
        _tween.Connect("tween_all_completed", this, nameof(ReagentFadeTweenCompleted));
        _inventory.Connect("ItemSlotSelected", this, nameof(InventorySlotSelected));
        GetNode<Button>("MortarPestle/PickReagents/AddToPotion").Connect("pressed", this, nameof(ItemButtonReleased));

        // Adding items to simulate an inventory
        _itemList.AddRange(new Item.ItemStack[] { new Item.ItemStack(Items.BRIMSTONE, 1), new Item.ItemStack(Items.FLY_AGARIC, 3), new Item.ItemStack(Items.ELDERBERRIES, 5), new Item.ItemStack(Items.ORPIMENT, 1), new Item.ItemStack(Items.HOLLY_BERRIES, 3) });
        _inventory.UpdateSlots(_itemList);

        // Tracking stuff for debug
        _debugOverlay.TrackProperty(nameof(_mortarPestleStage), this, "MortarPestleStage");
        _debugOverlay.TrackProperty(nameof(_mortar.CurrentParticleColour), _mortar, "Splash Colour");
    }

    public void PestleHitMortar(float power)
    {
        if (_mortarPestleStage == MortarPestleStage.Crush)
            _potionCircle.InitiateCircleFill(power);

    }

    public void ItemButtonReleased()
    {
        if (_inventory.SelectedItemId < 0)
            return;

        Item.ItemStack selectedItemStack = _itemList[_inventory.SelectedItemId];

        if (_alchemyStage == AlchemyStage.MortarPestle && _mortarPestleStage == MortarPestleStage.PickReagents && _potionReagents.Count < 4)
        {
            if (selectedItemStack.stackCount > 1)
            {
                _itemList[_inventory.SelectedItemId] = Item.DecreaseItemStackCount(selectedItemStack, 1);
            }
            else
            {
                _itemList.RemoveAt(_inventory.SelectedItemId);
            }

            _inventory.Update();

            HBoxContainer itemInfo = new HBoxContainer();
            itemInfo.Set("custom_constants/separation", 10f);

            Control itemIcon = new Control();
            itemIcon.RectMinSize = new Vector2(16f, 16f);

            Sprite itemBG = new Sprite();
            itemBG.Texture = _singleItemSlot;
            itemBG.Centered = false;

            Sprite itemSprite = new Sprite();
            itemSprite.Texture = selectedItemStack.item.IconTex;
            itemSprite.Centered = false;
            itemSprite.Position = new Vector2(2f, 2f);
            itemBG.AddChild(itemSprite);
            itemIcon.AddChild(itemBG);
            itemInfo.AddChild(itemIcon);

            Label itemName = new Label();
            itemName.Text = selectedItemStack.item.Name;
            itemName.AddFontOverride("font", _smallFont);
            itemName.MarginLeft = 4f;
            itemInfo.AddChild(itemName);

            _potionReagentsBox.AddChild(itemInfo);

            _potionReagents.Add(selectedItemStack.item);
            _proceedToCrush.Disabled = false;
        }

        GD.Print("Selected: " + selectedItemStack.item.Name);
    }

    public void DisplayHelpPopup()
    {
        _helpDialog.PopupCentered();

        switch (_alchemyStage)
        {
            case AlchemyStage.MortarPestle:
                {
                    switch (_mortarPestleStage)
                    {
                        case MortarPestleStage.PickReagents:
                            {
                                _helpDialog.DialogText = "Select your reagents from the list to the right.";
                                break;
                            }
                        case MortarPestleStage.Crush:
                            {
                                _helpDialog.DialogText = "Crush up the reagents with the mortar and pestle! Just pick up the pestle and start smashing.";
                                break;
                            }
                    }
                    break;
                }
            case AlchemyStage.Calcinator:
                {
                    break;
                }
            case AlchemyStage.Alembic:
                {
                    break;
                }
            case AlchemyStage.Retort:
                {
                    break;
                }
        }
    }

    public void NextStage()
    {
        switch (_alchemyStage)
        {
            case AlchemyStage.MortarPestle:
                {
                    if (_mortarPestleStage == MortarPestleStage.PickReagents)
                    {
                        _mortarPestleStage = MortarPestleStage.Crush;
                        GetNode<Node2D>("MortarPestle/PickReagents").Visible = false;
                        GetNode<Node2D>("MortarPestle/Crush").Visible = true;
                        for (int i = 0; i < _potionReagents.Count; i++)
                        {
                            _mortar.ReagentColours[i] = _potionReagents[i].PotionColour;

                            if (i > 0)
                                _mortar.AverageColour = _potionReagents[i].PotionColour.LinearInterpolate(_potionReagents[i - 1].PotionColour, .5f);
                        }
                    }
                    break;
                }
        }
    }

    public void ProceedToCrushPressed()
    {
        for (int i = 0; i < _potionReagentsBox.GetChildCount(); i++)
        {
            //ItemSprite
            Node2D N = ((Node)_potionReagentsBox.GetChildren()[i]).GetChild(0).GetChild(0) as Node2D;

            _tween.InterpolateProperty(GetNode("MortarPestle/PickReagents"), "modulate", Colors.White, new Color(1f, 1f, 1f, 0f), .5f);
            _tween.InterpolateProperty(_proceedToCrush, "modulate", Colors.White, new Color(1f, 1f, 1f, 0f), .5f);

            Sprite reagentSprite = new Sprite()
            {
                Texture = _potionReagents[i].IconTex,
                GlobalPosition = N.GlobalPosition + new Vector2(2f, 2f),
                Centered = false,
                ZIndex = -2

            };
            AddChild(reagentSprite);
            _reagentAnimSprites[i] = reagentSprite;

        }
        _tween.Start();
        _reagentAnimState = ReagentAnim.Hover;
    }

    public void ReagentFadeTweenCompleted()
    {
        if (_reagentAnimState == ReagentAnim.Hover)
        {
            _proceedToCrush.Disabled = true;

            foreach (Node N in _potionReagentsBox.GetChildren())
            {
                N.QueueFree();
            }

            foreach (Sprite S in _reagentAnimSprites)
            {
                if (S != null)
                {
                    _tween.InterpolateProperty(S, "global_position", S.GlobalPosition, new Vector2(160f + (float)GD.RandRange(-10, 10), 40f + (float)GD.RandRange(-10, 10)), 1f, Tween.TransitionType.Cubic);
                }
            }

            _tween.Start();
            _reagentAnimState = ReagentAnim.Fall;
        }
        else if (_reagentAnimState == ReagentAnim.Fall)
        {
            foreach (Sprite S in _reagentAnimSprites)
            {
                if (S != null)
                {
                    _tween.InterpolateProperty(S, "global_position", S.GlobalPosition, _mortar.GlobalPosition + new Vector2((float)GD.RandRange(-10, 10), (float)GD.RandRange(-10, 10)), 1.5f, Tween.TransitionType.Cubic);
                }
            }

            _tween.Start();
            _reagentAnimState = ReagentAnim.End;
        }
        else if (_reagentAnimState == ReagentAnim.End)
        {
            NextStage();
            for (int i = 0; i < _reagentAnimSprites.Length; i++)
            {
                if (_reagentAnimSprites[i] != null)
                {
                    _reagentAnimSprites[i].QueueFree();
                    _reagentAnimSprites[i] = null;
                }
            }
        }
    }

    private void InventorySlotSelected(int slotId)
    {
        if (slotId >= 0)
            _itemTooltip.Open(_itemList[slotId].item, _itemTooltip.RectPosition);
        else
            _itemTooltip.Close();
    }
}
