using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Elements {
    public enum Element{
        None,
        Fire,
        Lightning,
        Earth
    }

    public static Element RandomElement(){
        int index;
        index = Random.Range(0, 4);
        switch(index){
            case 0: return Element.None;
            case 1: return Element.Fire;
            case 2: return Element.Lightning;
            case 3: return Element.Earth;
            default: return Element.None;
        }
    }
}