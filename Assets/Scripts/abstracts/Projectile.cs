using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour {
    protected bool collided = false;
	protected int shotByPlayerNum, shotAtPlayerNum;
	protected Transform currentTarget;
	protected Coroutine moveProjectile;

    public float flightSpeed;
    public float reorientSpeed;
	public float reorientTime = 1.5f;

    public ParticleSpawner shotProjectile;
    public Elements.Element projectileElement;

    public virtual void OnTriggerEnter(Collider col){
        //Debug.Log("Projectile from " + shotByPlayerNum + " to " + shotAtPlayerNum + " collided with " + col.gameObject.name);
		PlayerMovement pm = col.GetComponent<PlayerMovement>();
		if(pm != null){
			StopCoroutine(moveProjectile);
			Destroy(gameObject);
		} else {
			return;
		}
    }

    public virtual void Shoot(int shotByPlayer, PlayerMovement playerMov){
        if(playerMov != null && shotByPlayer == playerMov.playerNum) {
			Debug.LogError("Player " + shotByPlayer + " shooting at " + playerMov.playerNum);
		}
        collided = false;
        shotByPlayerNum = shotByPlayer;
        shotAtPlayerNum = (playerMov != null) ? playerMov.playerNum : -1;
        currentTarget = (playerMov != null) ? playerMov.transform : null;
        moveProjectile = StartCoroutine(MoveProjectile());
	}

    protected abstract IEnumerator MoveProjectile();
    

}