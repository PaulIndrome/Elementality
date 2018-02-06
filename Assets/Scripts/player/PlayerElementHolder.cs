using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerElementHolder : PlayerAction{
    
    public Elements.Element currentElement;

    public PlayerElementEffect[] elementEffects;
    void Start(){
        if(elementEffects == null)
            elementEffects = GetComponents<PlayerElementEffect>();
    }

    public void SlotElement(Elements.Element element){
        foreach(PlayerElementEffect pE in elementEffects){
            if(pE.element != element)
                pE.enabled = false;
            else {
                pE.enabled = true;
                
            }
                
        }
    }

}