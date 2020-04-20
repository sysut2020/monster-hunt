using UnityEngine;

/// <summary>
/// Data holder for a weapon. All the different data must be set in the unity editor, in the inspector.
/// Also creates a shortcut in unity for creating new weapon data objects.
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WeaponData", order = 2)]
public class WeaponData : ScriptableObject {

    [Header("Weapon position")]
    [Tooltip("the Angle of the different arm elements")]
    [SerializeField]
    private PLAYER_ANIMATION holdingAnimation;

    [Header("Bullet properties")]
    [Tooltip("velocity of bullet in units/(1/50) sec.")]
    [SerializeField]
    private float bulletVelocity = 0f;

    [Tooltip("Damage inflicted by bullet.")]
    [SerializeField]
    private float bulletDamage = 0;

    /// <summary>
    /// Time in seconds 
    /// </summary>
    [Tooltip("How long the bullet will live in seconds if it doesnt hit anything.")]
    [SerializeField]
    private int bulletTtl = 0;

    [Tooltip("Fire rate in shots/sec.")]
    [SerializeField]
    private float fireRate = 1f;

    /// <summary>
    /// The spread of the bullet in degrees.
    /// A spread of eg. 20 degrees will add 20 degrees above and below the middle line resulting a total of 40 degree spread
    /// </summary>
    [Tooltip("Set the spread of the bullet")]
    [SerializeField]
    private float bulletSpread = 0f;

    public PLAYER_ANIMATION HoldingAnimation { get => holdingAnimation; set => holdingAnimation = value; }
    public float BulletVelocity { get => bulletVelocity; set => bulletVelocity = value; }
    public float BulletDamage { get => bulletDamage; set => bulletDamage = value; }
    public int BulletTtl { get => bulletTtl; set => bulletTtl = value; }
    public float FireRate { get => fireRate; set => fireRate = value; }
    public float BulletSpread { get => bulletSpread; set => bulletSpread = value; }
}