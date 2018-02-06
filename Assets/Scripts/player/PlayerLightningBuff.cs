using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightningBuff : PlayerAction{

	public delegate void lightningBuffDelegate(int pNum, bool startEnd);
	public static lightningBuffDelegate lightningBuffEvent;

	public delegate int lightningBuffRunningDelegate();
	public static lightningBuffRunningDelegate lightningBuffRunningEvent;

	public static int lightningBuffCheck;

	[HideInInspector] public enum Element {
		None,
		Fire,
		Earth,
		Lightning
	}	

	public Element currentElement;
	PlayerMovement playerMovement;
	Coroutine localBuffTimer;
	float originalMoveSpeed;
	
	[Range(0f,1f)]
	public static float moveSpeedReduction = 0.5f;

	public void Start(){
		if(moveSpeedReduction != 0.5f)
			moveSpeedReduction = 0.5f;

		this.playerMovement = GetComponent<PlayerMovement>();
		originalMoveSpeed = this.playerMovement.m_movementSpeed;
		
		lightningBuffEvent += LightningEventListener;
		lightningBuffRunningEvent += BuffRunningAnywhere;
	}

	public void SlotElement(Element element){
		this.currentElement = element;
	}

	public int BuffRunningAnywhere(){
		Debug.Log("Player " + playerNum + " checked for running Buffs");
		return localBuffTimer != null ? 1 : 0;
	}

	public void LightningEventListener(int pNum, bool startEnd){
		
		lightningBuffCheck += lightningBuffRunningEvent();

		Debug.Log("Player " + playerNum + " says the check amounts to " + lightningBuffCheck);

		switch(startEnd){
			// if a buff has started
			case true:
				// and I started it because I'm a badass!
				if(pNum == playerNum){
					// I start a new localbufftimer and keep my speed or get it back for thiiiiiiis many seconds!
					if(localBuffTimer != null) StopCoroutine(localBuffTimer);
					playerMovement.m_movementSpeed = originalMoveSpeed;
					localBuffTimer = StartCoroutine(LocalBuffTimer(5.0f));
					return;
				} else {
					// However, if someone else started it and I don't have a buff on me ...
					if(localBuffTimer == null){
						// ... my speed is reduced ;_;
						playerMovement.m_movementSpeed = originalMoveSpeed * moveSpeedReduction;
						return;
					}
					// if I DO have a buff on me, I keep my speed, yay! =^.^=
					else {
						return;
					}
				}
			// if a buff has ended
			case false:
				// and no more buffs are running anywhere.
				if(lightningBuffCheck <= 0){
					// everyone gets back up to speed, whoooo! 
					playerMovement.m_movementSpeed = originalMoveSpeed;
					return;
				} else {
					// otherwise the player who's buff has ended
					if(pNum == playerNum){
						// sadly gets his speed reduced. What a pity. 
						playerMovement.m_movementSpeed = originalMoveSpeed * moveSpeedReduction;
						return;
					} else {
						// Now, every other player that had nothing to do with the ending buff just keeps on truckin'.
						return;
					}
				}
		}
	}

	IEnumerator LocalBuffTimer(float buffTime){
		yield return new WaitForSeconds(buffTime);
		localBuffTimer = null;
		lightningBuffEvent(playerNum, false);
	}
	
}
