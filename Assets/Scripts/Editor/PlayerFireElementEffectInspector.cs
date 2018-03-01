/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerFireEffect))]
public class PlayerFireElementEffectInspector : Editor{

    PlayerFireEffect pfe;


    public void OnEnable(){
        pfe = serializedObject.targetObject as PlayerFireEffect;
    }

    public override void OnInspectorGUI(){
        serializedObject.Update();
        GUI.backgroundColor = GUI.contentColor = GUI.color = new Color(253f/255f,153f/255f,2f/255f);
        DrawDefaultInspector();
        serializedObject.ApplyModifiedProperties();
    }
}*/