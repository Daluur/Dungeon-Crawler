using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(EnemyDatabase))]
public class EnemyDatabaseEditor : Editor {

	EnemyInfo newEnemy = null;
	EnemyDatabase EDB;
	bool seeEnemies = false;
	bool editEnemy = false;
	int currentlyViewing = 0;

	public override void OnInspectorGUI(){
		EDB = target as EnemyDatabase;

		if (newEnemy != null) {
			AddingEnemy ();
			return;
		}
		if (seeEnemies) {
			SeeEnemies ();
			return;
		}

		if (EDB.eDatabase.Count == 0) {
			if (GUILayout.Button ("Add first enemy")) {
				AddEnemy ();
			}
		} else {
			EditorGUILayout.HelpBox ("There are currently " + EDB.eDatabase.Count + " Enemies in the database", MessageType.Info);
			if (GUILayout.Button ("Add new enemy")) {
				AddEnemy ();
			}
			if (GUILayout.Button ("See enemies")) {
				seeEnemies = true;
			}
		}
	}

	void AddEnemy(){
		newEnemy = new EnemyInfo ();
	}

	void AddingEnemy(){
		EditorGUILayout.HelpBox ("Adding a new Enemy", MessageType.Info);
		newEnemy.name = EditorGUILayout.TextField ("Enemy name", newEnemy.name);
		newEnemy.visual = (GameObject)EditorGUILayout.ObjectField ("Enemy Visual", newEnemy.visual, typeof(GameObject), false);
		newEnemy.eType = (ElementalType)EditorGUILayout.EnumPopup ("Enemy type", newEnemy.eType);
		newEnemy.minLevel = EditorGUILayout.IntField ("Enemy min level", newEnemy.minLevel);
		newEnemy.maxLevel = EditorGUILayout.IntField ("Enemy max level", newEnemy.maxLevel);
		if (GUILayout.Button ("Finish!")) {
			if (CheckEnemy (newEnemy)) {
				EDB.eDatabase.Add (newEnemy);
				newEnemy = null;
			}
		}
		if (GUILayout.Button ("Cancel!")) {
			newEnemy = null;
		}
	}

	bool CheckEnemy(EnemyInfo EI){
		if (EI.name == "" || EI.name == null) {
			Debug.LogError ("Invalid name");
			return false;
		}
		if (EI.visual == null) {
			Debug.LogError ("Invalid Visual");
			return false;
		}
		if (EI.maxLevel < EI.minLevel) {
			Debug.LogError ("Max level less than min");
			return false;
		}
		if (EI.minLevel < 0) {
			Debug.LogError ("Min level less than 0");
			return false;
		}
		return true;
	}

	void SeeEnemies(){
		if (editEnemy) {
			EditorGUILayout.HelpBox ("Editing enemy"+(currentlyViewing+1)+"/"+EDB.eDatabase.Count,MessageType.Info);
			EditEnemy (EDB.eDatabase [currentlyViewing]);
		} else {
			EditorGUILayout.HelpBox ("Viewing enemy"+(currentlyViewing+1)+"/"+EDB.eDatabase.Count,MessageType.Info);
			ViewEnemy (EDB.eDatabase [currentlyViewing]);

			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Prev")) {
				currentlyViewing--;
				if (currentlyViewing < 0) {
					currentlyViewing = EDB.eDatabase.Count - 1;
				}
			}
			if (GUILayout.Button ("Next")) {
				currentlyViewing++;
				if (currentlyViewing > EDB.eDatabase.Count - 1) {
					currentlyViewing = 0;
				}
			}
			EditorGUILayout.EndHorizontal ();
			if (GUILayout.Button ("Edit")) {
				editEnemy = true;
			}
			if (GUILayout.Button ("Back")) {
				seeEnemies = false;
			}
		}
	}

	void ViewEnemy(EnemyInfo EI){
		EditorGUILayout.LabelField ("Enemy name", EI.name);
		EditorGUI.BeginDisabledGroup (true);
		EditorGUILayout.ObjectField ("Enemy visual",EI.visual, typeof(GameObject), false);
		EditorGUI.EndDisabledGroup ();
		EditorGUILayout.LabelField ("Enemy type", EI.eType.ToString ());
		EditorGUILayout.LabelField ("Enemy min level", EI.minLevel.ToString());
		EditorGUILayout.LabelField ("Enemy max level", EI.maxLevel.ToString());
	}

	void EditEnemy(EnemyInfo EI){
		EI.name = EditorGUILayout.TextField ("Enemy name", EI.name);
		EI.visual = (GameObject)EditorGUILayout.ObjectField ("Enemy Visual", EI.visual, typeof(GameObject), false);
		EI.eType = (ElementalType)EditorGUILayout.EnumPopup ("Enemy type", EI.eType);
		EI.minLevel = EditorGUILayout.IntField ("Enemy min level", EI.minLevel);
		EI.maxLevel = EditorGUILayout.IntField ("Enemy max level", EI.maxLevel);
		if (GUILayout.Button ("Delete!")) {
			if (EditorUtility.DisplayDialog ("Delete " + EI.name, "Are you sure you want to delete this enemy?", "Yes", "No")) {
				EDB.eDatabase.RemoveAt (currentlyViewing);
				if (currentlyViewing > EDB.eDatabase.Count - 1) {
					currentlyViewing = EDB.eDatabase.Count - 1;
				}
				editEnemy = false;
				if (currentlyViewing == -1) {
					currentlyViewing = 0;
					seeEnemies = false;
				}
			}
		}
		if (GUILayout.Button ("Finish!")) {
			if (CheckEnemy (EI)) {
				editEnemy = false;
			}
		}
	}
}
