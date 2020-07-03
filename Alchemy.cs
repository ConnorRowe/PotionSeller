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

    private AlchemyStage _alchemyStage = AlchemyStage.MortarPestle;

    private Mortar _mortar;
    private PotionCircle _potionCircle;
    private HBoxContainer _reagentBox;
    private System.Collections.Generic.List<Item> _itemList = new System.Collections.Generic.List<Item>();

    private Texture _itemBtnBg;

    public override void _Ready()
    {
        _mortar = GetNode<Mortar>("Mortar");
        _potionCircle = GetNode<PotionCircle>("PotionCircle");
        _reagentBox = GetNode<HBoxContainer>("ReagentsBox");
        _mortar.Alchemy = this;
        _itemBtnBg = GD.Load<Texture>("res://textures/item_slot.png");

        _itemList.Add(Items.FLY_AGARIC);
        _itemList.Add(Items.ORPIMENT);
        _itemList.Add(Items.HOLLY_BERRIES);
        _itemList.Add(Items.ELDERBERRIES);

        CreateReagentButtons();
    }

    public void PestleHitMortar(float power)
    {
        _potionCircle.InitiateCircleFill(power);

    }

    public void CreateReagentButtons()
    {
        foreach (Item i in _itemList)
        {
            ItemButton newItemButton = new ItemButton();
            newItemButton.Normal = i.IconTex;
            Control buttonControl = new Control();
            buttonControl.AddChild(newItemButton);
            buttonControl.RectMinSize = new Vector2(32f, 32f);
            _reagentBox.AddChild(buttonControl);
            newItemButton.Alchemy = this;
            newItemButton.Item = i;
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
        _reagentBox.AddChild(endControl);
        endControl.AddChild(closingSprite);
    }

    public void ItemButtonReleased(ItemButton btn)
    {
        switch (_alchemyStage)
        {
            case AlchemyStage.MortarPestle:
                {
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

        GD.Print("Selected: " + btn.Item.Name);
    }
}
