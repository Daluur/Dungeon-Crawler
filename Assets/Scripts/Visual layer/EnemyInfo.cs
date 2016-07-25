using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class EnemyInfo {

	//The prefab for the visual of this enemy.
	public GameObject visual;

	//The enemy type what it is weak/strong against.
	public ElementalType eType;

	//The enemies name.
	public string name;

	//The level range this enemy can spawn within.
	public int minLevel;
	public int maxLevel;
}