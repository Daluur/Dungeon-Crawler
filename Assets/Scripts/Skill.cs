using UnityEngine;
using System.Collections;

public class Skill{

	//Skill name
	public string name;
	//SKill type
	int type;
	//Skill damage, as AP multiplier
	int APMult;
	//Skill Cooldown length
	int CD;
	//Skills current CD
	int CDduration = 0;

	/// <summary>
	/// Creates a new Skill
	/// </summary>
	/// <param name="newName">New Name.</param>
	/// <param name="newType">New Type.</param>
	/// <param name="newAPMult">New AP Multiplier.</param>
	/// <param name="newCD">New Cooldown.</param>
	public Skill(string newName, int newType, int newAPMult, int newCD){
		name = newName;
		type = newType;
		APMult = newAPMult;
		CD = newCD;
	}

	/// <summary>
	/// Calculates dmg and creates DamagePackage.
	/// </summary>
	public DamagePackage CalDmg(int AP){
		return new DamagePackage (type, AP * APMult);
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
