using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FlockOfCows : Skill {

	public FlockOfCows(){
		name = "Flock of Cows";
		selfTar = false;
		selfDam = false;
		type = ElementalType.None;
		APMult = 10;
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