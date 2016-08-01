using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class HolyHand : Skill {

	public HolyHand(){
		name = "Holy Hand";
		selfTar = true;
		selfDam = false;
		type = ElementalType.None;
		APMult = 6;
		CD = 0;
		CDduration = 2;
		effects.Add (new WeakDamageOverTime (this));
		effects.Add (new ReduceDamageTaken (this));
	}

	public override DamagePackage CalDmg(int AP, float critChance){
		System.Random rnd = new System.Random();
		if (rnd.Next (0, 100) < 100) {
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