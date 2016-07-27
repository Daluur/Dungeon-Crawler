using UnityEngine;
using System.Collections;

public class WeakHealingOverTime : Effect {

	//Skill type, self target or enemy target.
	Skill simpleSkill;
	int apWhenActivated;

	public WeakHealingOverTime(Skill skill) {
		selfTar = skill.selfTar;
		simpleSkill = new SimpleSkill ("Healing Over Time", true, skill.type, skill.APMult * 0.15F, false);
	}

	public override void AddToSkill(Skill skill) {
		selfTar = skill.selfTar;
		simpleSkill = new SimpleSkill ("Healing Over Time", true, skill.type, skill.APMult * 0.15F, false);
	}

	public override void ActivateEffect(Player player, Enemy enemy, PCNPC whoUsedIt) {
		if (enemy == null) {
			apWhenActivated = player.AP;
			player.effects.Add (this);
		} else {
			apWhenActivated = enemy.AP;
			enemy.effects.Add (this);
		}
	}

		public override void DoStuff(Player player, Enemy enemy, PCNPC whoUsedIt) {
		if (enemy == null) {
			player.UseEffect (simpleSkill, apWhenActivated);
		} else {
			enemy.UseEffect (simpleSkill, apWhenActivated);
		}
		round--;
	}

		public override void DeactivateEffect(Player player, Enemy enemy, PCNPC whoUsedIt) {
		if (enemy == null) {
			player.effects.Remove (this);
		} else {
			enemy.effects.Remove (this);
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
