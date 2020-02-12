using System.Xml.Linq;
using System;

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

        Vector3 v_RP_GP_Prj_GPMP = (mousePoint - (norm_GP_Prj_GPMP - v_GP_MP)) - helperRotPoint.transform.position;
        
        Vector3 v_RP_GP = helperAimPoint.transform.position - helperRotPoint.transform.position;
        Vector3 V2 = (norm_GP_Prj_GPMP + v_RP_GP);

        float r = new Vector2(v_RP_MP.x,v_RP_MP.y).magnitude;

        float x1 = v_RP_GP.x;
        float y1 = v_RP_GP.y;

        float x2 = V2.x;
        float y2 = V2.y;

        float dx = x2-x1;
        float dy = y2-y1;
        float dr = Mathf.Sqrt(Mathf.Pow(dx,2)+Mathf.Pow(dy,2));

        float D = x1*y2-x2*y1;

        float xcord = 0;
        float ycord = 0;

        float toSqr = Mathf.Pow(r,2)*Mathf.Pow(dr,2)-Mathf.Pow(D,2);

        if (toSqr > 0){
            float sgnX = (dy <0) ? -1:1;
            float xcord1 = (D*dy + sgnX*dx*Mathf.Sqrt(toSqr))/Mathf.Pow(dr,2);
            float xcord2 = (D*dy - sgnX*dx*Mathf.Sqrt(toSqr))/Mathf.Pow(dr,2);

            float ycord1 = (-D*dx + Mathf.Abs(dy)*Mathf.Sqrt(toSqr))/Mathf.Pow(dr,2);
            float ycord2 = (-D*dx - Mathf.Abs(dy)*Mathf.Sqrt(toSqr))/Mathf.Pow(dr,2);
            
            Vector2 v_mp_pos1 = new Vector2(V2.x, V2.y) - new Vector2(xcord1, ycord1);
            Vector2 v_mp_pos2 = new Vector2(V2.x, V2.y) - new Vector2(xcord2, ycord2);

            if (v_mp_pos1.magnitude > v_mp_pos2.magnitude){
                xcord = xcord2;
                ycord = ycord2;
            } else {
                xcord = xcord1;
                ycord = ycord1;
            }

            float ang2 = this.RadianToDegree(Math.Atan2(ycord, xcord));
            float deltaAng =ang2 - z_angle;
            z_angle -= deltaAng;
        } else {
            float ang2 = this.RadianToDegree(Math.Atan2(V2.y, V2.x));
            float deltaAng =ang2 - z_angle;
            z_angle -= deltaAng;
        }

        if (debug) {
            Debug.Log($"Z ANG     {z_angle}");
            Debug.Log($"XCORD   {xcord}  YCORD   {ycord}");
            Debug.Log($"the math   {Mathf.Pow(r,2)*Mathf.Pow(dr,2)-Mathf.Pow(D,2)}");
            //Debug.Log($"MOUSE  X    {mousePoint.x - helperRotPoint.transform.position.x}  Y   {mousePoint.y -helperRotPoint.transform.position.y}");

            Debug.DrawLine(helperRotPoint.transform.position,helperRotPoint.transform.position+ V2, Color.red);
            Debug.DrawLine(Vector3.zero, V2, Color.red);

            Debug.DrawLine(helperRotPoint.transform.position,helperRotPoint.transform.position+ v_RP_GP, Color.green);
            //Debug.DrawLine(helperRotPoint.transform.position,helperRotPoint.transform.position+ new Vector3(xcord1,ycord,0), Color.yellow);
            Debug.DrawLine(helperRotPoint.transform.position,helperRotPoint.transform.position+v_RP_GP_Prj_GPMP, Color.blue);
            // Debug.DrawLine(mousePoint,mousePoint + (norm_GP_Prj_GPMP - v_GP_MP), Color.blue);
            Debug.DrawLine(RotationPoint.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Color.magenta);

            Debug.DrawRay(RotationPoint.transform.position, RotationPoint.transform.right * 100, Color.gray);
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