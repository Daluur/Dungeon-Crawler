using UnityEngine;
using System.Collections;

public class WeakDamageOverTime : Effect {

		//Skill type, self target or enemy target.
	Skill simpleSkill;
	int apWhenActivated;

	public WeakDamageOverTime(Skill skill) {
		selfTar = skill.selfTar;
		simpleSkill = new SimpleSkill ("Damage Over Time", true, skill.type, skill.APMult * 0.15F, true);
	}

	public override void AddToSkill(Skill skill) {
		selfTar = skill.selfTar;
		simpleSkill = new SimpleSkill ("Damage Over Time", true, skill.type, skill.APMult * 0.15F, true);
	}

	public override void ActivateEffect(Player player, Enemy enemy, PCNPC whoUsedIt) {
		if (whoUsedIt == PCNPC.NPC) {
			apWhenActivated = enemy.AP;
			player.effects.Add (this);
		} else {
			apWhenActivated = player.AP;
			enemy.effects.Add (this);
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
			enemy.effects.Remove (this);
		} else {
			player.effects.Remove (this);
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
