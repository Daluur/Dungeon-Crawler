using UnityEngine;
using System.Collections;

public class City : MonoBehaviour {

	Dungeon testDungeon;

	// Use this for initialization
	void Start () {
		Debug.Log ("started a new dungeon");
		testDungeon = new Dungeon (0,5,1);
	}
}