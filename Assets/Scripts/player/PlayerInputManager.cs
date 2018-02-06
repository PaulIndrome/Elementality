using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour{
    public int playerNumber;
    public void SetNumber(int pNum){
        playerNumber = pNum;
    }

    void Start(){
        Setup();
    }
    void Setup(int pNum){
        PlayerAction[] playerActions = GetComponents<PlayerAction>();
        foreach(PlayerAction pA in playerActions)
            pA.SetPlayerNum(pNum);
    }
    void Setup(){
        PlayerAction[] playerActions = GetComponents<PlayerAction>();
        foreach(PlayerAction pA in playerActions)
            pA.SetPlayerNum(playerNumber);
    }
}