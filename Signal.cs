using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FlowControl {

	public class Signal {

		public static readonly Signal Triggered = new Signal(State.Triggered);

		public enum State {Init, Triggered};

		enum Strategy {And, Or, Single};

		Strategy strategy_ = Strategy.Single;

		State state_;

		HashSet<Signal> signals_;

		public State state {
			get {
					switch (strategy_) {
					case Strategy.Single:
							{
									return state_;
							}
							break;
					case Strategy.And:
							{
									if (state_ == State.Triggered)
											return state_;
				
									foreach (Signal sig in signals_) {
											if (sig.isTriggered ())
													continue;
											return state_;
									}
									Trigger ();
									return state_;
							}
							break;
					case Strategy.Or:
							{
									if (state_ == State.Triggered)
										return state_;
									
									foreach (Signal sig in signals_) {
										if (!sig.isTriggered ())
											continue;
										Trigger ();
										return state_;
									}
									return state_;
							}
							break;
					default:
							return state_;
					}

			}

		}

		public void Trigger() {
			if(state_ == State.Init) state_ = State.Triggered;
		}

		public bool isTriggered() {
			return state == State.Triggered;
		}

		public Signal(State st = State.Init) {
			state_ = st;
			strategy_ = Strategy.Single;
		}

		private Signal(Signal[] signals, Strategy strategy) {
			//signals_ = signals;
			strategy_ = strategy;
			signals_ = new HashSet<Signal> ();
			foreach (Signal sig in signals_) {
				if (sig == null)
						continue;
				signals_.Add (sig);
			}
			state_ = State.Init;
		}


		public static Signal SignalAnd(params Signal[] sig) {
			return new Signal(sig, Strategy.And);
		}

		public static Signal SignalOr(params Signal[] sig) {
			return new Signal(sig, Strategy.Or);
		}

	}
}


