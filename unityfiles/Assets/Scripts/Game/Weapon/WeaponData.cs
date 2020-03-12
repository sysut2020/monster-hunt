using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Data", menuName = "ScriptableObjects/WeaponData", order = 2)]
public class WeaponData : ScriptableObject {

    // TODO: fill stuff out
    
    [Header("Weapon posission")]
    [Tooltip ("the Angle of the different arm elements")]
    [SerializeField]
    private PLAYER_ANIMATION holdingAnimation; 

    // -- Weapon sprite -- //

    [Tooltip("the scale of the sprite used.")]
    [SerializeField]
    private Sprite weaponSprite;

    [Tooltip("")]
    [SerializeField]
    private Vector3 weaponPosission;

    [Tooltip("")]
    [SerializeField]
    private Vector3 weaponRotation;

    [Tooltip("")]
    [SerializeField]
    private Vector3 weaponScale;


    // -- Fireing point -- //

    [Tooltip("")]
    [SerializeField]
    private Vector3 firePointPosission;


    [Tooltip("")]
    [SerializeField]
    private Vector3 firePointRotation;
    
    

    // -- bullet stuff sprite -- //
    

    [Header("Bullet properties")]
    [Tooltip("velocity of bullet in units/(1/50) sec.")]
    [SerializeField]
    private float bulletVelocity = 0f;

    [Tooltip("Damage inflicted by bullet.")]
    [SerializeField]
    private float bulletDamage = 0;

    [Tooltip("How long the bullet will live if it doesn´t hit anything.")]
    [SerializeField]
    private int bulletTtl = 0;


    [Tooltip("Fire rate in shots/sec.")]
    [SerializeField]
    private float fireRate = 1f;

    [Tooltip("Fire rate in shots/sec.")]
    [SerializeField]
    private float bulletSpread = 0f;

    public PLAYER_ANIMATION HoldingAnimation { get => holdingAnimation; set => holdingAnimation = value; }
    public Sprite WeaponSprite { get => weaponSprite; set => weaponSprite = value; }
    public Vector3 WeaponRotation { get => weaponRotation; set => weaponRotation = value; }
    public Vector3 WeaponPosission { get => weaponPosission; set => weaponPosission = value; }
    public Vector3 WeaponScale { get => weaponScale; set => weaponScale = value; }
    public Vector3 FPRotation { get => firePointRotation; set => firePointRotation = value; }
    public Vector3 FPPosission { get => firePointPosission; set => firePointPosission = value; }
    public float BulletVelocity { get => bulletVelocity; set => bulletVelocity = value; }
    public float BulletDamage { get => bulletDamage; set => bulletDamage = value; }
    public int BulletTtl { get => bulletTtl; set => bulletTtl = value; }
    public float FireRate { get => fireRate; set => fireRate = value; }
    public float BulletSpread { get => bulletSpread; set => bulletSpread = value; }
}