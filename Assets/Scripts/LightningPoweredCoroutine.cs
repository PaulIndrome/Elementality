using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LightningPoweredCoroutine{
    public int playerNumber;

    public Coroutine powerRoutine;
    public LightningPoweredCoroutine(LightningBuffHandler runner, int pNum, float buffTime){
        playerNumber = pNum;
        powerRoutine = runner.StartCoroutine(PowerRoutineStart(buffTime));
    }

    public IEnumerator PowerRoutineStart(float buffTime){
        LightningBuffHandler.slowedPlayers[playerNumber] = true;
        yield return new WaitForSeconds(buffTime);
        LightningBuffHandler.slowedPlayers[playerNumber] = false;
    }

}
