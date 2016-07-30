using UnityEngine;
using System.Collections;

public class WeakHealingOverTime : Effect {

	//Skill type, self target or enemy target.
	Skill simpleSkill;
	int apWhenActivated;

	new int round = 3;

	public WeakHealingOverTime(Skill skill) {
		name = "Weak Healing Over Time";
		effectFromSkill = skill.name;
		simpleSkill = new SimpleSkill ("Healing Over Time", true, skill.type, skill.APMult * 0.15F, false);
	}

	public override void AddToSkill(Skill skill) {
		effectFromSkill = skill.name;
		simpleSkill = new SimpleSkill ("Healing Over Time", true, skill.type, skill.APMult * 0.15F, false);
	}

	public override void ActivateEffect(Player player, Enemy enemy, PCNPC whoUsedIt) {
		if (whoUsedIt == PCNPC.NPC) {
			enemy.AddEffect (this);
			apWhenActivated = enemy.AP;
		} else {
			player.AddEffect (this);
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
		round = 3;
	}

	public override bool IsOver () {
		if (round > 0) {
			return false;
		}
		return true;
	}
}
