using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAction : MonoBehaviour{

    public int playerNum;

    public virtual void SetPlayerNum(int pNum){
        playerNum = pNum;
    }

}