using UnityEngine;
using System.Collections;

public class ReduceDamage : Effect {

	string description = "Reduce all damage taken by 50%";

	//Skill type, self target or enemy target.
	bool selfTar;

	public int round = 3;


	public void AddToSkill(Skill skill) {
		selfTar = skill.selfTar;
	}

	public void UpdateTimeLeft() {
		round--;
	}

	public void ActivateEffect(Player player) {
		player.additionReductions = 50.0F;
	}

	public void DoStuff(Player player) {
		round--;
	}

	public void DeactivateEffect(Player player) {
		player.additionReductions = 0.0F;
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
