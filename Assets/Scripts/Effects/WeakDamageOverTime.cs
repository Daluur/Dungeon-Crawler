﻿using UnityEngine;
using System.Collections;

public class WeakDamageOverTime : Effect {

		//Skill type, self target or enemy target.

	Skill simpleSkill;
	int apWhenActivated;

	new int round = 3;

	public WeakDamageOverTime(Skill skill) {
		name = "Weak Damage Over Time";
		effectFromSkill = skill.name;
		simpleSkill = new SimpleSkill ("Damage Over Time", true, skill.type, skill.APMult * 0.15F, true);
	}

	public override void AddToSkill(Skill skill) {
		effectFromSkill = skill.name;
		simpleSkill = new SimpleSkill ("Damage Over Time", true, skill.type, skill.APMult * 0.15F, true);
	}

	public override void ActivateEffect(Player player, Enemy enemy, PCNPC whoUsedIt) {
		if (whoUsedIt == PCNPC.NPC) {
			player.AddEffect (this);
			apWhenActivated = enemy.AP;
		} else {
			enemy.AddEffect (this);
			apWhenActivated = player.AP;
		}
	}

	public override void DoStuff(Player player, Enemy enemy, PCNPC whoUsedIt) {
		if (whoUsedIt == PCNPC.NPC) {
			enemy.UseEffect (simpleSkill, apWhenActivated);
		} else {
			player.UseEffect (simpleSkill, apWhenActivated);
		}
		round--;
	}

	public override void DeactivateEffect(Player player, Enemy enemy, PCNPC whoUsedIt) {
		if (whoUsedIt == PCNPC.NPC) {
			enemy.RemoveEffect (this);
		} else {
			player.RemoveEffect (this);
		}
		round = 3;
	}

	public override void ResetEffect (Player player, Enemy enemy, PCNPC whoUsedIt) {
		if (whoUsedIt == PCNPC.NPC) {
			apWhenActivated = enemy.AP;
		} else {
			apWhenActivated = player.AP;
		}
		round = 3;
	}

	public override bool IsOver () {
		if (round > 0) {
			return false;
		}
		return true;
	}	 
}
