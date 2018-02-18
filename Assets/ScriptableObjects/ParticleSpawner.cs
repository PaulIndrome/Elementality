using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="custom/ParticleSpawner")]
public class ParticleSpawner : ScriptableObject {

	public bool rotateToTransform;
	public float preSetDuration;
	public GameObject particleObject;

	ParticleSystem particleSystem;
	GameObject spawn;

	public void SpawnParticleSystem(Transform tr){
		if(!CheckParticleObject()) return;
		if(preSetDuration > 0){
			var main = particleSystem.main;
			main.duration = preSetDuration;
		}
		spawn = Instantiate(particleObject, tr.position, (rotateToTransform ? tr.rotation : Quaternion.identity));
	}

	public void SpawnParticleSystem(Transform tr, float dur){
		if(!CheckParticleObject()) return;
		var main = particleSystem.main;
		main.duration = dur;
		spawn = Instantiate(particleObject, tr.position, (rotateToTransform ? tr.rotation : Quaternion.identity));
	}

	public void SpawnParticleSystem(Transform tr, Color col){
		if(!CheckParticleObject()) return;
		var main = particleSystem.main;
		main.startColor = col;
		if(preSetDuration > 0){
			main.duration = preSetDuration;
		}
		spawn = Instantiate(particleObject, tr.position, (rotateToTransform ? tr.rotation : Quaternion.identity));
	}

	public void SpawnParticleSystem(Transform tr, float dur, Color col){
		if(!CheckParticleObject()) return;
		var main = particleSystem.main;
		main.startColor = col;
		main.duration = dur;
		spawn = Instantiate(particleObject, tr.position, (rotateToTransform ? tr.rotation : Quaternion.identity));
	}

	bool CheckParticleObject(){
		particleSystem = particleObject.GetComponent<ParticleSystem>();
		return particleSystem != null;
	}

}
