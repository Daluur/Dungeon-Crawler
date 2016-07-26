using UnityEngine;
using System.Collections;

public class WeakOverTime : Effect {

	string description = "Adds over time effect: 5% of direct damage or healing, per round for 3 rounds.";

	//Skill type, self target or enemy target.
	Skill simpleSkill;
	bool selfTar;

	public int round = 3;


	public void AddToSkill(Skill skill) {
		simpleSkill = new Skill("Over Time", skill.selfTar, skill.type, skill.APMult*0.05F, 0);
		selfTar = skill.selfTar;
	}

	public void ActivateEffect(Player player) {

	}

	public void DoStuff(Player player) {
		player.UseEffect (simpleSkill);
		round--;
	}

	public void DeactivateEffect(Player player) {
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
