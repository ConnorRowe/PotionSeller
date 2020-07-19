using Godot;
using System;

public class Items : Reference
{
    // Items
    public static Item FLY_AGARIC = new Item("Fly Agaric", Item.ItemType.Reagent, Item.Rarity.Uncommon, "fly_agaric", Colors.LightPink, "A very distinctive mushroom; its bright red cap with white warts inspired early alchemists to brew potions inciting a magical strength.");
    public static Item ORPIMENT = new Item("Orpiment", Item.ItemType.Reagent, Item.Rarity.Rare, "orpiment", Colors.Orange, "Coveted for its rich hues when used as a pigment, only alchemists know its best use - as a poison.");
    public static Item HOLLY_BERRIES = new Item("Holly Berries", Item.ItemType.Reagent, Item.Rarity.Common, "holly_berries", Colors.IndianRed, "Somewhat toxic, yet when brewed properly, restoritive properties are revealed.");
    public static Item BRIMSTONE = new Item("Brimstone", Item.ItemType.Reagent, Item.Rarity.Common, "brimstone", Colors.LightGoldenrod, "With a potent aroma and hellish connotations, people fear it. Despite this, it has strong healing and empowering properties.");
    public static Item AQUA_FORTIS = new Item("Aqua Fortis", Item.ItemType.Solvent, Item.Rarity.Rare, "aqua_fortis", Colors.PaleGoldenrod, "A strong, rare acid. It reacts violently even with the air and forms thick fumes of dark orange.");
    public static Item AQUA_VITAE = new Item("Aqua Vitae", Item.ItemType.Solvent, Item.Rarity.Common, "aqua_vitae", Colors.Snow, "Also known as 'Spirit of Wine', it is a common solvent distilled from booze.");
    public static Item ELDERBERRIES = new Item("Elderberries", Item.ItemType.Reagent, Item.Rarity.Common, "elderberries", Colors.Indigo, "A berry found throughout forests all over the isles. Commonly known to help one see as a cat does.");

}
