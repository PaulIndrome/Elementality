using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : PlayerAction {
	
	PlayerController playerController;
	CharacterController characterController;
	Vector3 gravityDirection = Vector3.down;

	[Header("Jumping")]
	public float m_playerGravity = 0f;
	public float m_jumpHeight;
	public float m_jumpSpeed;
	public float m_jumpApexTime;

	public bool isGroundedInspectorCheck;

	string playerJumpButton;

	void Start () {
		characterController = GetComponent<CharacterController>();
		playerController = GetComponent<PlayerController>();
		playerController.IsGroundedEvent += GravityChange;
	}

	public override void SetPlayerNum(int pNum){
		base.SetPlayerNum(pNum);
		playerJumpButton = "Jump" + pNum;
	}
	
	void Update () {
		isGroundedInspectorCheck = characterController.isGrounded;

		if(playerController.IsGrounded && Input.GetButtonDown(playerJumpButton)){
			StartCoroutine(Jump());
		}

		if(!characterController.isGrounded){
			playerController.IsGrounded = false;
			gravityDirection.y -= m_playerGravity * Time.deltaTime;
			characterController.Move(gravityDirection * m_playerGravity);
			if(characterController.isGrounded){
				playerController.IsGrounded = true;
			}
		}

	}

	public bool GravityChange(bool isGrounded){
		if(isGrounded){
			gravityDirection.y = -0.1f;
			return isGrounded;
		} else {
			return isGrounded;
		}
	}

	IEnumerator Jump(){
		float timer = 0f;
		Vector3 jumpSpeedTemp = Vector3.up;
		jumpSpeedTemp.y = m_jumpSpeed;
		while(timer < m_jumpApexTime){
			characterController.Move(Vector3.Lerp(jumpSpeedTemp, Vector3.zero, timer / m_jumpApexTime));
			timer += Time.deltaTime;
			yield return null;
		}
		yield return null;
	}

	

}
