using UnityEngine;
using System.Collections;

public class ReduceDamageTaken : Effect {
	
	public override void AddToSkill(Skill skill) {
		selfTar = skill.selfTar;
	}

	public void UpdateTimeLeft() {
		round--;
	}

	public override void ActivateEffect(Player player, Enemy enemy, PCNPC whoUsedIt) {
		if (enemy == null) {
			player.effects.Add (this);
			player.additionalReductions = 50.0F;
		} else {
			enemy.effects.Add (this);
			enemy.additionalReductions = 50.0F;
		}
	}

	public override void DoStuff(Player player, Enemy enemy, PCNPC whoUsedIt) {
		if (enemy == null) {} else {}
		round--;
	}

	public override void DeactivateEffect(Player player, Enemy enemy, PCNPC whoUsedIt) {
		if (enemy == null) {
			player.effects.Remove (this);
			player.additionalReductions = 0.0F;
		} else {
			enemy.effects.Remove (this);
			enemy.additionalReductions = 0.0F;
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
