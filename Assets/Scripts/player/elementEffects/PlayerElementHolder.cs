using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerElementHolder : PlayerAction{
    
    public Elements.Element currentElement;

    public PlayerElementEffect[] elementEffects;
    void Start(){
        elementEffects = GetComponents<PlayerElementEffect>();
    }

    public void SlotElement(PlayerPickup pickup){
        currentElement = pickup.element;

        foreach(PlayerElementEffect pE in elementEffects){
            if(pE.element == pickup.element){
                pE.Activate(pickup);
            } else {
                continue;
            }
        }
    }

}