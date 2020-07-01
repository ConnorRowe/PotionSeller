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

    private Mortar _mortar;
    private PotionCircle _potionCircle;
    private HBoxContainer _reagentBox;
    private System.Collections.Generic.List<Item> _itemList = new System.Collections.Generic.List<Item>();

    public override void _Ready()
    {
        _mortar = GetNode<Mortar>("Mortar");
        _potionCircle = GetNode<PotionCircle>("PotionCircle");
        _reagentBox = GetNode<HBoxContainer>("ReagentsBox");
        _mortar.Alchemy = this;

        _itemList.Add(Items.FLY_AGARIC);
        _itemList.Add(Items.ORPIMENT);
        _itemList.Add(Items.HOLLY_BERRIES);

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
            buttonControl.RectMinSize = new Vector2(16f, 16f);
            _reagentBox.AddChild(buttonControl);
            newItemButton.Alchemy = this;
            newItemButton.Item = i;
        }
    }

    public void ItemButtonReleased(ItemButton btn)
    {
        GD.Print("Selected: " + btn.Item.Name);
    }
}
