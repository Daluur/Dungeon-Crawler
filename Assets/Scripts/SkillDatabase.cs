using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillDatabase : MonoBehaviour {

	public static SkillDatabase _instance;

	public List<Skill> sDatabase = new List<Skill>();

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
}
