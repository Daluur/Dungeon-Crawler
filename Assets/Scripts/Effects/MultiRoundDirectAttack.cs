using UnityEngine;
using System.Collections;

public class MultiRoundDirectAttack : Effect {

	string name = "Buff";

	string description = "Multi round attack";

	//Skill type, self target or enemy target.
	bool selfTar;

	public int round = 3;


	public void AddToSkill(Skill skill) {
		selfTar = skill.selfTar;
	}

	public void UpdateTimeLeft() {
		round--;
	}

	public void ActivateEffect(Player player = null, Enemy enemy = null) {
		if (enemy == null) {
			player.effects.Add (this);
			player.additionalReductions = 50.0F;
		} else {
			enemy.effects.Add (this);
			enemy.additionalReductions = 50.0F;
		}
	}

	public void DoStuff(Player player = null, Enemy enemy = null) {
		if (enemy == null) {} else {}
		round--;
	}

	public void DeactivateEffect(Player player = null, Enemy enemy = null) {
		if (enemy == null) {
			player.effects.Remove (this);
			player.additionalReductions = 0.0F;
		} else {
			enemy.effects.Remove (this);
			enemy.additionalReductions = 0.0F;
		}
		round = 3;
	}

	public bool IsOver () {
		if (round > 0) {
			return false;
		}
		return true;
	}

	public bool IsSelfTar() {
		return selfTar;
	}
}
