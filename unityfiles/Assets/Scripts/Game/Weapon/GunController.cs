using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
TODO: implement support for scriptable objects (in this case bullet types)

will assume the current structure is 
someGunName(with tag weapon)
    -> (visual gun stuff)
    -> Gun object with name gun

*/

public class GunController : MonoBehaviour {
    // -- inspector -- //

    [Header("Bullet properties")]
    [Tooltip("velocity of bullet in units/(1/50) sec.")]
    [SerializeField]
    private Vector2 bulletVelocity = Vector2.zero;

    [Tooltip("Damage inflicted by bullet.")]
    [SerializeField]
    private float bulletDamage = 0;

    [Tooltip("How long the bullet will live if it doesn´t hit anything.")]
    [SerializeField]
    private float bulletTtl = 0;

    [Tooltip("2d texture for bullet sprite.")]
    [SerializeField]
    private Texture2D bulletTexture = null;

    [Tooltip("the scale of the sprite used.")]
    [SerializeField]
    private Vector2 spriteScale;

    [Tooltip("Fire rate in shots/sec.")]
    [SerializeField]
    private float fireRate = 1f;

    // -- properties -- //

    public float FireRate {
        get { return fireRate; }
        set {
            this.fireRate = value;
            this.SetBulletWaitTime(value);
            this.fireRateTimer.Update(this.timerUID, this.bulletWaitTime);
        }
    }

    // -- internal -- //
    private readonly Timers fireRateTimer = new Timers();
    private ArrayList activeBullets = new ArrayList();
    private ArrayList idleBullets = new ArrayList();

    [SerializeField]
    private bool isFiring;

    [SerializeField]
    private GameObject firePoint;

    private float bulletWaitTime;

    private string timerUID;

    private GameObject blueprintBullet;

    // -- public -- //
    /// <summary>
    /// Starts releasing bullets as fast as the 
    /// fire rate allows
    /// </summary>
    public void StartFiring() {
        this.isFiring = true;
    }

    /// <summary>
    /// stops the firing
    /// </summary>
    public void StopFiring() {
        this.isFiring = false;
    }

    /// <summary>
    /// Fires a bullet if able (not on cooldown since last shot)
    /// </summary>
    public void FireOnce() {
        this.MaybeFire();
    }

    // -- private -- //

    /// <summary>
    /// generates a new bullet and fires it
    /// </summary>
    private void FireNewProjectile() {
        GameObject bulletCopy = Instantiate(this.blueprintBullet);
        bulletCopy.transform.rotation = this.firePoint.transform.rotation;
        bulletCopy.transform.position = this.firePoint.transform.position;
        bulletCopy.SetActive(true);

        this.activeBullets.Add(bulletCopy);
    }

    /// <summary>
    /// reuses one of the inactive bullets and fires it
    /// </summary>
    private void FireExistingProjectile() {
        GameObject bulletCopy = (GameObject) this.idleBullets[0];
        this.idleBullets.Remove(bulletCopy);

        bulletCopy.transform.rotation = this.firePoint.transform.rotation;
        bulletCopy.transform.position = this.firePoint.transform.position;
        bulletCopy.SetActive(true);

        this.activeBullets.Add(bulletCopy);
    }

    /// <summary>
    /// generates a bullet blueprint for the 
    /// </summary>
    private void GenerateBulletBlueprint() {
        GameObject bullet = new GameObject();
        this.blueprintBullet = bullet;
        bullet.name = "Bullet";

        SpriteRenderer spriteRender = bullet.AddComponent<SpriteRenderer>() as SpriteRenderer;
        spriteRender.sprite = Sprite.Create(this.bulletTexture,
            new Rect(0, 0, this.bulletTexture.width, this.bulletTexture.height), Vector2.zero);
        bullet.transform.localScale = this.spriteScale;

        BoxCollider2D boxCol = bullet.AddComponent<BoxCollider2D>() as BoxCollider2D;
        boxCol.isTrigger = true;

        BulletControl bulletControl = bullet.AddComponent<BulletControl>() as BulletControl;

        bulletControl.Velocity = this.bulletVelocity;
        bulletControl.Damage = this.bulletDamage;
        bulletControl.BulletTimeToLive = this.bulletTtl;

        // for hit detection
        Rigidbody2D rigidB2d = bullet.AddComponent<Rigidbody2D>() as Rigidbody2D;
        rigidB2d.bodyType = RigidbodyType2D.Kinematic;

        bullet.SetActive(false);
    }

    /// <summary>
    /// Calculates and sets the bullet wait time
    /// </summary>
    /// <param name="fireRate"> the new fire rate</param>
    private void SetBulletWaitTime(float fireRate) {
        this.bulletWaitTime = 1000 / fireRate;
    }

    /// <summary>
    /// Checks if it is time to fire if it is, it wil fire if not not 
    /// </summary>
    private void MaybeFire() {
        // is it time to release a bullet
        if (fireRateTimer.Done(this.timerUID, true)) {
            this.Fire();

        }
    }

    /// <summary>
    /// Fires a projectile from the gun
    /// </summary>
    private void Fire() {
        // if yes is there any bullets in the active bullet list that is inactive
        for (int i = this.activeBullets.Count; i > 0; i--) {
            GameObject g = (GameObject) this.activeBullets[i - 1];
            //TODO: probably shit solution somone try somthing better
            if (!g.activeInHierarchy) {
                //if there is move them to the inactive list
                this.activeBullets.Remove(g);
                this.idleBullets.Add(g);
            }
        }

        // if there are any inactive bullets fire them if not make a new one
        if (this.idleBullets.Count >= 1) {
            this.FireExistingProjectile();
        } else {
            this.FireNewProjectile();
        }
    }
    // -- unity -- //

    // -- unity -- //

    void Start() {
        //TODO: Discuss system for timer naming conventions
        this.SetBulletWaitTime(this.fireRate);
        this.timerUID = this.fireRateTimer.RollingUID;
        this.fireRateTimer.Set(this.timerUID, bulletWaitTime);
        this.GenerateBulletBlueprint();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            this.MaybeFire();
        }
        if (isFiring) {
            this.MaybeFire();
        }
    }

    // -- debug -- //

    /// <summary>
    /// Directly fire a projectile regardless of firerate
    /// meant for debug only
    /// </summary>
    public void DEBUG_FIRE() {
        this.Fire();
    }
}