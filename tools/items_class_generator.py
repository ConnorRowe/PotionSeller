import json
import os

with open(os.path.join(os.path.dirname(__file__), "items.cdb"), 'r') as f:
    data = json.load(f)

outputFileName = "generated_class.txt"
outputPath = os.path.join(os.path.dirname(__file__), outputFileName)

fileOut = open(outputPath, 'w')


def getItemType(index: int):
    ItemTypes = {
        0: "Reagent",
        1: "Solvent",
        2: "Finisher"
    }

    return "Item.ItemType." + ItemTypes.get(index)


def getItemRarity(index: int):
    Rarities = {
        0: "Common",
        1: "Uncommon",
        2: "Rare",
        3: "Legendary"
    }

    return "Item.Rarity." + Rarities.get(index)


def main():
    # write class declaration
    fileOut.write(
        "using Godot;\nusing System;\n\npublic class Items : Reference\n{\n\t// Items\n")
    itemList = data['sheets'][0]['lines']

    # write all the items as static members
    for item in itemList:
        fileOut.write("\tpublic static Item " + item['name'].upper().replace(' ', '_') + " = new Item(\"" +
                      item['name'] + "\", " + getItemType(item['type']) + ", " + getItemRarity(item['rarity']) + ", \"" + item['name'].lower().replace(' ', '_') +
                      "\", " + "Colors." + item['potion_colour'] + ", \"" + item['description'] + "\");\n")

    f.close()

    # close class
    fileOut.write("\n}\n")

    # close the file and open it up in an external text editor
    fileOut.close()
    os.startfile(outputPath)


if __name__ == "__main__":
    main()
