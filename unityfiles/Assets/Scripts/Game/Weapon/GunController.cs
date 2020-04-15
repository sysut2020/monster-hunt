using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GunController {
    private readonly Gun gun;
    private readonly BulletData bulletData;
    private WeaponData weaponData;
    private readonly BulletBuffer bulletBuffer;
    private Transform firePoint;

    private readonly WUTimers fireRateTimer = new WUTimers();
    private string timerUID;

    public GunController(Gun gun, BulletBuffer bulletBuffer) {
        this.gun = gun;
        this.WeaponData = gun.WeaponData;
        this.firePoint = gun.FirePoint;
        this.bulletBuffer = bulletBuffer;
        this.timerUID = this.fireRateTimer.RollingUID;
        this.fireRate = WeaponData.FireRate;
        this.fireRateTimer.Set(this.timerUID, this.GetBulletWaitTime(this.WeaponData.FireRate));

        if (!this.gun.Bullet.TryGetComponent(out SpriteRenderer bulletSpriteRender)) {
            throw new MissingComponentException("Bullet sprite missing");
        }


        this.bulletData = new BulletData(
            WeaponData.BulletVelocity, WeaponData.BulletTtl, WeaponData.BulletDamage, bulletSpriteRender.sprite,
            this.gun.Bullet.transform
        );
    }

    public static event EventHandler BulletFireEvent;

    // -- properties -- //
    private float fireRate = 0;

    public float FireRate {
        get { return fireRate; }
        set {
            this.fireRate = value;
            this.fireRateTimer.Update(this.timerUID, this.GetBulletWaitTime(value));
        }
    }

    public WeaponData WeaponData {
        get => weaponData;
        internal set => weaponData = value;
    }

    // -- public -- //

    /// <summary>
    /// Checks if it is time to fire if it is, it wil fire if not not 
    /// </summary>
    public void MaybeFire() {
        // is it time to release a bullet
        if (fireRateTimer.Done(this.timerUID, true)) {
            this.Fire();
        }
    }


    // -- private -- //


    /// <summary>
    /// recives a game objet orients it and fires it
    /// </summary>
    /// <param name="bullet">the GO to fire</param>
    private void FireProjectile(GameObject bullet) {
        bullet.transform.rotation = Quaternion.Euler(
            this.firePoint.transform.rotation.eulerAngles.x,
            this.firePoint.transform.rotation.eulerAngles.y,
            this.firePoint.transform.rotation.eulerAngles.z +
            Random.Range(-this.WeaponData.BulletSpread, this.WeaponData.BulletSpread)
        );
        bullet.transform.position = this.firePoint.transform.position;
        bullet.SetActive(true);
    }


    /// <summary>
    /// Calculates and sets the bullet wait time
    /// </summary>
    /// <param name="fireRate"> the new fire rate</param>
    private int GetBulletWaitTime(float fireRate) {
        return (int) Mathf.Floor(1000 / fireRate);
    }


    /// <summary>
    /// Fires a projectile from the gun
    /// </summary>
    private void Fire() {
        GameObject bullet = bulletBuffer.GetBullet();
        BulletControl bulletControl = null;

        if (!bullet.TryGetComponent(out bulletControl)) {
            bulletControl = bullet.AddComponent<BulletControl>() as BulletControl;
        }

        bulletControl.BulletData = this.bulletData;

        this.FireProjectile(bullet);

        BulletFireEvent?.Invoke(this, EventArgs.Empty);
    }
}