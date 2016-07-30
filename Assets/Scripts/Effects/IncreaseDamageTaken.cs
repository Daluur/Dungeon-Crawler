using UnityEngine;
using System.Collections;

public class IncreaseDamageTaken : Effect {

	new public string name = "Increase Damage Taken";
	bool selfTar;

	new int round = 1;

	public IncreaseDamageTaken(Skill skill) {
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
				enemy.damageIncrease = 50.0F;
			} else {
				player.AddEffect (this);
				player.damageIncrease = 50.0F;
			}
		} else {
			if (whoUsedIt == PCNPC.NPC) {
				player.AddEffect (this);
				player.damageIncrease = 50.0F;
			} else {
				enemy.AddEffect (this);
				enemy.damageIncrease = 50.0F;
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
			enemy.damageIncrease = 0.0F;
		} else {
			player.RemoveEffect (this);
			player.damageIncrease = 0.0F;
		}
		round = 1;
	}

	public override void ResetEffect (Player player, Enemy enemy, PCNPC whoUsedIt) {
		round = 1;
	}

	public override bool IsOver () {
		if (round > 0) {
			return false;
		}
		return true;
	} 
}
