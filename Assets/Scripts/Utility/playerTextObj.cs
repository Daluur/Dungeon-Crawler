using UnityEngine;
using System.Collections;

public class playerTextObj : MonoBehaviour {

	public void AnimationEnded(){
		CombatText._instance.PlayerAnimEnd ();
	}
}
