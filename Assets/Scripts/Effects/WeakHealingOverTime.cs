using UnityEngine;
using System.Collections;

public class WeakHealingOverTime : Effect {

	string description = "Adds healing over time effect: 15% of direct damage per round for 3 rounds.";

	//Skill type, self target or enemy target.
	Skill simpleSkill;
	bool selfTar;

	public int round = 3;


	public void AddToSkill(Skill skill) {
		selfTar = skill.selfTar;
		simpleSkill = new Skill ("Over Time", true, skill.type, skill.APMult * 0.15F, 0, false);


	}

	public void ActivateEffect(Player player, Enemy enemy = null) {
		if (enemy == null) {
			player.effects.Add (this);
		} else {
			enemy.effects.Add (this);
		}
	}

	public void DoStuff(Player player, Enemy enemy = null) {
		if (enemy == null) {
			player.UseEffect (simpleSkill);
		} else {
			enemy.UseEffect (simpleSkill);
		}
		round--;
	}

	public void DeactivateEffect(Player player, Enemy enemy = null) {
		if (enemy == null) {
			player.effects.Remove (this);
		} else {
			enemy.effects.Remove (this);
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
