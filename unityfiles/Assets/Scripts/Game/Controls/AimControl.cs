using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using Quaternion = UnityEngine.Quaternion;

/// <summary>
/// Calulcates the angle between an objects aim point and the rotate
/// point where the rotation of aiming is applied, and calculates an offset, so
/// we can get the correct rotation angle.
/// </summary>
public class AimControl {

    /// <summary>
    /// The point where we want the aiming to be tracked
    /// </summary>
    private readonly GameObject FirePoint;

    /// <summary>
    /// The point where the rotation is applied
    /// </summary>
    private readonly Transform RotationPoint;

    private GameObject helperRotPoint;
    private GameObject helperAimPoint;

    private bool debug = true;

    public AimControl(GameObject firePoint, Transform rotatePoint) {
        this.FirePoint = firePoint;
        this.RotationPoint = rotatePoint;
        this.CreateHelperObjects();
    }

    /// <summary>
    /// Creates helper game objects for calculating the angle
    /// </summary>
    private void CreateHelperObjects() {
        Quaternion tmp = RotationPoint.rotation;
        RotationPoint.rotation = new Quaternion();

        helperRotPoint = new GameObject();
        helperAimPoint = new GameObject();

        helperRotPoint.transform.parent = RotationPoint.transform.parent;
        helperRotPoint.transform.rotation = RotationPoint.rotation;
        helperRotPoint.transform.Rotate(0, 0, -90);
        helperRotPoint.transform.position = RotationPoint.position;
        helperRotPoint.name = "Aim control - helper rotationpoint";

        helperAimPoint.transform.parent = helperRotPoint.transform;
        helperAimPoint.transform.rotation = FirePoint.transform.rotation;
        helperAimPoint.transform.position = FirePoint.transform.position;
        helperAimPoint.name = "hAim control- helper aim point";

        helperRotPoint.transform.rotation = tmp;
        RotationPoint.transform.rotation = tmp;
    }

    /// <summary>
    /// Returns the angle which points towards the provided mouse point
    /// </summary>
    /// <param name="mousePoint">the point to aim at</param>
    /// <returns>angle that points towars mousepoint</returns>
    public Vector3 GetAngle(Vector3 mousePoint) {
        Vector3 v_RP_MP = mousePoint - RotationPoint.transform.position;
        float z_angle = this.RadianToDegree(Math.Atan2(v_RP_MP.y, v_RP_MP.x));
        bool isFlipped = (z_angle < 90 && z_angle > -90) ? false : true;
        Vector3 angle = (isFlipped) ? new Vector3(180, 0, -z_angle): new Vector3(0, 0, z_angle);
        helperRotPoint.transform.rotation = Quaternion.Euler(angle);


        Vector3 v_GP_MP = mousePoint - helperAimPoint.transform.position;
        Vector3 norm_GP = helperAimPoint.transform.right;
        Vector3 norm_GP_Prj_GPMP = Vector3.Project(v_GP_MP, norm_GP);

        Vector3 norm_RPMP_Prj_GPMP = Vector3.Project(v_GP_MP, v_RP_MP.normalized);
        Vector3 v_RP_GP_Prj_GPMP = (mousePoint - (norm_GP_Prj_GPMP - v_GP_MP)) - helperRotPoint.transform.position;
        
        z_angle = this.RadianToDegree(Math.Atan2(v_RP_GP_Prj_GPMP.y, v_RP_GP_Prj_GPMP.x));

        //z_angle = this.RadianToDegree(Math.Atan2(norm_GP_Prj_GPMP.y, norm_GP_Prj_GPMP.x));
        //Debug.Log($"DEBUG ZANG {z_angle}");
        //float additional = this.RadianToDegree(Math.Acos((new Vector2(v_GP_MP.x, v_GP_MP.y).magnitude/new Vector2(norm_GP_Prj_GPMP.x, norm_GP_Prj_GPMP.y).magnitude) % 1) );
        //Debug.Log($"DEBUG additional {additional}");

        // Debug.Log($"DEBUG Hosss {v_GP_MP.magnitude}");
        // Debug.Log($"DEBUG A {v_GP_MP.x} {v_GP_MP.y} {v_GP_MP.z}");

        // Debug.Log($"DEBUG mot {norm_GP_Prj_GPMP.magnitude}");
        // Debug.Log($"DEBUG B {norm_GP_Prj_GPMP.x} {norm_GP_Prj_GPMP.y} {norm_GP_Prj_GPMP.z}");
        

        Debug.Log($"DEBUG ZANG2 {z_angle}");

        if (debug) {
            Debug.DrawLine(Vector3.zero, norm_GP_Prj_GPMP, Color.red);
            Debug.DrawLine(Vector3.zero, v_GP_MP, Color.green);
            // Debug.DrawLine(Vector3.zero, Vector3.Project(v_GP_MP, norm_GP), Color.yellow);
            // Debug.DrawLine(mousePoint,mousePoint + (norm_GP_Prj_GPMP - v_GP_MP), Color.blue);
            Debug.DrawLine(RotationPoint.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Color.magenta);

            Debug.DrawRay(FirePoint.transform.position, FirePoint.transform.right * 100, Color.cyan);
            //Debug.DrawRay(FirePoint.transform.position, helperAimPoint.transform.right * 100, Color.white);
        }

        return (isFlipped) ? new Vector3(180, 0, -z_angle) : new Vector3(0, 0, z_angle);

    }

    /// <summary>
    /// Enables debugging
    /// </summary>
    /// <param name="debug">true to enable</param>
    public void ToggleDebug(bool debug) {
        this.debug = debug;
    }

    private double DegreeToRadian(double angle) {
        return (Math.PI * angle / 180.0);
    }

    private float RadianToDegree(double angle) {
        return (float) (angle * (180.0 / Math.PI));
    }

}