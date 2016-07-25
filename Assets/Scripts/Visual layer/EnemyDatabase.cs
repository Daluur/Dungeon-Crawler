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
	/// <param name="t">Type</param>
	/// <param name="l">Level</param>
	public EnemyInfo CreateEnemyOfType(ElementalType t, int l){
		List<EnemyInfo> temp = new List<EnemyInfo> ();
		foreach (EnemyInfo item in eDatabase) {
			if (item.minLevel <= l && item.maxLevel >= l) {
				if (item.eType == t || item.eType == ElementalType.None) {
					temp.Add (item);
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