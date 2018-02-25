using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthDefenseWall : PlayerAction {

	float wallDuration;
	Transform[] wallObjects;
	Vector3[] upperPositions, lowerPositions;

	[MinMaxRange(18.5f, 25.5f)]
	public RangedFloat randomSpeedRange;
	
	public void SetupWall(int pNum, float buffDuration){
		wallObjects = transform.GetComponentsInChildren<Transform>();
		upperPositions = new Vector3[wallObjects.Length-1];
		lowerPositions = new Vector3[wallObjects.Length-1];
		for(int i = 1; i < wallObjects.Length; i++){
			upperPositions[i-1] = wallObjects[i].position;
			wallObjects[i].position -= Vector3.up * 7.5f;
			lowerPositions[i-1] = wallObjects[i].position;
		}

		wallDuration = buffDuration;
		SetPlayerNum(pNum);
		StartCoroutine(RaiseWall());
	}

	IEnumerator RaiseWall(){
		bool[] wallPieceRisen = new bool[upperPositions.Length];
		float[] randomRiseSpeeds = new float[upperPositions.Length];
		for(int i = 0; i < upperPositions.Length; i++){
			randomRiseSpeeds[i] = randomSpeedRange.Random();
			wallPieceRisen[i] = false;
		}

		float allRisen = upperPositions.Length;

		while(allRisen > 0){
			for(int i = 1; i < wallObjects.Length; i++){
				if(!wallPieceRisen[i-1] && wallObjects[i].position == upperPositions[i-1]){
					wallPieceRisen[i-1] = true;
					allRisen -= 1;
				}
				wallObjects[i].position = Vector3.MoveTowards(wallObjects[i].position, upperPositions[i-1], Time.deltaTime * randomRiseSpeeds[i-1]);
			}
			yield return null;
		}

		yield return new WaitForSeconds(wallDuration);
		
		StartCoroutine(LowerWall());
	}

	IEnumerator LowerWall(){
		bool[] wallPieceLowered = new bool[lowerPositions.Length];
		float[] randomFallSpeeds = new float[lowerPositions.Length];
		for(int i = 0; i < lowerPositions.Length; i++){
			randomFallSpeeds[i] = randomSpeedRange.Random();
			wallPieceLowered[i] = false;
		}

		float allLowered = lowerPositions.Length;

		while(allLowered > 0){
			for(int i = 1; i < wallObjects.Length; i++){
				if(!wallPieceLowered[i-1] && wallObjects[i].position == lowerPositions[i-1]){
					wallPieceLowered[i-1] = true;
					allLowered -= 1;
				}
				wallObjects[i].position = Vector3.MoveTowards(wallObjects[i].position, lowerPositions[i-1], Time.deltaTime * randomFallSpeeds[i-1]);
			}
			yield return null;
		}

		Destroy(gameObject);

	}



}
