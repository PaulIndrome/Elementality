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
	PlayerElement m_playerElement;

	string horizontalMoveAxis, verticalMoveAxis;

    void Start () {
		m_characterController = GetComponent<CharacterController>();
		m_playerController = GetComponent<PlayerController>();
		m_playerElement = GetComponent<PlayerElement>();
	}
	
	void Update () {
		if(m_playerController.CanMove){

			m_playerDirectionNext.x = Input.GetAxis(horizontalMoveAxis);
			m_playerDirectionNext.z = Input.GetAxis(verticalMoveAxis);

			if(m_playerDirectionNext.magnitude > 0.15f){
				MovementAcceleration();
			} else {
				MovementDeceleration();
			}
		}
	}

	public void MovementAcceleration(){
		m_accTimeElapsed = Mathf.Clamp(m_accTimeElapsed + Time.deltaTime, 0, m_accTime);

		Vector3 playerMoveSpeedActual = m_playerDirectionNext * Time.deltaTime * m_movementSpeed;
		
		Vector3 moveNext = Vector3.Lerp(m_playerDirectionCurrent, playerMoveSpeedActual, m_accTimeElapsed / m_accTime);
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(m_playerDirectionNext, Vector3.up), (m_accTimeElapsed / m_accTime) * m_turnSpeed);
		m_characterController.Move(moveNext);
		m_playerDirectionCurrent = moveNext;
		m_decTimeElapsed = 0f;
	}

	public void MovementDeceleration(){
		m_decTimeElapsed = Mathf.Clamp(m_decTimeElapsed + Time.deltaTime, 0, m_decTime);
		Vector3 moveNext = Vector3.Lerp(m_playerDirectionCurrent, Vector3.zero, m_decTimeElapsed / m_decTime);
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

}
