﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MegaPunch : Skill {

	public MegaPunch(){
		name = "Mega Punch";
		selfTar = false;
		selfDam = false;
		type = ElementalType.None;
		APMult = 10;
		CDduration = 0;
	}

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