using UnityEngine;

/**
 * A class designed to hold any and all prefabs used during runtime.
 * 
 * @author Joshua Hooker
 * 22 February 2016
 */
public static class PrefabManager {
	public static GameObject TilePrefab;

	public static readonly GameObject HumanMobileBasePrefab;
	public static readonly GameObject HumanInfantryPrefab;
	public static readonly GameObject HumanExoPrefab;
	public static readonly GameObject HumanTankPrefab;
	public static readonly GameObject HumanArtilleryPrefab;

	public static readonly GameObject AlienInfantryPrefab;
	public static readonly GameObject AlienElitePrefab;
	public static readonly GameObject AlienTankPrefab;
	public static readonly GameObject AlienArtilleryPrefab;
	public static readonly GameObject AlienMobileBasePrefab;

	public static readonly GameObject CardPrefab;
	/* Initiaze all the prefabs */
	static PrefabManager () {
		TilePrefab = (GameObject)Resources.Load("Prefabs/Hex");

		HumanMobileBasePrefab = (GameObject)Resources.Load("Prefabs/Human_Mobile_Base");
		HumanInfantryPrefab = (GameObject)Resources.Load("Prefabs/Human_Infantry");
		HumanTankPrefab = (GameObject)Resources.Load("Prefabs/Human_Tank");
		HumanExoPrefab = (GameObject)Resources.Load("Prefabs/Human_Exo");
		HumanArtilleryPrefab = (GameObject)Resources.Load("Prefabs/Human_Artillery");

		AlienMobileBasePrefab = (GameObject)Resources.Load("Prefabs/Alien_Mobile_Base");
		AlienInfantryPrefab = (GameObject)Resources.Load("Prefabs/Alien_Infantry");
		AlienTankPrefab = (GameObject)Resources.Load("Prefabs/Alien_Tank");
		AlienElitePrefab = (GameObject)Resources.Load("Prefabs/Alien_Elite");
		AlienArtilleryPrefab = (GameObject)Resources.Load("Prefabs/Alien_Artillery");

		CardPrefab = (GameObject)Resources.Load("Prefabs/Card");
	}
}
