using UnityEngine;
using System.Collections;

public class InfoMessage : MonoBehaviour {

	public void AnimationEnded(){
		CombatText._instance.EndInfoMessage ();
	} 
}
