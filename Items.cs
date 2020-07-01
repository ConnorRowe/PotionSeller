using Godot;
using System;

public class Items : Reference
{
    // Items
    public static Item FLY_AGARIC = new Item("Fly Agaric", Item.ItemType.Reagent, Item.Rarity.Uncommon, "fly_agaric", Colors.LightPink);
    public static Item ORPIMENT = new Item("Orpiment", Item.ItemType.Reagent, Item.Rarity.Rare, "orpiment", Colors.Orange);
    public static Item HOLLY_BERRIES = new Item("Holly Berries", Item.ItemType.Reagent, Item.Rarity.Common, "holly_berries", Colors.IndianRed);

}
