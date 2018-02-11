using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerHealth))]
public class PlayerHealthInspector : Editor {
	PlayerHealth pH;

	public Texture2D redTex, greenTex, whiteTex, clickedTex;
	public GUIStyle red, green, white;

	void OnEnable(){
		red = new GUIStyle();
		green = new GUIStyle();
		white = new GUIStyle();

		red.normal.background = redTex;
		green.normal.background = greenTex;
		white.normal.background = whiteTex;

		red.onActive.background = green.onActive.background = whiteTex;
		white.onActive.background = redTex;

		red.stretchHeight = green.stretchHeight = true;

		red.border = green.border = white.border = new RectOffset(2,2,2,2);

		red.margin = green.margin = white.margin = new RectOffset(20,30,10,10);

		pH = (PlayerHealth) target;
	}

	public override void OnInspectorGUI(){

		GUILayout.BeginHorizontal();
		if(GUILayout.Button("", white, GUILayout.MaxHeight(20f), GUILayout.MaxWidth(20f))){
			pH.currentHealth = ((pH.currentHealth == 0) ? 3 : 0);	
		}
		for(int i = 0; i<3; i++){
			GUILayout.BeginVertical();
			if(i < pH.currentHealth){
				if(GUILayout.Button(greenTex, green, GUILayout.MinHeight(20f))){
					pH.currentHealth += i+1;	
				}
			} else {
				if(GUILayout.Button(redTex, red, GUILayout.MinHeight(20f))){
					pH.currentHealth += i+1;
				}
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndHorizontal();

		GUI.color = Color.white;

		DrawDefaultInspector();

	}
}
