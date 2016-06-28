using UnityEngine;
using System.Collections;

public class Dungeon {

	int type;
	int length;
	int level;
	int atIndex = 0;

	public Dungeon(int newType, int newLengh, int newLevel){
		type = newType;
		length = newLengh;
		level = newLevel;
		CombatController._instance.NewDungeon (this);
		NextEncounter ();
	}

	public void NextEncounter(){
		atIndex++;
		if (atIndex == length) {
			Debug.Log ("Dungeon is finished");
			return;
		}
		Debug.Log ("Created new enemy, enemy number: "+atIndex);
		CombatController._instance.NewEnemy (new Enemy (type, level, atIndex));
		//Create the mob visually.
	}
}
