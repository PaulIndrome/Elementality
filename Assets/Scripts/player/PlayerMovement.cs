using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerAction {

	[Header("Movement")]
	public float m_movementSpeed;
	public float m_turnSpeed;
	[Range(0,1)] public float m_dodgeSpeedReduction;
	public float m_accTime, m_decTime;
	private float m_accTimeElapsed, m_decTimeElapsed;
	CharacterController m_characterController;
	Vector3 m_playerDirectionNext, m_playerDirectionCurrent = Vector3.zero;
	PlayerController m_playerController;
	PlayerHealth playerHealth;
	PlayerJump m_playerJump;
	Animator playerAnimator;

	public AnimationCurve knockBackCurve;
	public float knockBackRange, knockBackHeight, knockBackSpeed;

	string horizontalMoveAxis, verticalMoveAxis;
	bool idleRunning = false;

    void Start () {
		playerAnimator = GetComponentInChildren<Animator>();
		m_characterController = GetComponent<CharacterController>();
		m_playerController = GetComponent<PlayerController>();
		m_playerJump = GetComponent<PlayerJump>();
	}
	
	void Update () {
		if(m_playerController.CanMove){

			m_playerDirectionNext.x = Input.GetAxis(horizontalMoveAxis);
			m_playerDirectionNext.z = Input.GetAxis(verticalMoveAxis);

			float inputMagnituede = m_playerDirectionNext.magnitude;

			if(inputMagnituede > 0.15f){
				idleRunning = false;
				playerAnimator.SetFloat("speed", inputMagnituede);
				MovementAcceleration();
			} else {
				if(inputMagnituede < 0.05f && !idleRunning){
					idleRunning = true;
					playerAnimator.SetFloat("idleBlend", Random.Range(0,3));
				} else {
					MovementDeceleration();
				}
			}
		}
	}

	public void MovementAcceleration(){
		m_accTimeElapsed = Mathf.Clamp(m_accTimeElapsed + Time.deltaTime, 0, m_accTime);

		Vector3 playerMoveSpeedActual = m_playerDirectionNext * Time.deltaTime * m_movementSpeed;
		Debug.Log(playerMoveSpeedActual);
		
		Vector3 moveNext = Vector3.Lerp(m_playerDirectionCurrent, playerMoveSpeedActual, m_accTimeElapsed / m_accTime);
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(m_playerDirectionNext, Vector3.up), (m_accTimeElapsed / m_accTime) * m_turnSpeed);
		m_characterController.Move(moveNext);
		m_playerDirectionCurrent = moveNext;
		m_decTimeElapsed = 0f;
	}

	public void MovementDeceleration(){
		m_decTimeElapsed = Mathf.Clamp(m_decTimeElapsed + Time.deltaTime, 0, m_decTime);
		Vector3 moveNext = Vector3.Lerp(m_playerDirectionCurrent, Vector3.zero, m_decTimeElapsed / m_decTime);
		playerAnimator.SetFloat("speed", moveNext.magnitude);
		m_characterController.Move(moveNext);
		m_playerDirectionCurrent = moveNext;
		m_accTimeElapsed = 0f;
	}

	public void StartDodgeRecovery(float coolDown){
		StartCoroutine(DodgeRecover(coolDown));
	}

	public override void SetPlayerNum(int pNum){
		base.SetPlayerNum(pNum);
		horizontalMoveAxis = "HorizontalMove" + pNum;
		verticalMoveAxis = "VerticalMove" + pNum;
	}

	IEnumerator DodgeRecover(float coolDown){
		float timer = 0f;
		float originalSpeed = m_movementSpeed;
		float reducedSpeed =  m_movementSpeed * m_dodgeSpeedReduction;
		while(timer < coolDown){
			m_movementSpeed = Mathf.Lerp(reducedSpeed, originalSpeed, timer / coolDown);
			timer += Time.deltaTime;
			yield return null;
		}
		m_movementSpeed = originalSpeed;
		yield break;
	}

	public void Hit(bool melee, Vector3 hitFromPosition){
		if(melee){
			m_playerController.CanMove = false;
			StartCoroutine(KnockBack((transform.position - hitFromPosition).normalized));
		}
	}

	IEnumerator KnockBack(Vector3 flyDirection){
		float x = 0;
		Vector3 originalPosition = transform.position;
		Vector3 finalPosition = originalPosition + (flyDirection * knockBackRange);
		Debug.DrawRay(originalPosition, finalPosition);

		Vector3 xzMovement = finalPosition - originalPosition;
		xzMovement.y = 0;
		Debug.Log(xzMovement.magnitude);

		float yOriginal = originalPosition.y;
		m_playerJump.flying = true;
		while(x <= 1){
			Debug.DrawRay(originalPosition, finalPosition);
			Vector3 nextMove = (xzMovement * Time.deltaTime * knockBackSpeed);
			m_characterController.Move(nextMove);
			m_characterController.Move(new Vector3(0, knockBackCurve.Evaluate(x) * knockBackHeight, 0));
			//transform.position += flyDirection * x * knockBackRange * Time.deltaTime;
			//transform.position += new Vector3(0, knockBackCurve.Evaluate(x) * knockBackHeight, 0);
			x += Time.deltaTime * knockBackSpeed;
			yield return null;
		}
		m_playerJump.flying = false;
		m_playerController.CanMove = true;
	}

}
