using UnityEngine;
using System.Collections;

public class Dungeon {

	//Which type, e.g. fire, water, desert w/e
	int type;
	//How many enemies.
	int length;
	//Level you should be.
	int level;
	//which enemy out of the length is the current.
	int atIndex = -1;

	/// <summary>
	/// Initializes a new instance of the <see cref="Dungeon"/> class.
	/// </summary>
	/// <param name="newType">New type.</param>
	/// <param name="newLengh">New lengh.</param>
	/// <param name="newLevel">New level.</param>
	public Dungeon(int newType, int newLengh, int newLevel){
		type = newType;
		length = newLengh;
		level = newLevel;
		CombatController._instance.NewDungeon (this);
		NextEncounter ();
	}

	/// <summary>
	/// Starts the next  encounter.
	/// </summary>
	public void NextEncounter(){
		atIndex++;
		if (atIndex == length) {
			Debug.Log ("Dungeon is finished");
			return;
		}
		//Creates a new enemy
		//TODO: get the enemy from some sort of database.
		Debug.Log ("Created new enemy, enemy number: "+atIndex);
		CombatController._instance.NewEnemy (new Enemy (type, level, atIndex));
		//Create the mob visually.
	}
}
