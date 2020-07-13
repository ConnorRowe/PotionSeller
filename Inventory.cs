using Godot;
using System;

public class Inventory : Control
{
    private int _columns = 8;
    private int _rows = 1;
    private int _selectedItemId = -1;
    private Vector2 _itemTexOffset = new Vector2(2f, 2f);
    private float _scale = 1.0f;

    Texture _itemSlot;
    Texture _itemSlotHighlight;
    Texture _slotEnd;
    DynamicFont _smallFont;
    ItemTooltip _itemTooltip;

    private System.Collections.Generic.List<Item.ItemStack> _itemStacks;

    public float Scale { get { return _scale; } set { _scale = value; _smallFont.Size = Mathf.FloorToInt(12 * Scale); } }

    public override void _Ready()
    {
        _itemSlot = GD.Load<Texture>("res://textures/item_slot.png");
        _itemSlotHighlight = GD.Load<Texture>("res://textures/item_slot_highlight.png");
        _smallFont = GD.Load<DynamicFont>("res://font/small_font.tres");

        _itemTooltip = GetParent().GetNode<ItemTooltip>("ItemTooltip");

        Scale = 1.5f;
    }

    public override void _Draw()
    {
        int itemId = 0;

        for (int y = 0; y < _rows; y++)
        {
            for (int x = 0; x <= _columns; x++)
            {
                if (x == _columns)
                    DrawTextureRectRegion(_itemSlot, new Rect2(new Vector2(MarginLeft + (x * (_itemSlot.GetWidth() * _scale)), MarginTop + (y * (_itemSlot.GetHeight() * _scale))), new Vector2(2f * _scale, _itemSlot.GetHeight() * _scale)), new Rect2(0f, 0f, 2f, 20f));
                else
                {
                    Vector2 texPos = new Vector2(MarginLeft + (x * _itemSlot.GetWidth() * _scale), MarginTop + (y * _itemSlot.GetHeight() * _scale));

                    DrawTextureRect(_itemSlot, new Rect2(texPos, new Vector2(_itemSlot.GetWidth() * _scale, _itemSlot.GetHeight() * _scale)), false);

                    if (itemId < _itemStacks.Count)
                    {
                        if (itemId == _selectedItemId)
                            DrawTextureRect(_itemSlotHighlight, new Rect2(texPos, new Vector2(_itemSlotHighlight.GetWidth() * _scale, _itemSlotHighlight.GetHeight() * _scale)), false, Item.GetRarityColour(_itemStacks[itemId].item.ItemRarity));

                        DrawTextureRect(_itemStacks[itemId].item.IconTex, new Rect2(texPos + (_itemTexOffset * _scale), new Vector2(_itemStacks[itemId].item.IconTex.GetWidth() * _scale, _itemStacks[itemId].item.IconTex.GetHeight() * _scale)), false);
                        string stackCount = _itemStacks[itemId].stackCount.ToString();
                        float characterOffset = 4.1f * (stackCount.Length - 1) * _scale;
                        DrawString(_smallFont, texPos + new Vector2((_itemSlotHighlight.GetWidth() * .77f * _scale) - characterOffset, _itemSlotHighlight.GetHeight() * .87f * _scale), stackCount, Colors.DarkSlateGray);
                        DrawString(_smallFont, texPos + new Vector2((_itemSlotHighlight.GetWidth() * .75f * _scale) - characterOffset, _itemSlotHighlight.GetHeight() * .85f * _scale), stackCount);

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
            if (new Rect2(eventScreenTouch.Position, new Vector2(1f, 1f)).Intersects(new Rect2(MarginLeft, MarginTop, _columns * _itemSlot.GetWidth() * _scale, _rows * _itemSlot.GetHeight() * _scale)))
            {
                int x = Mathf.FloorToInt((eventScreenTouch.Position.x - MarginLeft) / (_itemSlot.GetWidth() * _scale));
                int y = Mathf.FloorToInt((eventScreenTouch.Position.y - MarginTop) / (_itemSlot.GetHeight() * _scale));
                int itemId = x + (y * _columns);

                if (itemId < _itemStacks.Count)
                {
                    GD.Print("Item overlap: " + _itemStacks[itemId].ToString());
                    SelectSlot(itemId);
                }
            }
            else
            {
                SelectSlot(-1);
            }
        }
    }

    public void UpdateSlots(System.Collections.Generic.List<Item.ItemStack> itemStackList)
    {
        _itemStacks = itemStackList;

        Update();
    }

    public void SelectSlot(int itemId)
    {
        _selectedItemId = itemId;

        if (itemId >= 0)
        {
            int x = Mathf.CeilToInt((float)itemId + 1 / (float)_rows);
            int y = Mathf.CeilToInt((float)itemId % (float)_rows);
            _itemTooltip.Open(_itemStacks[itemId].item, new Vector2(((x * _itemSlot.GetWidth() * _scale) + _itemTexOffset.x) + MarginLeft + (_itemSlot.GetWidth() * 1.5f * _scale), (((y + 1) * _itemSlot.GetHeight() * _scale) + _itemTexOffset.y) + MarginTop + (4f * _scale)));
        }
        else
            _itemTooltip.Close();

        Update();
    }
}
