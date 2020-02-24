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

    // TODO; kan man serialize for get\set uten at det havner i feltet på siden
    [SerializeField]
    private Vector2 bulletVelocity = Vector2.zero;

    [SerializeField]
    private float bulletDamage = 0;

    [SerializeField]
    private float bulletTtl = 5;

    [SerializeField]
    private Sprite bulletSprite = null;

    [SerializeField]
    private Vector2 spriteScale = new Vector2(1, 1);

    [SerializeField]
    private float fireRate = 1f;
    [SerializeField]
    private float bulletSpread = 0f;

    [SerializeField]
    private GameObject firePoint;

    // -- properties -- //

    public float FireRate {
        get { return fireRate; }
        set {
            this.fireRate = value;
            this.SetBulletWaitTime(value);
            
            bool suc = this.fireRateTimer.Update(this.timerUID, this.bulletWaitTime);
        }
    }

    public Vector2 BulletVelocity { get => bulletVelocity; set => bulletVelocity = value; }
    public float BulletDamage { get => bulletDamage; set => bulletDamage = value; }
    public float BulletTtl { get => bulletTtl; set => bulletTtl = value; }
    public Sprite BulletSprite { get => bulletSprite; set => bulletSprite = value; }
    public Vector2 SpriteScale { get => spriteScale; set => spriteScale = value; }
    public GameObject FirePoint { get => firePoint; set => firePoint = value; }
    public float BulletSpread { get => bulletSpread; set => bulletSpread = value; }

    // -- internal -- //
    private readonly Timers fireRateTimer = new Timers();
    private ArrayList activeBullets = new ArrayList();
    private ArrayList idleBullets = new ArrayList();

    [SerializeField]
    private bool isFiring;

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
        this.FireProjectile(bulletCopy);
    }

    /// <summary>
    /// reuses one of the inactive bullets and fires it
    /// </summary>
    private void FireExistingProjectile() {
        GameObject bulletCopy = (GameObject) this.idleBullets[0];
        this.idleBullets.Remove(bulletCopy);
        this.FireProjectile(bulletCopy);
        
    }

    /// <summary>
    /// recives a game objet orients it and fires it
    /// </summary>
    /// <param name="bullet">the GO to fire</param>
    private void FireProjectile(GameObject bullet){
        bullet.transform.rotation = Quaternion.Euler( 
            this.FirePoint.transform.rotation.eulerAngles.x,
            this.FirePoint.transform.rotation.eulerAngles.y,
            this.FirePoint.transform.rotation.eulerAngles.z + Random.Range(-this.BulletSpread, this.BulletSpread)
        );
        bullet.transform.position = this.FirePoint.transform.position;
        bullet.SetActive(true);

        this.activeBullets.Add(bullet);
    }

    /// <summary>
    /// generates a bullet blueprint for the 
    /// </summary>
    public void GenerateBulletBlueprint() {
        GameObject bullet = new GameObject();
        this.blueprintBullet = bullet;
        bullet.name = "Bullet";

        SpriteRenderer spriteRender = bullet.AddComponent<SpriteRenderer>() as SpriteRenderer;
        spriteRender.sprite = this.BulletSprite;
        bullet.transform.localScale = this.SpriteScale;

        BoxCollider2D boxCol = bullet.AddComponent<BoxCollider2D>() as BoxCollider2D;
        boxCol.isTrigger = true;

        BulletControl bulletControl = bullet.AddComponent<BulletControl>() as BulletControl;

        bulletControl.Velocity = this.BulletVelocity;
        bulletControl.Damage = this.BulletDamage;
        bulletControl.BulletTimeToLive = this.BulletTtl;

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

    void Awake() {
        //TODO: Discuss system for timer naming conventions
        this.SetBulletWaitTime(this.FireRate);
        this.timerUID = this.fireRateTimer.RollingUID;
        this.fireRateTimer.Set(this.timerUID, bulletWaitTime);
    }

    void Update() {
        // todo: This meens the fire rate is capped by the framerate this may be a non issue
        if (isFiring) {
            this.MaybeFire();
        }
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy() {
        foreach (GameObject GO in activeBullets) { Destroy(GO); }
        foreach (GameObject GO in idleBullets) { Destroy(GO); }
        Destroy(blueprintBullet);
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