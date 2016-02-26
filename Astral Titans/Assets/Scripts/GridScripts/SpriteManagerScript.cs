using UnityEngine;
using System.Collections;

/**
 * A class designed to house all sprites used at runtime: namely nex tiles and cards.
 * 
 */
public static class SpriteManagerScript {

    // Hex Sprites
	public static readonly Sprite desertSprite;
	public static readonly Sprite redDesertSprite;
	public static readonly Sprite blueDesertSprite;
	public static readonly Sprite plainsSprite;
	public static readonly Sprite redPlainsSprite;
	public static readonly Sprite bluePlainsSprite;
	public static readonly Sprite waterSprite;
	public static readonly Sprite redWaterSprite;
	public static readonly Sprite blueWaterSprite;
	public static readonly Sprite mountainSprite;
	public static readonly Sprite redMountainSprite;
	public static readonly Sprite blueMountainSprite;
    // Card Sprites
	public static readonly Sprite empty_card;
	public static readonly Sprite bronze_card;
	public static readonly Sprite silver_card;
	public static readonly Sprite gold_card;

	public static readonly Sprite human_infantry_card;
	public static readonly Sprite human_tank_card;
	public static readonly Sprite human_exo_card;
	public static readonly Sprite human_artillery_card;

	public static readonly Sprite alien_infantry_card;
	public static readonly Sprite alien_tank_card;
	public static readonly Sprite alien_elite_card;
	public static readonly Sprite alien_artillery_card;

	static SpriteManagerScript() {
		// Load sprites from folder: Assets/Resoures/Terrain_Tiles
		desertSprite = Resources.Load<Sprite>("Sprites/Terrain_Tiles/desert_tile");
		redDesertSprite = Resources.Load<Sprite>("Sprites/Terrain_Tiles/desert_tile_red");
		blueDesertSprite = Resources.Load<Sprite>("Sprites/Terrain_Tiles/desert_tile_blue");
		plainsSprite = Resources.Load<Sprite>("Sprites/Terrain_Tiles/grass_tile");
		redPlainsSprite = Resources.Load<Sprite>("Sprites/Terrain_Tiles/grass_tile_red");
		bluePlainsSprite = Resources.Load<Sprite>("Sprites/Terrain_Tiles/grass_tile_blue");
		waterSprite = Resources.Load<Sprite>("Sprites/Terrain_Tiles/water_tile");
		redWaterSprite = Resources.Load<Sprite>("Sprites/Terrain_Tiles/water_tile_red");
		blueWaterSprite = Resources.Load<Sprite>("Sprites/Terrain_Tiles/water_tile_blue");
		mountainSprite = Resources.Load<Sprite>("Sprites/Terrain_Tiles/volcano_tile");
		redMountainSprite = Resources.Load<Sprite>("Sprites/Terrain_Tiles/volcano_tile_red");
		blueMountainSprite = Resources.Load<Sprite>("Sprites/Terrain_Tiles/volcano_tile_blue");

		// Load sprites from folder: Assets/Resources/Cards
		empty_card = Resources.Load<Sprite>("Sprites/Cards/Human_Card");
		bronze_card = Resources.Load<Sprite>("Sprites/Cards/Bronze");
		silver_card = Resources.Load<Sprite>("Sprites/Cards/Silver");
		gold_card = Resources.Load<Sprite>("Sprites/Cards/Gold");

		human_infantry_card = Resources.Load<Sprite>("Sprites/Cards/Human_Infantry");
		human_tank_card = Resources.Load<Sprite>("Sprites/Cards/Human_Tank");
		human_exo_card = Resources.Load<Sprite>("Sprites/Cards/Human_Exosuit");
		human_artillery_card = null; // does not exist yet!

		alien_infantry_card = Resources.Load<Sprite>("Sprites/Cards/Alien_Infantry");
		alien_tank_card = Resources.Load<Sprite>("Sprites/Cards/Alien_Tank");
		alien_elite_card = Resources.Load<Sprite>("Sprites/Cards/Alien_Elite");
		alien_artillery_card = null; // does not exist yet!
	}
}
