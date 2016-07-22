using UnityEngine;
using System.Collections;

public class Dungeon {

	//Which type, e.g. Fire, Water, Earth.
	ElementalType dungeonType;
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
	public Dungeon(ElementalType newType, int newLengh, int newLevel){
		dungeonType = newType;
		length = newLengh;
		level = newLevel;
		//Changes the background color based on which dungeon type it is.
		if (dungeonType == ElementalType.Earth) {
			VisualController._instance.ChangebackgroundColor (new Color(0.23f,0.55f,0.23f));
		}
		else if (dungeonType == ElementalType.Fire) {
			VisualController._instance.ChangebackgroundColor (new Color(0.55f,0.23f,0.23f));
		}
		else if (dungeonType == ElementalType.Water) {
			VisualController._instance.ChangebackgroundColor (new Color(0.23f,0.23f,0.55f));
		}

		//Tells the combat controller which dungeon we are in.
		CombatController._instance.NewDungeon (this);
		//Starts the first encounter.
		NextEncounter ();
	}

	/// <summary>
	/// Starts the next  encounter.
	/// </summary>
	public void NextEncounter(){
		atIndex++;
		if (atIndex == length) {
			Debug.Log ("Dungeon is finished");
			Debug.Log ("Should give end-of-dungeon loot now!");
			//Leaves the dungeon.
			GameStateManager._instance.LeaveDungeon ();
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
		CombatController._instance.NewEnemy (new Enemy (temp.eType, level, atIndex));
	}
}