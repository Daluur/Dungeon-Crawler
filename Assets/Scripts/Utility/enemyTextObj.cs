using UnityEngine;
using System.Collections;

public class enemyTextObj : MonoBehaviour {

	public void AnimationEnded(){
		CombatText._instance.EnemyAnimEnd ();
	}
}
