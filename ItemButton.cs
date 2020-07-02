using Godot;
using System;

public class ItemButton : TouchScreenButton
{
    private Item _item;
    private Alchemy _alchemy;

    public Item @Item { get { return _item; } set { _item = value; } }
    public Alchemy @Alchemy { get { return _alchemy; } set { _alchemy = value; } }

    public override void _Ready()
    {
        var rectangle = new RectangleShape2D();
        rectangle.Extents = new Vector2(8f, 8f);
        Shape = rectangle;
        Scale = new Vector2(2f, 2f);
        Connect("released", this, nameof(ButtonReleased));
    }

    private void ButtonReleased()
    {
        _alchemy.ItemButtonReleased(this);
    }
}
