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
		CDduration = 0;
	}

	public override DamagePackage CalDmg(int AP, float critChance){
		System.Random rnd = new System.Random();
		if (rnd.Next (0, 100) < critChance) {
			return new DamagePackage (type, (AP * APMult) * 2, name, true);
		}
		return new DamagePackage (type, AP * APMult, name, false);
	}

	public override void AddEffect(Effect newEffect) {
		newEffect.AddToSkill(this);
		effects.Add(newEffect);
	}

	public override void ActivateCD() {
		CD = CDduration;
	}

	public override void UpdateCD() {
		CD--;
	}

	public override bool IsOnCD(){
		if (CD > 0) {
			return true;
		}

		return false;
	}

}