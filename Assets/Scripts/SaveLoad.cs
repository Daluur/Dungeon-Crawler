using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad {

	public static void Save(PlayerData data) {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerData.dat");
		bf.Serialize (file, data);
		file.Close ();
	}

	public static PlayerData Load() {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Open (Application.persistentDataPath + "/playerData.dat", FileMode.Open);
		PlayerData data = (PlayerData)bf.Deserialize (file);
		file.Close ();

		return data;

	}

	public static bool SaveExist() {
		return File.Exists (Application.persistentDataPath + "/PlayerData.dat");

	}
}