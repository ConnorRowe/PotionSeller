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

    public static Color CommonColour = Colors.AliceBlue;
    public static Color UncommonColour = Colors.Chartreuse;
    public static Color RareColour = Colors.MediumBlue;
    public static Color LegendaryColour = Colors.DarkOrange;

    public class ItemStack
    {
        public Item item;
        public int stackCount;

        public ItemStack(Item item, int stackCount)
        {
            this.item = item;
            this.stackCount = stackCount;
        }

        public override string ToString()
        {
            return item.Name + ", " + stackCount.ToString();
        }
    }

    private readonly string _name;
    private readonly ItemType _itemType;
    private readonly Rarity _rarity;
    private readonly Texture _iconTex;
    private readonly Texture _normalMap;
    private readonly Color _potionColour;
    private readonly string _description;

    public string Name { get { return _name; } }
    public ItemType Type { get { return _itemType; } }
    public Rarity ItemRarity { get { return _rarity; } }
    public Texture IconTex { get { return _iconTex; } }
    public Texture NormalMap { get { return _normalMap; } }
    public Color PotionColour { get { return _potionColour; } }
    public string Description { get { return _description; } }


    public Item(string name, ItemType itemType, Rarity rarity, string iconTexFileName, Color potionColour, string description)
    {
        _name = name;
        _itemType = itemType;
        _rarity = rarity;
        _iconTex = GD.Load<Texture>("res://textures/" + iconTexFileName + ".png");
        _normalMap = GD.Load<Texture>("res://textures/normal/" + iconTexFileName + "_n.png");
        _potionColour = potionColour;
        _description = description;
    }

    public static Color GetRarityColour(Item.Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                return CommonColour;
            case Rarity.Uncommon:
                return UncommonColour;
            case Rarity.Rare:
                return RareColour;
            case Rarity.Legendary:
                return LegendaryColour;
        }
        return Colors.Magenta;
    }

    public static string GetRarityName(Item.Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                return "Common";
            case Rarity.Uncommon:
                return "Uncommon";
            case Rarity.Rare:
                return "Rare";
            case Rarity.Legendary:
                return "Legendary";
        }
        return string.Empty;
    }

    public static string GetItemTypeName(Item.ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Reagent:
                return "Reagent";
            case ItemType.Solvent:
                return "Solvent";
            case ItemType.Finisher:
                return "Finisher";
        }
        return string.Empty;
    }

    public static ItemStack DecreaseItemStackCount(ItemStack itemStack, int count)
    {
        return new ItemStack(itemStack.item, itemStack.stackCount - count);
    }
}
