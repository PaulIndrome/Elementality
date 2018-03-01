/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerLightningEffect))]
public class PlayerLightningElementEffectInspector : Editor{

    PlayerLightningEffect ple;


    public void OnEnable(){
        ple = serializedObject.targetObject as PlayerLightningEffect;
    }

    public override void OnInspectorGUI(){
        serializedObject.Update();
        GUI.backgroundColor = GUI.contentColor = GUI.color = new Color(159f/255f,255f/255f,255f/255f);
        DrawDefaultInspector();
        serializedObject.ApplyModifiedProperties();
    }
}*/