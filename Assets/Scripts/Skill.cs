using UnityEngine;
using System.Collections;
using System;

[Serializable]

public class Skill{

	//Skill name
	public string name;
	//Skill type, self target or enemy target.
	public bool selfTar;
	//SKill elemental type
	public int type;
	//Skill damage, as AP multiplier
	public int APMult;
	//Skill Cooldown length
	public int CD;
	//Skills current CD
	public int CDduration = 0;
	//Has over time effect
	public bool hasOT;
	//Number of rounds the effect lasts
	public int rounds;
	//The damage, as AP multiplier per round
	public int APMultPerRound;


	/// <summary>
	/// Creates a new Skill
	/// </summary>
	/// <param name="newName">New Name.</param>
	/// <param name="newSelfTar">NEw Self Target.</param>
	/// <param name="newType">New Type.</param>
	/// <param name="newAPMult">New AP Multiplier.</param>
	/// <param name="newCD">New Cooldown.</param>
	public Skill(string newName, bool newSelfTar, int newType, int newAPMult, int newCD){
		name = newName;
		selfTar = newSelfTar;
		type = newType;
		APMult = newAPMult;
		CD = newCD;
		hasOT = false;
	}

	public Skill(string newName, bool newSelfTar, int newType, int newAPMult, int newCD, int newRounds, int newAPMultPerRound){
		name = newName;
		selfTar = newSelfTar;
		type = newType;
		APMult = newAPMult;
		CD = newCD;
		hasOT = true;
		rounds = newRounds;
		APMultPerRound = newAPMultPerRound;
	}

	/// <summary>
	/// Calculates dmg and creates DamagePackage.
	/// </summary>
	public DamagePackage CalDmg(int AP){
		if (hasOT) {
			return new DamagePackage (type, AP * APMult, rounds, AP * APMultPerRound);
		}

		return new DamagePackage (type, AP * APMult);
	}

	public bool isSelfTarget(){
		return selfTar;
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
