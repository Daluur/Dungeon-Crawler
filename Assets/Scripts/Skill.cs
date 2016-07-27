using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Skill{

	//Skill name
	public string name;
	//Skill type, self target or enemy target.
	public bool selfTar;
	public bool selfDam;
	//SKill elemental type
	public ElementalType type;
	//Skill damage, as AP multiplier
	public float APMult;
	//Skill Cooldown length
	public int CD;
	//Skills current CD
	public int CDduration = 0;

	public Effect mainEffect;
	public List<Effect> effects = new List<Effect> ();
		
	/// <summary>
	/// Creates a new Skill
	/// </summary>
	/// <param name="newName">New Name.</param>
	/// <param name="newSelfTar">NEw Self Target.</param>
	/// <param name="newType">New Type.</param>
	/// <param name="newAPMult">New AP Multiplier.</param>
	/// <param name="newCD">New Cooldown.</param>
	public Skill(string newName, bool newSelfTar, ElementalType newType, float newAPMult, int newCD, bool newSelfDam = false){
		name = newName;
		selfTar = newSelfTar;
		selfDam = newSelfDam;
		type = newType;
		APMult = newAPMult;
		CD = newCD;
	}

	/// <summary>
	/// Calculates dmg and creates DamagePackage.
	/// </summary>
	public DamagePackage CalDmg(int AP, float critChance){
		System.Random rnd = new System.Random();
		if (rnd.Next (0, 100) < critChance) {
			return new DamagePackage (type, (AP * APMult) * 2);
		}
		return new DamagePackage (type, AP * APMult);
	}

	public bool isSelfTarget(){
		return selfTar;
	}

	public bool isSelfDamage(){
		return selfDam;
	}

	/// <summary>
	/// Set Cooldown.
	/// </summary>
	public void setCD() {
		CDduration = CD;
	}

	/// <summary>
	/// Update Cooldown.
	/// </summary>
	public void updateCD() {
		CDduration--;
	}

	/// <summary>
	/// Check if Skill is on Cooldown.
	/// </summary>
	/// <returns><c>true</c>, if on cooldown, <c>false</c> otherwise.</returns>
	public bool onCD(){
		if (CDduration > 0) {
			return true;
		}

		return false;
	}

}