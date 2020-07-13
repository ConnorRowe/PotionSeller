using Godot;
using System;

public class ItemTooltip : Control
{
    private Tween _tween;
    private RichTextLabel _itemName;
    private RichTextLabel _type;
    private RichTextLabel _description;
    private bool isTooltipOpen = false;
    private Item queuedItem;
    private Vector2 queuedPos;
    private bool hasItemQueued = false;

    public override void _Ready()
    {
        _tween = GetNode<Tween>("Tween");
        _itemName = GetNode<RichTextLabel>("VBoxContainer/ItemName");
        _type = GetNode<RichTextLabel>("VBoxContainer/Type");
        _description = GetNode<RichTextLabel>("VBoxContainer/Description");

        RectScale = new Vector2(0f, 1f);

        _tween.Connect("tween_all_completed", this, nameof(TweenAllComplete));
    }

    private void UpdateLabels(Item item)
    {
        _itemName.BbcodeText = "[color=#" + Item.GetRarityColour(item.ItemRarity).ToHtml(false) + "]" + item.Name + "[/color]";
        _type.BbcodeText = "[color=black]" + Item.GetRarityName(item.ItemRarity) + " " + Item.GetItemTypeName(item.Type) + "[/color]";
        _description.Text = item.Description;
    }

    public void Open(Item item, Vector2 pos)
    {
        if (isTooltipOpen)
        {
            Close();
            queuedItem = item;
            queuedPos = pos;
            hasItemQueued = true;
        }
        else
        {
            UpdateLabels(item);
            MarginLeft = pos.x + 64f;
            RectPosition = new Vector2(pos.x, pos.y);
            RectSize = RectMinSize;
            hasItemQueued = false;
            _tween.InterpolateProperty(this, "rect_scale", new Vector2(0f, 1f), new Vector2(1f, 1f), .5f, Tween.TransitionType.Bounce, Tween.EaseType.Out);
            _tween.InterpolateProperty(this, "modulate", new Color(1f, 1f, 1f, 0f), Colors.White, .1f);
            _tween.Start();
        }
    }

    public void Close()
    {
        if (isTooltipOpen && !_tween.IsActive())
        {
            _tween.InterpolateProperty(this, "rect_scale", new Vector2(1f, 1f), new Vector2(0f, 1f), .25f, Tween.TransitionType.Cubic, Tween.EaseType.Out);
            _tween.InterpolateProperty(this, "modulate", Colors.White, new Color(1f, 1f, 1f, 0f), .25f);
            _tween.Start();
        }
    }

    private void TweenAllComplete()
    {
        isTooltipOpen = !isTooltipOpen;

        if (hasItemQueued)
            Open(queuedItem, queuedPos);
    }
}
