﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Fireball : Skill {

	public Fireball(){
		name = "Fireball";
		selfTar = false;
		selfDam = false;
		type = ElementalType.Fire;
		APMult = 8;
		CDduration = 2;
		effects.Add(new WeakDamageOverTime (this));
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