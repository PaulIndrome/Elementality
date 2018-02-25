using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerHealth))]
public class PlayerHealthInspector : PlayerElementEffectInspector {

	PlayerHealth pH;
	public Texture2D redTex, greenTex, whiteTex, clickedTex;
	public GUIStyle red, green, white;

	SerializedProperty playerNumProp, currentHealthProp, playerRespawnProp, meleeParticleProp;

	public override void OnEnable(){
		base.OnEnable();
		pH = pee as PlayerHealth;
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

		playerNumProp = serializedObject.FindProperty("playerNum");
		currentHealthProp = serializedObject.FindProperty("healthCurrent");
		playerRespawnProp = serializedObject.FindProperty("playerRespawnTime");
		meleeParticleProp = serializedObject.FindProperty("meleeParticle");
	}

	public override void OnInspectorGUI(){
		serializedObject.Update();

		GUILayout.BeginHorizontal();
		if(GUILayout.Button("", white, GUILayout.MaxHeight(20f), GUILayout.MaxWidth(20f))){
			pH.currentHealth = ((pH.currentHealth == 0) ? 3 : 0);	
		}
		for(int i = 0; i<3; i++){
			GUILayout.BeginVertical();
			if(i < pH.currentHealth){
				if(GUILayout.Button(greenTex, green, GUILayout.MinHeight(20f))){
					pH.currentHealth = i+1;	
				}
			} else {
				if(GUILayout.Button(redTex, red, GUILayout.MinHeight(20f))){
					pH.currentHealth = i+1;
				}
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndHorizontal();

		DrawInspector(false);

		//GUI.backgroundColor = GUI.contentColor = GUI.color = new Color(255f/255f,202f/255f,202f/255f);

		EditorGUILayout.LabelField("Player Number: " + playerNumProp.intValue);
		pH.element = (Elements.Element) EditorGUILayout.EnumPopup("Element", pH.element);
		EditorGUILayout.IntSlider(currentHealthProp, 0, 3, new GUIContent("Current Health"));
		EditorGUILayout.Slider(playerRespawnProp, 1, 10, new GUIContent("Respawn Time"));
		EditorGUILayout.ObjectField(meleeParticleProp, typeof(ParticleSpawner), new GUIContent("get-hit particles"));

		serializedObject.ApplyModifiedProperties();
	}


}
