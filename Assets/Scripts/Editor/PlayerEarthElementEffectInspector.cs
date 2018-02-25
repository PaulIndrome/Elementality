/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerEarthEffect))]
public class PlayerEarthElementEffectInspector : Editor{

    PlayerEarthEffect pee;


    public void OnEnable(){
        pee = serializedObject.targetObject as PlayerEarthEffect;
    }

    public override void OnInspectorGUI(){
        serializedObject.Update();
        GUI.backgroundColor = GUI.contentColor = GUI.color = new Color(186f/255f,167f/255f,122f/255f);
        DrawDefaultInspector();
        serializedObject.ApplyModifiedProperties();
    }
}*/