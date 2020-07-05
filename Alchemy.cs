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
    private HBoxContainer _inventoryBox;
    private VBoxContainer _potionReagentsBox;
    private AcceptDialog _helpDialog;
    private Button _proceedToCrush;
    private Tween _tween;

    // Lists
    private System.Collections.Generic.List<Item> _itemList = new System.Collections.Generic.List<Item>();
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
        _inventoryBox = GetNode<HBoxContainer>("MortarPestle/PickReagents/InventoryBox");
        _potionReagentsBox = GetNode<VBoxContainer>("MortarPestle/PickReagents/PotionReagentsBox");
        _mortar.Alchemy = this;
        _helpDialog = GetNode<AcceptDialog>("HelpDialog");
        _proceedToCrush = GetNode<Button>("MortarPestle/PickReagents/ProceedToCrush");
        _tween = GetNode<Tween>("MortarPestle/PickReagents/Tween");

        // Load assets needed
        _itemBtnBg = GD.Load<Texture>("res://textures/item_slot.png");
        _singleItemSlot = GD.Load<Texture>("res://textures/single_item_slot.png");
        _smallFont = GD.Load<DynamicFont>("res://font/small_font.tres");

        // Signal connections
        GetNode<TouchScreenButton>("HelpButton").Connect("released", this, nameof(DisplayHelpPopup));
        _proceedToCrush.Connect("pressed", this, nameof(ProceedToCrushPressed));
        _tween.Connect("tween_all_completed", this, nameof(ReagentFadeTweenCompleted));

        // Adding items to simulate an inventory
        _itemList.Add(Items.FLY_AGARIC);
        _itemList.Add(Items.ORPIMENT);
        _itemList.Add(Items.HOLLY_BERRIES);
        _itemList.Add(Items.BRIMSTONE);
        _itemList.Add(Items.ELDERBERRIES);

        // Generate buttons for each item
        CreateReagentButtons();
    }

    public void PestleHitMortar(float power)
    {
        if (_mortarPestleStage == MortarPestleStage.Crush)
            _potionCircle.InitiateCircleFill(power);

    }

    public void CreateReagentButtons()
    {
        foreach (Item i in _itemList)
        {
            ItemButton newItemButton = new ItemButton();
            newItemButton.Normal = i.IconTex;
            newItemButton.Alchemy = this;
            newItemButton.Item = i;

            Control buttonControl = new Control();
            buttonControl.AddChild(newItemButton);
            buttonControl.RectMinSize = new Vector2(32f, 32f);

            _inventoryBox.AddChild(buttonControl);

            Sprite buttonBg = new Sprite();
            buttonBg.Texture = _itemBtnBg;
            buttonBg.Offset = new Vector2(7f, 8f);
            buttonBg.ZIndex = -1;

            newItemButton.AddChild(buttonBg);
        }

        Sprite closingSprite = new Sprite();
        Control endControl = new Control();
        endControl.RectMinSize = new Vector2(32f, 32f);

        closingSprite.Texture = _itemBtnBg;
        closingSprite.Offset = new Vector2(-1, 8f);
        closingSprite.RegionEnabled = true;
        closingSprite.RegionRect = new Rect2(0f, 0f, 2f, 20f);
        closingSprite.Scale = Vector2.One * 2;

        _inventoryBox.AddChild(endControl);
        endControl.AddChild(closingSprite);
    }

    public void ItemButtonReleased(ItemButton btn)
    {
        if (_alchemyStage == AlchemyStage.MortarPestle && _mortarPestleStage == MortarPestleStage.PickReagents && _potionReagents.Count < 4)
        {
            HBoxContainer itemInfo = new HBoxContainer();
            itemInfo.Set("custom_constants/separation", 10f);

            Control itemIcon = new Control();
            itemIcon.RectMinSize = new Vector2(16f, 16f);

            Sprite itemBG = new Sprite();
            itemBG.Texture = _singleItemSlot;
            itemBG.Centered = false;

            Sprite itemSprite = new Sprite();
            itemSprite.Texture = btn.Item.IconTex;
            itemSprite.Centered = false;
            itemSprite.Position = new Vector2(2f, 2f);
            itemBG.AddChild(itemSprite);
            itemIcon.AddChild(itemBG);
            itemInfo.AddChild(itemIcon);

            Label itemName = new Label();
            itemName.Text = btn.Item.Name;
            itemName.AddFontOverride("font", _smallFont);
            itemName.MarginLeft = 4f;
            itemInfo.AddChild(itemName);

            _potionReagentsBox.AddChild(itemInfo);

            _potionReagents.Add(btn.Item);
            _proceedToCrush.Disabled = false;
        }

        GD.Print("Selected: " + btn.Item.Name);
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
}
