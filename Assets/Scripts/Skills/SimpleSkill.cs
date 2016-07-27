using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SimpleSkill : Skill {

	public SimpleSkill(string newName, bool newSelfTar, ElementalType newType, float newAPMult, bool newSelfDam = false){
		name = newName;
		selfTar = newSelfTar;
		selfDam = newSelfDam;
		type = newType;
		APMult = newAPMult;
		CD = 0;
	}

	/// <summary>
	/// Calculates dmg and creates DamagePackage.
	/// </summary>
	public override DamagePackage CalDmg(int AP, float critChance){
		System.Random rnd = new System.Random();
		if (rnd.Next (0, 100) < critChance) {
			return new DamagePackage (type, (AP * APMult) * 2);
		}
		return new DamagePackage (type, AP * APMult);
	}

	public override void AddEffect(Effect newEffect) {
		newEffect.AddToSkill(this);
		effects.Add(newEffect);
	}

	/// <summary>
	/// Set Cooldown.
	/// </summary>
	public override void SetCD() {
		CD = CD;
	}

	/// <summary>
	/// Update Cooldown.
	/// </summary>
	public override void UpdateCD() {
		CD--;
	}

	/// <summary>
	/// Check if Skill is on Cooldown.
	/// </summary>
	/// <returns><c>true</c>, if on cooldown, <c>false</c> otherwise.</returns>
	public override bool OnCD(){
		if (CD > 0) {
			return true;
		}

		return false;
	}

}