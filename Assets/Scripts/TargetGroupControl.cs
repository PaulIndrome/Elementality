using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TargetGroupControl : MonoBehaviour {


	static CinemachineTargetGroup cinemachineTargetGroup;
	// Use this for initialization
	void Start () {
		cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();
	}
	
	public static void ToggleWeightOfPlayer(int pNum, bool active){
		cinemachineTargetGroup.m_Targets[pNum].weight = active ? 1 : 0;
	}
}
