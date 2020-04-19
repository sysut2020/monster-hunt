using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a killable object.
/// </summary>
public interface IKillable {

	/// <summary>
	/// Notify the component that it is dead.
	/// </summary>
	void IsDead();
}