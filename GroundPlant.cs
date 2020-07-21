using Godot;
using System;

public class GroundPlant : Sprite, IInteractable
{
    private Item.ItemStack _itemStack;
    private System.Collections.Generic.List<Item> possibleItems = new System.Collections.Generic.List<Item>(new Item[] { Items.ELDERBERRIES, Items.FLY_AGARIC, Items.HOLLY_BERRIES, Items.ORPIMENT, Items.VEILED_STINKHORN });

    public Item.ItemStack ItemStack { get { return _itemStack; } set { _itemStack = value; UpdateSpriteFromItemStack(); } }

    public override void _Ready()
    {
        GD.Randomize();
        ItemStack = new Item.ItemStack(possibleItems[(int)(GD.Randi() % (uint)possibleItems.Count)], 1);

        ZIndex = Mathf.FloorToInt(Position.y);
    }

    private void UpdateSpriteFromItemStack()
    {
        if (_itemStack != null)
        {
            Texture = _itemStack.item.IconTex;
            NormalMap = _itemStack.item.NormalMap;
        }
    }

    public bool Interact(Player player)
    {
        player.PickupItem(ItemStack);

        return true;
    }
}
