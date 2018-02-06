using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBuffHandler : MonoBehaviour {

	public delegate void LightningEffect(float buffTime, int pNum);
	public static LightningEffect lightningEffectEvent;

	public static bool[] slowedPlayers = new bool[]{false, false, false, false};
	public static bool[] exceptionPlayers = new bool[]{false, false, false, false};

	public static Queue<Coroutine> exceptionKeepers;

	public static bool buffActive;

	public static float speedDecreaseFactor;

	static Coroutine globalEffectCheck = null;

	//make a queue of coroutines handling the players' speedFactor state
	//as long as the queue isn't empty, 

	void Start(){
		exceptionKeepers = new Queue<Coroutine>();
		lightningEffectEvent += StartNewBuff;
	}

	// I am trying to code a PowerUp that debuffs all other players.
	// If player 0 collects the PowerUp, players 1 - 3 are slowed down.
	// If player 1 collects the same PowerUp while player 0's buff is still running
	// only players 2 and 3 are debuffed UNTIL
	// player 0's buff runs out, at which point he is debuffed as well UNTIL
	// player 1's buff runs out and no other players picked up the same PowerUp.

	// Essentially, as long as ANY player is buffed, 
	// all other players are debuffed UNLESS they have the buff themselves. 
	public IEnumerator GlobalEffectCheck(){
		int i;
		buffActive = true;
		while(exceptionKeepers.Count > 0){
			for(i = 0; i < slowedPlayers.Length; i++){
				if(exceptionPlayers[i])
					slowedPlayers[i] = false;
				else
					slowedPlayers[i] = true;
			}
			i = 0;
			yield return null;
		}
		buffActive = false;
		globalEffectCheck = null;
		yield return null;
	}

	public void StartNewBuff(float buffTime, int pNum){
		if(globalEffectCheck == null){
			StartCoroutine(GlobalEffectCheck());
		}
	}

}
