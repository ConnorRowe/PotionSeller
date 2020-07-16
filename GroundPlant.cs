using Godot;
using System;

public class GroundPlant : Sprite, IInteractable
{
    private Item.ItemStack _itemStack;

    public Item.ItemStack ItemStack { get { return _itemStack; } set { _itemStack = value; UpdateSpriteFromItemStack(); } }

    public override void _Ready()
    {
        ItemStack = new Item.ItemStack(Items.FLY_AGARIC, 1);

        ZIndex = Mathf.FloorToInt(Position.y);
    }

    private void UpdateSpriteFromItemStack()
    {
        if (_itemStack != null)
        {
            Texture = _itemStack.item.IconTex;
        }
    }

    public bool Interact(Player player)
    {
        player.PickupItem(ItemStack);

        return true;
    }
}
