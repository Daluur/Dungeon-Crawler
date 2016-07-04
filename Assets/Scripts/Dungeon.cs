using UnityEngine;
using System.Collections;

public class Dungeon {

	//Which type, e.g. Dungeon, cave, forest w/e
	int dungeonType;
	//How many enemies.
	int length;
	//Level you should be.
	int level;
	//which enemy out of the length is the current.
	int atIndex = 0;

	/// <summary>
	/// Initializes a new instance of the <see cref="Dungeon"/> class.
	/// </summary>
	/// <param name="newType">New type.</param>
	/// <param name="newLengh">New lengh.</param>
	/// <param name="newLevel">New level.</param>
	public Dungeon(int newType, int newLengh, int newLevel){
		dungeonType = newType;
		length = newLengh;
		level = newLevel;
		//Changes the background color based on which dungeon type it is.
		if (dungeonType == 0) {
			VisualController._instance.ChangebackgroundColor (Color.green);
		}
		else if (dungeonType == 1) {
			VisualController._instance.ChangebackgroundColor (Color.gray);
		}
		else if (dungeonType == 2) {
			VisualController._instance.ChangebackgroundColor (Color.black);
		}

		//Tells the combat controller which dungeon we are in.
		CombatController._instance.NewDungeon (this);
		//Starts the next encounter.
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
		Debug.Log ("Created new enemy, enemy number: "+atIndex);
		EnemyInfo temp = EnemyDatabase._instance.CreateEnemyOfType (dungeonType, level);
		if (temp == null) {
			Debug.LogError ("Something bad happened! returned null from enemy spawn!");
			return;
		}
		//Creates the enemy visual.
		VisualController._instance.CreateEnemyVisual (temp);
		//Creates the actual enemy.
		CombatController._instance.NewEnemy (new Enemy (temp.type, level, atIndex));
	}
}
