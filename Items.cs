using Godot;
using System;

public class Items : Reference
{
    // Items
    public static Item FLY_AGARIC = new Item("Fly Agaric", Item.ItemType.Reagent, Item.Rarity.Uncommon, "fly_agaric", Colors.LightPink);
    public static Item ORPIMENT = new Item("Orpiment", Item.ItemType.Reagent, Item.Rarity.Rare, "orpiment", Colors.Orange);
    public static Item HOLLY_BERRIES = new Item("Holly Berries", Item.ItemType.Reagent, Item.Rarity.Common, "holly_berries", Colors.IndianRed);
    public static Item BRIMSTONE = new Item("Brimstone", Item.ItemType.Reagent, Item.Rarity.Common, "brimstone", Colors.LightGoldenrod);
    public static Item AQUA_FORTIS = new Item("Aqua Fortis", Item.ItemType.Solvent, Item.Rarity.Uncommon, "aqua_fortis", Colors.PaleGoldenrod);
    public static Item AQUA_VITAE = new Item("Aqua Vitae", Item.ItemType.Solvent, Item.Rarity.Common, "aqua_vitae", Colors.Snow);
    public static Item ELDERBERRIES = new Item("Elderberries", Item.ItemType.Reagent, Item.Rarity.Common, "elderberries", Colors.RebeccaPurple);

}
