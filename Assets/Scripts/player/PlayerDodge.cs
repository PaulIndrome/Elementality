using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodge : PlayerAction {

	[Header("Dodging")]
	public float m_dodgeDistance;
	public float m_dodgeSpeed;
	public float m_dodgeCooldown;
	[Tooltip("During cooldown after a dodge, the player is significantly slowed down")]
	public float m_stickDeadZone;

	CharacterController m_characterController;
	Vector3 m_dodgeDirection, m_dodgeDirectionTemp = Vector3.zero;
	PlayerController m_playerController;
	PlayerMovement m_playerMovement;
	Coroutine dodge;
	private bool cancelDodge = false;

	string horizontalDodgeAxis, verticalDodgeAxis;

	void Start () {
		m_characterController = GetComponent<CharacterController>();
		m_playerController = GetComponent<PlayerController>();
		m_playerMovement = GetComponent<PlayerMovement>();
		m_playerController.IsDodgingEvent += Dodging;
	}
	
	void Update () {
		m_dodgeDirection.x = Input.GetAxis(horizontalDodgeAxis);
		m_dodgeDirection.z = Input.GetAxis(verticalDodgeAxis);

		if(!m_playerController.IsDodging && m_dodgeDirection.magnitude > m_stickDeadZone){
			m_dodgeDirectionTemp = m_dodgeDirection.normalized;
			m_playerController.IsDodging = true;
		} 
	}

	public bool Dodging(bool isDodging){
		if(isDodging){
			dodge = StartCoroutine(Dodge());
			return true;
		} else 
			return false;
	}

	public override void SetPlayerNum(int playerNumber){
		base.SetPlayerNum(playerNumber);
		horizontalDodgeAxis = "HorizontalDodge" + playerNumber;
		verticalDodgeAxis = "VerticalDodge" + playerNumber;
	}

	public void OnControllerColliderHit(ControllerColliderHit hit){
		if(dodge != null && hit.transform.CompareTag("Obstacles")){
			cancelDodge = true;
		}
	}

	IEnumerator Dodge(){
		float distanceTravelled = 0;
		Vector3 startPos = transform.position;
		while(distanceTravelled < m_dodgeDistance && !cancelDodge){
			m_characterController.Move(m_dodgeDirectionTemp * Time.deltaTime * m_dodgeSpeed);
			distanceTravelled = Vector3.Distance(startPos, transform.position);
			yield return null;
		}
		m_playerController.CanMove = true;
		m_playerMovement.StartDodgeRecovery(m_dodgeCooldown);
		yield return new WaitForSeconds(m_dodgeCooldown);
		yield return new WaitUntil(() => m_dodgeDirection.magnitude <= m_stickDeadZone);
		m_playerController.IsDodging = false;
		cancelDodge = false;
		dodge = null;
		yield return null;
	}


}
