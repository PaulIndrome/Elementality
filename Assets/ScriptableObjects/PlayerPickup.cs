using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerPickup : ScriptableObject {
	public Elements.Element element;

	public virtual void Apply(PlayerElementHolder playerElementHolder){
		playerElementHolder.SlotElement(this);
	}


}
