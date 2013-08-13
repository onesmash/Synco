using UnityEngine;
using System.Collections;

namespace FlowControl {

	public delegate void Excutor();

	public delegate Signal Runnable();

	public class Synco {

		public static Signal delayExcute(MonoBehaviour behaviour, float time, Excutor excutor) {
			return seqExcute(behaviour,
			          	() => {
							return sleep(behaviour, time);
						},
						() => {
							excutor();
							return Signal.Triggered;
						});
		}

		public static Signal repeatExcuteDuring(MonoBehaviour behaviour, float time, Excutor excutor) {
			Signal timeout = new Signal();
			return parExcute(behaviour,
			                 	() => {
									return delayExcute(behaviour, time, () => {
										timeout.Trigger();
									});
								},
								() => {
									return waitCoroutine(behaviour, behaviour.StartCoroutine(repeatUntil(timeout, Time.fixedDeltaTime, excutor, null)));
								});
		}

		public static Signal seqExcute(MonoBehaviour behaviour, params Runnable[] runnables) {

			return waitCoroutine(behaviour, behaviour.StartCoroutine(seq(behaviour, runnables)));
		}

		public static Signal parExcute(MonoBehaviour behaviour, params Runnable[] runnables) {
			Signal signal = new Signal(Signal.State.Triggered);
			foreach(Runnable runnable in runnables) {
				signal = Signal.SignalAnd(signal, runnable());
			}
			return signal;
		}

		static Signal waitCoroutine(MonoBehaviour behaviour, Coroutine cor) {
			Signal sig = new Signal();
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
			sig.Trigger();
		}

		static IEnumerator repeatUntil(Signal signal, float time, Excutor handlerForFalse, Excutor handlerForTrue) {
			while(signal.state == Signal.State.Init) {
				if(handlerForFalse != null) handlerForFalse();
				yield return new WaitForSeconds(time);
			}
			if (signal.state == Signal.State.Triggered) {
				if(handlerForTrue != null) handlerForTrue();
			}
		}

		static IEnumerator sleep(float time) {
			yield return new WaitForSeconds(time);
		}
	}

}

