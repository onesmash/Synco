using UnityEngine;
using System.Collections;
using FlowControl;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
			Synco.seqExcute (this,
	              	() => {
						print ("time: " + Time.time);
						return Signal.Triggered;
					},
			        () => {
						print ("Hello world!");
						return Signal.Triggered;
					},
					() => {
						return Synco.parExcute(this, 
			            
									() => {
										float time = Time.time;
										return Synco.delayExcute(this, 5, () => {
											print("delta time: " + (Time.time - time));
										});
									},
									() => {
										return Synco.repeatExcuteDuring(this, float.PositiveInfinity, () => {
											//print(Time.time);
										});
									});
					});

			

	
	}
}
	
	
