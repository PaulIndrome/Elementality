using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerElementEffect), true)]
public class PlayerElementEffectInspector : Editor{

    protected PlayerElementEffect pee;


    public virtual void OnEnable(){
        pee = serializedObject.targetObject as PlayerElementEffect;
    }

    public override void OnInspectorGUI(){
        DrawInspector(true);
    }

    public virtual void DrawInspector(bool drawDefault){
        serializedObject.Update();
        Color color = GUI.color;
        switch(pee.element){
            case Elements.Element.None:
                color = new Color(255f/255f,202f/255f,202f/255f);
                break;
            case Elements.Element.Earth:
                color = new Color(186f/255f,167f/255f,122f/255f);
                break;
            case Elements.Element.Fire:
                color = new Color(253f/255f,153f/255f,2f/255f);
                break;
            case Elements.Element.Lightning:
                color = new Color(159f/255f,255f/255f,255f/255f);
                break;
        }
        GUI.backgroundColor = GUI.contentColor = GUI.color = color;

        if(drawDefault)
            DrawDefaultInspector();
            
        serializedObject.ApplyModifiedProperties();
    }
}