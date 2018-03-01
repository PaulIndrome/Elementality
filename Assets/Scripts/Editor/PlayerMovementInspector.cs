using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerMovement))]
public class PlayerMovementInspector : Editor{

    PlayerMovement playerMovement;
    public void OnEnable(){
        playerMovement = serializedObject.targetObject as PlayerMovement;
    }

    public override void OnInspectorGUI(){
        DrawDefaultInspector();
        if(GUILayout.Button("hit from 0,0,0")){
            playerMovement.Hit(true, Vector3.zero);
        }
    }

}