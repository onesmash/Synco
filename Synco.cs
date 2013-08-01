using UnityEngine;
using System.Collections;

public delegate void Excutor();

public delegate Signal Runnable();

public class Synco {
	
	public static void delayExcute(MonoBehaviour behaviour, float time, Excutor excutor) {
		seqExcute(behaviour,
			() => {
				return sleep(behaviour, time);
			},
			() => {
				excutor();
				return Signal.TRUE;
			});
	}
	
	public static Signal seqExcute(MonoBehaviour behaviour, params Runnable[] runnables) {
		
		return waitCoroutine(behaviour, behaviour.StartCoroutine(seq(behaviour, runnables)));
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
		return waitCoroutine(behaviour, behaviour.StartCoroutine(sleep(time)));
	}
	
	static IEnumerator seq(MonoBehaviour behaviour, Runnable[] runnables) {
		foreach(Runnable runnable in runnables) {
			yield return behaviour.StartCoroutine(repeatUntil(runnable(), Time.fixedDeltaTime, null, null));
		}
	}
	
	static IEnumerator coroutineToSignal(Coroutine cor, Signal sig) {
		yield return cor;
		sig.state = true;
	}
	
	static IEnumerator repeatUntil(Signal signal, float time, Excutor handlerForFalse, Excutor handlerForTrue) {
		while(!signal.state) {
			if(handlerForFalse != null) handlerForFalse();
			yield return new WaitForSeconds(time);
		}
		if(handlerForTrue != null) handlerForTrue();
	}
	
	static IEnumerator sleep(float time) {
		yield return new WaitForSeconds(time);
	}
}
