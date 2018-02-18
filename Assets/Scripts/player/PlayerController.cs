using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	
	public delegate bool BoolChangeDelegate(bool newVal);
	public event BoolChangeDelegate IsDodgingEvent, IsGroundedEvent, CanMoveEvent, PlayerIsActiveEvent;
	public static event BoolChangeDelegate AllPlayersActiveEvent;

	public Color[] playerColors;

	public bool IsGrounded{
		get {return m_isGrounded;}
		set {
			//Debug.Log("isGrounded being set");
			if(m_isGrounded == value) return;
			m_isGrounded = value;
			m_isJumping = !m_isGrounded;
			if(IsGroundedEvent != null)
				IsGroundedEvent(m_isGrounded);
		}
	}
	public bool IsDodging{
		get {return m_isDodging;}
		set {
			//Debug.Log("isDodging being set");
			if(m_isDodging == value) return;
			m_isDodging = value;
			m_canMove = !m_isDodging;
			if(IsDodgingEvent != null)
				IsDodgingEvent(m_isDodging);
		}
	}
	public bool CanMove{
		get {return m_canMove;}
		set {
			//Debug.Log("canMove being set");
			if(m_canMove == value) return;
			m_canMove = value;
			if(CanMoveEvent != null)
				CanMoveEvent(m_canMove);
		}
	}
	public bool PlayerIsActive{
		get {return m_playerIsActive;}
		set {
			if(m_playerIsActive == value) return;
			m_playerIsActive = value;
			if(PlayerIsActiveEvent != null)
				PlayerIsActiveEvent(m_playerIsActive);
		}
	}

	[SerializeField] private bool m_isDodging;
	[SerializeField] private bool m_isGrounded;
	[SerializeField] private bool m_isJumping;
	[SerializeField] private bool m_canMove;
	public bool m_playerIsActive;
	[SerializeField] private float m_timeGrounded;
	[SerializeField] private float m_timeJumping;

	void Start(){
		m_isDodging = false;
		m_isGrounded = true;
		m_canMove = true;
		m_isJumping = false;
		m_playerIsActive = true;

		PlayerIsActiveEvent += SetPlayerActive;
		AllPlayersActiveEvent += SetPlayerActive;

		StartCoroutine(Timers());
	}

	IEnumerator Timers(){
		while (m_playerIsActive){
			if(m_isGrounded){
				m_timeGrounded += Time.deltaTime;
				m_timeJumping = 0;
			} else if(m_isJumping){
				m_timeJumping += Time.deltaTime;
				m_timeGrounded = 0;
			}
			yield return null;
		}
		yield return null;
	}

	public bool SetPlayerActive(bool state){
		GetComponent<PlayerDodge>().enabled = state;
		GetComponent<PlayerMovement>().enabled = state;
		GetComponent<PlayerJump>().enabled = state;
		return state;
	}


}
