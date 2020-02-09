using System.Numerics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using Quaternion = UnityEngine.Quaternion;

public class GunAngleStuff{

    private GameObject FirePoint;
    private Transform RotationPoint;


    // these can mey be tansforms insted of GO
    private GameObject helperRotPoint;
    private GameObject helperGunPoint;
    // -- public -- // 
    public GunAngleStuff(GameObject FP, Transform RP){
        this.FirePoint = FP;
        this.RotationPoint = RP;

        Quaternion tmp = RotationPoint.rotation;
        RotationPoint.rotation = new Quaternion();
        
        helperRotPoint = new GameObject();
        helperGunPoint = new GameObject();

        helperRotPoint.transform.parent   = RotationPoint.transform.parent;
        helperRotPoint.transform.rotation = RotationPoint.rotation;
        helperRotPoint.transform.Rotate(0,0,-90);
        helperRotPoint.transform.position = RotationPoint.position;
        helperRotPoint.name = "helperRotPoint";

        helperGunPoint.transform.parent   = helperRotPoint.transform;        
        helperGunPoint.transform.rotation = FirePoint.transform.rotation;
        helperGunPoint.transform.position = FirePoint.transform.position;
        helperGunPoint.name = "helperGunPoint";

        helperRotPoint.transform.rotation = tmp;
        RotationPoint.transform.rotation = tmp;
    }
    
    public Vector3 GetAngle(Vector3 mousePoint){
        Vector3 v_RP_MP = mousePoint - RotationPoint.transform.position;
        float z_angle = this.RadianToDegree(Math.Atan2(v_RP_MP.y, v_RP_MP.x));        
        bool isFlipped = (z_angle < 90 && z_angle > -90)? false : true;
        Vector3 angle = (isFlipped) ? new Vector3(0,0,z_angle): new Vector3(180,0,-z_angle);
        helperRotPoint.transform.rotation = Quaternion.Euler(angle);
        Vector3 v_GP_MP = mousePoint- helperGunPoint.transform.position;
        Vector3 norm_GP = helperGunPoint.transform.right;
        Vector3 norm_GP_Prj_GPMP = Vector3.Project(v_GP_MP, norm_GP);
        Vector3 v_RP_GP_Prj_GPMP = (mousePoint+(norm_GP_Prj_GPMP - v_GP_MP)) - helperRotPoint.transform.position;
        z_angle = this.RadianToDegree(Math.Atan2(v_RP_GP_Prj_GPMP.y, v_RP_GP_Prj_GPMP.x));

        if (true){
            Debug.DrawLine(Vector3.zero, norm_GP, Color.red);
            Debug.DrawLine(Vector3.zero, v_GP_MP, Color.green);
            Debug.DrawLine(Vector3.zero, Vector3.Project(v_GP_MP, norm_GP), Color.yellow);
            Debug.DrawLine(Vector3.zero, norm_GP_Prj_GPMP - v_GP_MP, Color.blue);  
            Debug.DrawLine(RotationPoint.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Color.magenta);
            Debug.DrawRay(FirePoint.transform.position, FirePoint.transform.right*100, Color.cyan);
        }
        

        return (isFlipped) ? new Vector3(180,0,-z_angle) : new Vector3(0,0,z_angle);
    }

    // -- private -- //
    private double DegreeToRadian(double angle){
        return (Math.PI * angle / 180.0); 
    }

    private float RadianToDegree(double angle){
        return (float)( angle * (180.0 / Math.PI));
    }


}
