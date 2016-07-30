using UnityEngine;
using System.Collections;

public class ReduceDamageTaken : Effect {

	new public string name = "Reduce Damage Taken";
	bool selfTar;

	new int round = 2;

	public ReduceDamageTaken(Skill skill) {
		effectFromSkill = skill.name;
		selfTar = skill.selfTar;
	}
	
	public override void AddToSkill(Skill skill) {
		effectFromSkill = skill.name;
		selfTar = skill.selfTar;
	}

	public override void ActivateEffect(Player player, Enemy enemy, PCNPC whoUsedIt) {
		if (selfTar) {
			if (whoUsedIt == PCNPC.NPC) {
				enemy.AddEffect (this);
				enemy.additionalReductions = 50.0F;
			} else {
				player.AddEffect (this);
				player.additionalReductions = 50.0F;
			}
		} else {
			if (whoUsedIt == PCNPC.NPC) {
				player.AddEffect (this);
				player.additionalReductions = 50.0F;
			} else {
				enemy.AddEffect (this);
				enemy.additionalReductions = 50.0F;
			}
		}
	}

	public override void DoStuff(Player player, Enemy enemy, PCNPC whoUsedIt) {
		if (enemy == null) {} else {}
		round--;
	}

	public override void DeactivateEffect(Player player, Enemy enemy, PCNPC whoUsedIt) {
		if (whoUsedIt == PCNPC.NPC) {
			enemy.RemoveEffect (this);
			enemy.additionalReductions = 0.0F;
		} else {
			player.RemoveEffect (this);
			player.additionalReductions = 0.0F;
		}
		round = 2;
	}

	public override void ResetEffect (Player player, Enemy enemy, PCNPC whoUsedIt) {
		round = 2;
	}

	public override bool IsOver () {
		if (round > 0) {
			return false;
		}
		return true;
	} 
}
