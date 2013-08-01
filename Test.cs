using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Synco.parExcute(this, 
			() => {
				print(1);
				return Signal.TRUE;
			},
			() => {
				print(2);
				return Signal.TRUE;
			});
	
	}
}
	
	
