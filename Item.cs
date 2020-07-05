using Godot;
using System;

public class Item : Reference
{
    public enum ItemType
    {
        Reagent,
        Solvent,
        Finisher
    }

    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Legendary
    }

    private readonly string _name;
    private readonly ItemType _itemType;
    private readonly Rarity _rarity;
    private readonly Texture _iconTex;
    private readonly Color _potionColour;
    private readonly string _description;

    public string Name { get { return _name; } }
    public ItemType Type { get { return _itemType; } }
    public Rarity ItemRarity { get { return _rarity; } }
    public Texture IconTex { get { return _iconTex; } }
    public Color PotionColour { get { return _potionColour; } }
    public string Description { get { return _description; } }


    public Item(string name, ItemType itemType, Rarity rarity, string iconTexFileName, Color potionColour, string description)
    {
        _name = name;
        _itemType = itemType;
        _rarity = rarity;
        _iconTex = GD.Load<Texture>("res://textures/" + iconTexFileName + ".png");
        _potionColour = potionColour;
        _description = description;
    }
}
