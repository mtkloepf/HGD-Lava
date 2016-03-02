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
	public static readonly Sprite fogSprite;
	public static readonly Sprite blueFogSprite;

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
		fogSprite = Resources.Load<Sprite>("Sprites/Terrain_Tiles/fog_tile");
		blueFogSprite = Resources.Load<Sprite>("Sprites/Terrain_Tiles/fog_tile_blue");

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

	/* Returns a card sprite based on the type given. */
	public static Sprite card_by_type(CardScript.CardType type) {
		switch (type) {
			case CardScript.CardType.HumanInfantry:		return human_infantry_card;
			case CardScript.CardType.HumanTank:			return human_tank_card;
			case CardScript.CardType.HumanExo:			return human_exo_card;
			case CardScript.CardType.AlienInfantry:		return alien_infantry_card;
			case CardScript.CardType.AlienTank:			return alien_tank_card;
			case CardScript.CardType.AlienElite:		return alien_elite_card;
			case CardScript.CardType.Currency1:			return bronze_card;
			case CardScript.CardType.Currency2:			return silver_card;
			case CardScript.CardType.Currency3:			return gold_card;
			default:									return empty_card;
		}
	}

	/* Returns the standing Sprite for the given Unit type from the folder New Units/ */
	public static Sprite get_unit_sprite(UnitScript.Types type) {
		switch (type) {
			case UnitScript.Types.H_Infantry:		return Resources.Load<Sprite>("New Units/Human Infantry/Human_Infantry_Stand");
			case UnitScript.Types.H_Exo:			return Resources.Load<Sprite>("New Units/Human Exosuit/Exo_Stand");
			case UnitScript.Types.H_Tank:			return Resources.Load<Sprite>("New Units/Human Tank/Human_Tank_Stand");
			case UnitScript.Types.H_Artillery:		return Resources.Load<Sprite>("New Units/Human Artillery/Human_Artillary_Stand");
			case UnitScript.Types.H_Base:			return Resources.Load<Sprite>("New Units/Human HQ/Human_HQ_Stand");
			case UnitScript.Types.A_Infantry:		return Resources.Load<Sprite>("New Units/Alien Infatry/Alien_Infantry_Stand");
			case UnitScript.Types.A_Elite:			return Resources.Load<Sprite>("New Units/Alien Elite/Alien_Elite_Stand");
			case UnitScript.Types.A_Tank:			return Resources.Load<Sprite>("New Units/Aliem Tank/Alien_Tank_Stand");
			case UnitScript.Types.A_Artillery:		return Resources.Load<Sprite>("New Units/Alien Artillery/Alien_Artillary_Stand");
			case UnitScript.Types.A_Base:			return Resources.Load<Sprite>("New Units/Alien Mothership/Alien_Mothership_Stand");
			default:						return null;
		}
	}

	/* Returns the Controller for the given Unit type found in the folder: Resources/Animations/Controllers/ */
	public static RuntimeAnimatorController get_controller(UnitScript.Types type) {
		switch (type) {
			case UnitScript.Types.H_Infantry:		return Resources.Load<RuntimeAnimatorController>("Animations/Controllers/Human_Infantry_Controller");
			case UnitScript.Types.H_Exo:			return Resources.Load<RuntimeAnimatorController>("Animations/Controllers/Human_Exo");
			case UnitScript.Types.H_Tank:			return Resources.Load<RuntimeAnimatorController>("Animations/Controllers/Human_Tank_Controller");
			case UnitScript.Types.H_Artillery:		return Resources.Load<RuntimeAnimatorController>("Animations/Controllers/Human_Artillery");
			case UnitScript.Types.H_Base:			return Resources.Load<RuntimeAnimatorController>("Animations/Controllers/Human_Mobile_Base");
			case UnitScript.Types.A_Infantry:		return Resources.Load<RuntimeAnimatorController>("Animations/Controllers/Alien_Infantry_Controller");
			case UnitScript.Types.A_Elite:			return Resources.Load<RuntimeAnimatorController>("Animations/Controllers/Alien_Elite_Controller");
			case UnitScript.Types.A_Tank:			return Resources.Load<RuntimeAnimatorController>("Animations/Controllers/Alien_Tank_Controller");
			case UnitScript.Types.A_Artillery:		return Resources.Load<RuntimeAnimatorController>("Animations/Controllers/Alien_Artillery_Controller");
			case UnitScript.Types.A_Base:			return Resources.Load<RuntimeAnimatorController>("Animations/Controllers/Alien_Mobile_Base"); 
			default:						return null;
		}
	}
}
