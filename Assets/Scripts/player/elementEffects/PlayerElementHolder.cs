using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerElementHolder : PlayerAction{
    
    public Elements.Element currentElement;

    public PlayerElementEffect[] elementEffects;
    void Start(){
        elementEffects = GetComponentsInChildren<PlayerElementEffect>();
    }

    public void SlotElement(PlayerPickup pickup){
        //this means that a health pickup (both damage and healing) clears the element
        currentElement = pickup.element;

        foreach(PlayerElementEffect pE in elementEffects){
            if(pE.element == pickup.element){
                pE.Activate(pickup);
            } else {
                continue;
            }
        }
    }

    public void SlotElement(HealthPickup healthPickup){
        // slotted element is only set to none when healed
        if(healthPickup.healthImpact >= 1){
            currentElement = healthPickup.element;
        } 

        foreach(PlayerElementEffect pE in elementEffects){
            if(pE.element == healthPickup.element){
                pE.Activate(healthPickup);
            } else {
                continue;
            }
        }
    }

}