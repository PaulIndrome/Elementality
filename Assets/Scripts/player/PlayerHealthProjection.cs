using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthProjection : MonoBehaviour {

	public Material[] healthCirclesMats;
	Projector projector;
	int currentHealth;

	void Start () {
		projector = GetComponent<Projector>();
		GetComponentInParent<PlayerHealth>().healthChangeEvent += UpdateHealthCircle;
	}

	public void UpdateHealthCircle(int cH){
		currentHealth = Mathf.Clamp(cH, 0, 3);
		projector.material = healthCirclesMats[currentHealth];
	}
}
