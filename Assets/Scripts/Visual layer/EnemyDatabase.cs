using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyDatabase : MonoBehaviour {

	public static EnemyDatabase _instance;

	public List<EnemyInfo> eDatabase = new List<EnemyInfo> ();

	/// <summary>
	/// Makes it a singleton.
	/// </summary>
	void Awake(){
		if (_instance == null) {
			_instance = this;
		} else {
			Debug.Log ("There are 2 EnemyDatabases");
		}
	}

	/// <summary>
	/// Create the type of the enemy of.
	/// </summary>
	/// <returns>The enemy of type.</returns>
	/// <param name="t">T.</param>
	/// <param name="l">L.</param>
	public EnemyInfo CreateEnemyOfType(int t, int l){
		List<EnemyInfo> temp = new List<EnemyInfo> ();
		//Finds which enemies it can spawn.
		foreach (EnemyInfo item in eDatabase) {
			if (item.minLevel <= l && item.maxLevel >= l) {
				switch (t) {
				case 0:
					if (item.forest) {
						temp.Add (item);
					}
					break;
				case 1: 
					if (item.cave) {
						temp.Add (item);
					}
					break;
				case 2:
					if (item.dungeon) {
						temp.Add (item);
					}
					break;
				default:
					Debug.LogError (t + " is not a valid dungoenType according to enemy spawning!");
					break;
				}
			}
		}
		if (temp.Count == 0) {
			Debug.LogError ("There are no enemies that can spawn in dungeon type: "+t+" for level: "+l);
			return null;
		}

		//Spawns a random enemy, that can be spawned.
		int toSpawn = Random.Range (0, temp.Count);
		return temp [toSpawn];
	}
}