using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerPickup : ScriptableObject {

	public PlayerElement.Element element;

	public abstract void Apply(PlayerElement playerElement);

}
