using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeHitableByBullet : MonoBehaviour {

	[SerializeField]
	private EnemyHealthController affectedHealthController;

	public HealthController AffectedHealthController { get => affectedHealthController; }

	void Awake() {
		if (AffectedHealthController == null) {
			throw new MissingComponentException("enabler missing somthing to enable");
		}
	}
}