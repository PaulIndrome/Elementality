using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerElementEffect : PlayerAction {
	public Elements.Element element;
	protected PlayerPickup pickup;
	
	public abstract void Activate(PlayerPickup pickup);

	public abstract void Interrupt();
}
