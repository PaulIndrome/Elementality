using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour {
    protected bool collided = false;
	protected int shotByPlayerNum, shotAtPlayerNum;
	protected Transform currentTarget;
	protected Coroutine moveProjectile;

    public Elements.Element projectileElement;

    public virtual void Shoot(int shotByPlayer, PlayerMovement playerMov){
        if(playerMov != null && shotByPlayer == playerMov.playerNum) {
			Debug.LogError("Player " + shotByPlayer + " shooting at " + playerMov.playerNum);
		}
        collided = false;
        shotByPlayerNum = shotByPlayer;
        shotAtPlayerNum = (playerMov != null) ? playerMov.playerNum : -1;
        currentTarget = (playerMov != null) ? playerMov.transform : null;
        moveProjectile = StartProjectile();
	}

    public abstract Coroutine StartProjectile();
    

}