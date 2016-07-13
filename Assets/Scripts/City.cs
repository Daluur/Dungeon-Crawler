using UnityEngine;
using System.Collections;

public class City : MonoBehaviour {

	Dungeon testDungeon;

	// Use this for initialization
	void Start () {
		//has to be remade when there are more dungeons.
		EnterTestDungeon ();
	}

	void EnterTestDungeon(){
		Debug.Log ("started a new dungeon");
		testDungeon = new Dungeon (1,5,1);
	}
}