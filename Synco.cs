using UnityEngine;
using System.Collections;

public delegate void Excutor();

public delegate Signal Runnable();

public class Synco : MonoBehaviour {
	
	static Synco getSynco(MonoBehaviour behaviour) {
		Synco synco = behaviour.gameObject.GetComponent<Synco>();
		if(synco == null) {
			synco = behaviour.gameObject.AddComponent<Synco>();
		}
		return synco;
	}
	
	public static Signal seqExcute(MonoBehaviour behaviour, params Runnable[] runnables) {
		Synco synco = getSynco(behaviour);
		return waitCoroutine(behaviour, behaviour.StartCoroutine(synco.seq(runnables)));
	}
	
	public static Signal parExcute(MonoBehaviour behaviour, params Runnable[] runnables) {
		Signal signal = new Signal(true);
		foreach(Runnable runnable in runnables) {
			signal = Signal.SignalAnd(signal, runnable());
		}
		return signal;
	}
	
	public static Signal waitCoroutine(MonoBehaviour behaviour, Coroutine cor) {
		Signal sig = new Signal(false);
		behaviour.StartCoroutine(coroutineToSignal(cor, sig));
		return sig;
	}
	
	public static Signal sleep(MonoBehaviour behaviour, float time) {
		Synco synco = getSynco(behaviour);
		return waitCoroutine(behaviour, behaviour.StartCoroutine(synco.sleep(time)));
	}
	
	IEnumerator seq(Runnable[] runnables) {
		foreach(Runnable runnable in runnables) {
			yield return StartCoroutine(repeatUntil(runnable(), Time.fixedDeltaTime, null, null));
		}
	}
	
	static IEnumerator coroutineToSignal(Coroutine cor, Signal sig) {
		yield return cor;
		sig.state = true;
	}
	
	IEnumerator repeatUntil(Signal signal, float time, Excutor handlerForFalse, Excutor handlerForTrue) {
		while(!signal.state) {
			if(handlerForFalse != null) handlerForFalse();
			yield return new WaitForSeconds(time);
		}
		if(handlerForTrue != null) handlerForTrue();
	}
	
	IEnumerator sleep(float time) {
		yield return new WaitForSeconds(time);
	}
}
