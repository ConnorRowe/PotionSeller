extends Reference

func post_import(scene):
	for layer in scene.get_children():
		var layer_tile_set = layer.get_tileset()

		print("tiles_ids length = ", layer_tile_set.get_tiles_ids().size())
		
		var i = 1
		for tile in layer_tile_set.get_tiles_ids():
			var loadedTex = load("res://textures/terrain_normal/terrain_" + String(i) + "_n.png")
			layer_tile_set.tile_set_normal_map(tile,loadedTex)
			i += 1

	return scene
