using System;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class BulletData{

    public readonly Guid Guid = Guid.NewGuid();

    private float velocity;
    private int timeToLive;
    private float damage;

    private Sprite sprite;
    private Transform spriteTransform;

    public BulletData(float velocity, int timeToLive, float damage, Sprite sprite, Transform spriteTransform){
        this.Velocity = velocity;
        this.TimeToLive = timeToLive;
        this.Damage = damage;
        this.Sprite = sprite;
        this.SpriteTransform = spriteTransform;
    }

    public float Velocity { get => velocity; internal set => velocity = value; }
    public int TimeToLive { get => timeToLive; internal set => timeToLive = value; }
    public float Damage { get => damage; set => damage = value; }
    public Sprite Sprite { get => sprite; internal set => sprite = value; }
    public Transform SpriteTransform { get => spriteTransform; internal set => spriteTransform = value; }


}