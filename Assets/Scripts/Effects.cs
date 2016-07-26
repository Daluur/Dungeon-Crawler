using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Effects : MonoBehaviour {

	List<Effect> effects = new List<Effect>();

	void Awake () {
		effects.Add (new WeakOverTime ());
	}

}
