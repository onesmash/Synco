using UnityEngine;
using System.Collections;

public class Signal {
	
	public static readonly Signal TRUE = new Signal(true);
	
	public static readonly Signal FALSE = new Signal(false);
	
	enum Strategy {And, Or, Single};
	
	Strategy strategy_ = Strategy.Single;

	private bool state_;
	
	private Signal[] signals_;
	
	public bool state {
		get {
			switch (strategy_) {
			case Strategy.Single: {
				return state_;
			} break;
			case Strategy.And: {
				for(int i = 0; i < signals_.Length; i++) {
					if(signals_[i] == null || signals_[i].state) continue;
					return false;
				}
				return true;
			} break;
			case Strategy.Or: {
				for(int i = 0; i < signals_.Length; i++) {
					if(signals_[i] == null || !signals_[i].state) continue;
					return true;
				}
				return false;
			} break;
			default: return false;
			}
			
		}
		
		set {
			if(strategy_ == Strategy.Single) state_ = value;
		}
	}
	
	public Signal(bool signal) {
		state_ = signal;
		strategy_ = Strategy.Single;
	}
	
	private Signal(Signal[] signals, Strategy strategy) {
		signals_ = signals;
		strategy_ = strategy;
	}
	
	
	public static Signal SignalAnd(params Signal[] sig) {
		return new Signal(sig, Strategy.And);
	}
	
	public static Signal SignalOr(params Signal[] sig) {
		return new Signal(sig, Strategy.Or);
	}
	
}
