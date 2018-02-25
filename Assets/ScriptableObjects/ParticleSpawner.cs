using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="custom/ParticleSpawner")]
public class ParticleSpawner : ScriptableObject {

	public bool rotateToTransform;
	public float preSetDuration, controlledDestroy = 0;
	public GameObject particleObject;

	ParticleSystem particleSystem;
	List<GameObject> spawns;

	public void OnEnable(){
		spawns = new List<GameObject>();
	}

	public void SpawnParticleSystem(Transform tr){
		if(!CheckParticleObject()) return;
		if(preSetDuration > 0.01f){
			var main = particleSystem.main;
			main.duration = preSetDuration;
		}
		GameObject spawn = Instantiate(particleObject, tr.position, (rotateToTransform ? tr.rotation : Quaternion.identity));
		spawns.Add(spawn);
		StartControlledDestroy(tr, spawn);
	}

	public void SpawnParticleSystem(Transform tr, float dur){
		if(!CheckParticleObject()) return;
		var main = particleSystem.main;
		main.duration = dur;
		GameObject spawn = Instantiate(particleObject, tr.position, (rotateToTransform ? tr.rotation : Quaternion.identity));
		spawns.Add(spawn);
		StartControlledDestroy(tr, spawn);
	}

	public void SpawnParticleSystem(Transform tr, Color col){
		if(!CheckParticleObject()) return;
		var main = particleSystem.main;
		main.startColor = col;
		if(preSetDuration > 0){
			main.duration = preSetDuration;
		}
		GameObject spawn = Instantiate(particleObject, tr.position, (rotateToTransform ? tr.rotation : Quaternion.identity));
		spawns.Add(spawn);
		StartControlledDestroy(tr, spawn);
	}

	public void SpawnParticleSystem(Transform tr, float dur, Color col){
		if(!CheckParticleObject()) return;
		var main = particleSystem.main;
		main.startColor = col;
		main.duration = dur;
		GameObject spawn = Instantiate(particleObject, tr.position, (rotateToTransform ? tr.rotation : Quaternion.identity));
		spawns.Add(spawn);
		StartControlledDestroy(tr, spawn);
	}

	bool CheckParticleObject(){
		particleSystem = particleObject.GetComponent<ParticleSystem>();
		return particleSystem != null;
	}

	void StartControlledDestroy(Transform tr, GameObject spawn){
		if(controlledDestroy > 0){
			MonoBehaviour mb = tr.gameObject.GetComponent<MonoBehaviour>();
			if(mb != null){
				mb.StartCoroutine(ControlledDestroy(spawn));
			}
		}
	}

	IEnumerator ControlledDestroy(GameObject spawn){
		yield return new WaitForSeconds(controlledDestroy);
		if(spawns.Contains(spawn)){
			spawns.Remove(spawn);
			Destroy(spawn);
		}
	}

}
