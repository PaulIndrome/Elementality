using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpawnCollision : MonoBehaviour {
	public int playerNum;
	public Color[] playerColors;
	public ParticleSystem fireCircleParticles;
	public PlayerPickup damagePickup;
	public void Setup(int pNum, float fireBurnTime){
		playerNum = pNum;
		ParticleSystem.MainModule main = fireCircleParticles.main;
		main.duration = fireBurnTime;
		main.startColor = playerColors[playerNum];
		fireCircleParticles.Play(true);
		StartCoroutine(DisableColliderAfter(fireBurnTime - 1f));
	}
	public void OnTriggerEnter(Collider col){
		PlayerElementHolder playerElementHolder = col.gameObject.GetComponent<PlayerElementHolder>();
		if(playerElementHolder != null && playerNum != playerElementHolder.playerNum){
			damagePickup.Apply(playerElementHolder);
		}
	}
	IEnumerator DisableColliderAfter(float time){
		yield return new WaitForSeconds(time);
		yield return new WaitUntil(() => fireCircleParticles.particleCount <= 10);
		GetComponent<SphereCollider>().enabled = false;
	}
}