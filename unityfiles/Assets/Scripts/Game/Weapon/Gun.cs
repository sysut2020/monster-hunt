using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A dataholder holding a guns data
/// </summary>
public class Gun : MonoBehaviour {

	[SerializeField]
	[Tooltip("The guns weapon data.")]
	private WeaponData weaponData = null;

	[SerializeField]
	[Tooltip("The fire points transform.")]
	private Transform firePoint = null;

	[SerializeField]
	[Tooltip("The bullet GO.")]
	private GameObject bullet = null;

	public WeaponData WeaponData {
		get {
			if (weaponData == null) {
				throw new MissingComponentException("Gun weapon data missing");
			}
			return weaponData;
		}
	}
	public Transform FirePoint {
		get {
			if (firePoint == null) {
				throw new MissingComponentException("Gun fire point missing");
			}
			return firePoint;
		}
	}

	public GameObject Bullet {
		get {
			if (bullet == null) {
				throw new MissingComponentException("Gun bullet missing");
			}
			return bullet;
		}
	}
}