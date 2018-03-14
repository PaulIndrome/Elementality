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
        foreach(PlayerElementEffect pE in elementEffects){
            if(pE.element == pickup.element){
                if(pickup.element == Elements.Element.None){
                    HealthPickup hp = pickup as HealthPickup;
                    if(hp.healthImpact >= 1){
                        // slotted element is only set to none when healed
                        currentElement = hp.element;
                    }
                } else {
                    currentElement = pickup.element;
                }
                pE.SlotPickup(pickup);
            } else {
                continue;
            }
        }
    }

    //public void SlotElement(HealthPickup healthPickup){
    //    
    //    if(healthPickup.healthImpact >= 1){
    //        currentElement = healthPickup.element;
    //    } 
//
    //    foreach(PlayerElementEffect pE in elementEffects){
    //        if(pE.element == healthPickup.element){
    //            pE.SlotPickup(healthPickup);
    //        } else {
    //            continue;
    //        }
    //    }
    //}

}