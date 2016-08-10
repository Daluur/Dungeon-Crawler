using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad {

	// Player
	public static void SavePlayer(PlayerData data) {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerData.dat");
		bf.Serialize (file, data);
		file.Close ();
	}

	public static PlayerData LoadPlayer() {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Open (Application.persistentDataPath + "/playerData.dat", FileMode.Open);
		PlayerData data = (PlayerData)bf.Deserialize (file);
		file.Close ();

		return data;
	}

	public static bool PlayerExist() {
		return File.Exists (Application.persistentDataPath + "/PlayerData.dat");
	}


	// Shop
	public static void SaveShop(List<int> data) {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/ShopData.dat");
		bf.Serialize (file, data);
		file.Close ();
	}

	public static List<int> LoadShop() {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Open (Application.persistentDataPath + "/ShopData.dat", FileMode.Open);
		List<int> data = (List<int>)bf.Deserialize (file);
		file.Close ();

		return data;
	}

	public static bool ShopExist() {
		return File.Exists (Application.persistentDataPath + "/ShopData.dat");
	}

}
	