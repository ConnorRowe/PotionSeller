using Godot;
using System;

public class Inventory : Control
{
    private int _columns = 8;
    private int _rows = 1;
    private int _selectedItemId = -1;
    private Vector2 _itemTexOffset = new Vector2(2f, 2f);
    private float _scale = 1.0f;
    private Vector2 _drawPosition = new Vector2();
    private bool _canDeselect = true;

    private Texture _itemSlot;
    private Texture _itemSlotHighlight;
    private DynamicFont _smallFont;

    private System.Collections.Generic.List<Item.ItemStack> _itemStacks;

    public float Scale { get { return _scale; } set { _scale = value; _smallFont.Size = Mathf.FloorToInt(12 * Scale); Update(); } }
    public Vector2 DrawPosition { get { return _drawPosition; } set { _drawPosition = value; Update(); } }
    public int SelectedItemId { get { return _selectedItemId; } }
    public bool CanDeselect { get { return _canDeselect; } set { _canDeselect = value; } }

    [Signal]
    delegate void ItemSlotSelected(int slotId);

    public override void _Ready()
    {
        _itemSlot = GD.Load<Texture>("res://textures/item_slot.png");
        _itemSlotHighlight = GD.Load<Texture>("res://textures/item_slot_highlight.png");
        _smallFont = GD.Load<DynamicFont>("res://font/small_font.tres");

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
                    DrawTextureRectRegion(_itemSlot, new Rect2(new Vector2(DrawPosition.x + (x * (_itemSlot.GetWidth() * _scale)), DrawPosition.y + (y * (_itemSlot.GetHeight() * _scale))), new Vector2(2f * _scale, _itemSlot.GetHeight() * _scale)), new Rect2(0f, 0f, 2f, 20f));
                else
                {
                    Vector2 texPos = new Vector2(DrawPosition.x + (x * _itemSlot.GetWidth() * _scale), DrawPosition.y + (y * _itemSlot.GetHeight() * _scale));

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
            if (new Rect2(eventScreenTouch.Position, new Vector2(1f, 1f)).Intersects(new Rect2(DrawPosition.x, DrawPosition.y, _columns * _itemSlot.GetWidth() * _scale, _rows * _itemSlot.GetHeight() * _scale)))
            {
                int x = Mathf.FloorToInt((eventScreenTouch.Position.x - DrawPosition.x) / (_itemSlot.GetWidth() * _scale));
                int y = Mathf.FloorToInt((eventScreenTouch.Position.y - DrawPosition.y) / (_itemSlot.GetHeight() * _scale));
                int itemId = x + (y * _columns);

                if (itemId < _itemStacks.Count)
                {
                    GD.Print("Item selected: " + _itemStacks[itemId].ToString());
                    SelectSlot(itemId);
                }
            }
            else if (_canDeselect)
            {
                SelectSlot(-1);
            }
        }

        if (@event.IsActionPressed("debug_scrollup"))
        {
            Scale += 0.1f;
        }
        if (@event.IsActionPressed("debug_scrolldown"))
        {
            Scale -= 0.1f;
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
        EmitSignal(nameof(ItemSlotSelected), itemId);

        Update();
    }

    public Vector2 GetSlotPosition(int slotId)
    {
        int x = Mathf.CeilToInt((float)slotId + 1 / (float)_rows);
        int y = Mathf.CeilToInt((float)slotId % (float)_rows);
        return new Vector2(((x * _itemSlot.GetWidth() * _scale) + _itemTexOffset.x) + MarginLeft + (_itemSlot.GetWidth() * 1.5f * _scale), (((y + 1) * _itemSlot.GetHeight() * _scale) + _itemTexOffset.y) + MarginTop + (4f * _scale));
    }

    public void SetSize(int slotsX, int slotsY)
    {
        _columns = slotsX;
        _rows = slotsY;

        Update();
    }
}
