using Godot;
using System;

public class Inventory : Control
{
    private int _columns = 8;
    private int _rows = 1;
    private Vector2 itemTexOffset = new Vector2(2f, 2f);

    Texture _itemSlot;
    Texture _slotEnd;
    DynamicFont _smallFont;

    private System.Collections.Generic.List<Item.ItemStack> _itemStacks;

    public override void _Ready()
    {
        _itemSlot = GD.Load<Texture>("res://textures/item_slot.png");
        _smallFont = GD.Load<DynamicFont>("res://font/small_font.tres");
    }

    public override void _Draw()
    {
        int itemId = 0;

        for (int y = 0; y < _rows; y++)
        {
            for (int x = 0; x <= _columns; x++)
            {
                if (x == _columns)
                    DrawTextureRectRegion(_itemSlot, new Rect2(new Vector2(MarginLeft + (x * _itemSlot.GetWidth()), MarginTop + (y * _itemSlot.GetHeight())), new Vector2(2f, _itemSlot.GetHeight())), new Rect2(0f, 0f, 2f, 20f));
                else
                {
                    Vector2 texPos = new Vector2(MarginLeft + (x * _itemSlot.GetWidth()), MarginTop + (y * _itemSlot.GetHeight()));

                    DrawTexture(_itemSlot, texPos);

                    if (itemId < _itemStacks.Count)
                    {
                        DrawTexture(_itemStacks[itemId].item.IconTex, texPos + itemTexOffset);
                        string stackCount = _itemStacks[itemId].stackCount.ToString();
                        float characterOffset = 4.1f * (stackCount.Length - 1);
                        DrawString(_smallFont, texPos + new Vector2(14.5f - characterOffset, 16.5f), stackCount, Colors.DarkSlateGray);
                        DrawString(_smallFont, texPos + new Vector2(14f - characterOffset, 16f), stackCount);
                        itemId++;
                    }
                }
            }
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventScreenTouch eventScreenTouch && eventScreenTouch.Pressed)
        {
            // If touch position is inside the inventory rectangle
            if (new Rect2(eventScreenTouch.Position, new Vector2(1f, 1f)).Intersects(new Rect2(MarginLeft, MarginTop, _columns * _itemSlot.GetWidth(), _rows * _itemSlot.GetHeight())))
            {
                int x = Mathf.FloorToInt((eventScreenTouch.Position.x - MarginLeft) / _itemSlot.GetWidth());
                int y = Mathf.FloorToInt((eventScreenTouch.Position.y - MarginTop) / _itemSlot.GetHeight());
                int itemId = x + (y * _columns);

                if(itemId < _itemStacks.Count)
                {
                    GD.Print("Item overlap: " + _itemStacks[itemId].ToString());
                }
            }
        }
    }

    public void UpdateSlots(System.Collections.Generic.List<Item.ItemStack> itemStackList)
    {
        _itemStacks = itemStackList;

        Update();
    }
}
