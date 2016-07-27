using UnityEngine;
using System.Collections;

public class MultiRoundDirectAttack : Effect {

	//Skill type, self target or enemy target.
	Skill simpleSkill;



	public override void AddToSkill(Skill skill) {
		selfTar = skill.selfTar;
		simpleSkill = new SimpleSkill ("Flurry", skill.selfTar, skill.type, skill.APMult * 0.15F);

	}

	public void UpdateTimeLeft() {
		round--;
	}

	public override void ActivateEffect(Player player, Enemy enemy, PCNPC whoUsedIt) {
		if (enemy == null) {
			player.effects.Add (this);
			player.isMultiRoundAttack = true;
			player.multiRoundAttack = simpleSkill;
		} else {
			enemy.effects.Add (this);
			enemy.isMultiRoundAttack = true;
			enemy.multiRoundAttack = simpleSkill;
		}
	}

	public override void DoStuff(Player player, Enemy enemy, PCNPC whoUsedIt) {
		if (enemy == null) {} else {}
		round--;
	}

	public override void DeactivateEffect(Player player, Enemy enemy, PCNPC whoUsedIt) {
		if (enemy == null) {
			player.effects.Remove (this);			
			player.isMultiRoundAttack = false;
			player.multiRoundAttack = null;
		} else {
			enemy.effects.Remove (this);
			enemy.isMultiRoundAttack = false;
			enemy.multiRoundAttack = null;
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
