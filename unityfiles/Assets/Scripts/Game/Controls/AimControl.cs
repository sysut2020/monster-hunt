using System.Xml.Linq;
using System;

using UnityEngine;

using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using Quaternion = UnityEngine.Quaternion;

/// <summary>
/// Calculates the angle between an objects aim point and the rotate
/// point where the rotation of aiming is applied, and calculates an offset, so
/// we can get the correct rotation angle.
/// </summary>
public class AimControl {

    /// <summary>
    /// The point where we want the aiming to be tracked
    /// </summary>
    private GameObject FirePoint;

    /// <summary>
    /// The point where the rotation is applied
    /// </summary>
    private readonly Transform RotationPoint;

    private Vector2 chekVec = Vector2.zero;

    private GameObject helperRotPoint;
    private GameObject helperAimPoint;

    private bool debug = true;

    public AimControl(GameObject firePoint, Transform rotatePoint) {
        this.FirePoint = firePoint;
        this.RotationPoint = rotatePoint;
        this.CreateHelperObjects();
        this.SubscribeToEvents();
    }

    // -- events -- //    


    /// <summary>
    /// Subscribes to the relevant events for this class
    /// </summary>
    private void SubscribeToEvents() {
        PlayerWeaponController.WeaponChangedEvent += c_WeaponChangedEvent;
           
    }
    
    /// <summary>
    /// Subscribes to the relevant events for this class
    /// </summary>
    private void UnsubscribeFromEvents() {
        PlayerWeaponController.WeaponChangedEvent -= c_WeaponChangedEvent;
        
    }

    private void c_WeaponChangedEvent(object _, WeaponChangedEventArgs args){
        CreateHelperObjects();
    }

    /// <summary>
    /// Creates helper game objects for calculating the angle
    /// </summary>
    private void CreateHelperObjects() {
        Quaternion tmp = RotationPoint.rotation;
        RotationPoint.rotation = new Quaternion();



        if (helperRotPoint == null){
            helperRotPoint = new GameObject();
            helperRotPoint.transform.parent = RotationPoint.transform.parent;
            //helperRotPoint.transform.localScale = Vector3.zero;
            helperRotPoint.name = "Aim control - helper rotationpoint";
        }
        if (helperAimPoint == null){
            helperAimPoint = new GameObject();
            helperAimPoint.transform.parent = helperRotPoint.transform;
            helperAimPoint.name = "hAim control- helper aim point";
        } 




        helperRotPoint.transform.rotation = RotationPoint.rotation;
        helperRotPoint.transform.Rotate(0, 0, 0);
        helperRotPoint.transform.position = RotationPoint.position;
        helperRotPoint.name = "Aim control - helper rotationpoint";

        helperAimPoint.transform.rotation = FirePoint.transform.rotation;
        helperAimPoint.transform.position = FirePoint.transform.position;
        helperAimPoint.name = "hAim control- helper aim point";

        helperRotPoint.transform.rotation = tmp;
        RotationPoint.transform.rotation = tmp;




              

        
        // helperRotPoint.transform.localRotation = RotationPoint.localRotation;
        // helperRotPoint.transform.localPosition = RotationPoint.localPosition;

        // helperAimPoint.transform.localRotation = FirePoint.transform.localRotation;
        // helperAimPoint.transform.localPosition = FirePoint.transform.localPosition;
        
        //helperAimPoint.transform.rotation = Quaternion.Euler(helperRotPoint.transform.InverseTransformDirection(FirePoint.transform.rotation.eulerAngles));
        //helperAimPoint.transform.localPosition = helperRotPoint.transform.InverseTransformPoint(FirePoint.transform.position);
        
    }

    /// <summary>
    /// Returns the angle which points towards the provided mouse point
    /// </summary>
    /// <param name="mousePoint">the point to aim at</param>
    /// <returns>angle that points towars mousepoint</returns>
    public Vector3 GetAngle(Vector3 mousePoint) {
        Vector2 newChekVec = RotationPoint.transform.position-FirePoint.transform.position;

        if (Mathf.Floor(chekVec.magnitude*10) != Mathf.Floor(newChekVec.magnitude*10)){
            this.chekVec = newChekVec;
            CreateHelperObjects();
        }


        // rotates the helper gun point towards the mouse
        Vector3 v_RP_MP = mousePoint - RotationPoint.transform.position;
        float z_angle = this.RadianToDegree(Math.Atan2(v_RP_MP.y, v_RP_MP.x));
        bool isFlipped = (z_angle < 90 && z_angle > -90) ? false : true;
        Vector3 angle = (isFlipped) ? new Vector3(180, 0, -z_angle): new Vector3(0, 0, z_angle);
        
        helperRotPoint.transform.rotation = Quaternion.Euler(angle);

        // finds the vector projected from the shooting direction on the vector from the helper GP to the MP 
        Vector3 v_GP_MP = mousePoint - helperAimPoint.transform.position;
        Vector3 norm_GP = helperAimPoint.transform.right;
        Vector3 norm_GP_Prj_GPMP = Vector3.Project(v_GP_MP, norm_GP);

        // finds the point where a line from the helper GP to the point found in the block above
        // wold intersect a circle centered at the RP with a radius of the distance between the RP and MP
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

        float xCord = 0;
        float yCord = 0;

        float toSqr = Mathf.Pow(r,2)*Mathf.Pow(dr,2)-Mathf.Pow(D,2);

        // to avoid sqrt(-num) only use the "correct" coordinates for x,y when the squared block is
        // bigger then 0
        if (toSqr > 0){
            float sgnX = (dy <0) ? -1:1;
            float xCord1 = (D*dy + sgnX*dx*Mathf.Sqrt(toSqr))/Mathf.Pow(dr,2);
            float xCord2 = (D*dy - sgnX*dx*Mathf.Sqrt(toSqr))/Mathf.Pow(dr,2);

            float yCord1 = (-D*dx + Mathf.Abs(dy)*Mathf.Sqrt(toSqr))/Mathf.Pow(dr,2);
            float yCord2 = (-D*dx - Mathf.Abs(dy)*Mathf.Sqrt(toSqr))/Mathf.Pow(dr,2);
            
            Vector2 v_mp_pos1 = new Vector2(helperAimPoint.transform.position.x, helperAimPoint.transform.position.y) - new Vector2(xCord1, yCord1);
            Vector2 v_mp_pos2 = new Vector2(helperAimPoint.transform.position.x, helperAimPoint.transform.position.y) - new Vector2(xCord2, yCord2);

            if(debug){
                Debug.DrawLine(helperRotPoint.transform.position, helperRotPoint.transform.position + new Vector3(xCord1, yCord1), Color.red);
                Debug.DrawLine(helperRotPoint.transform.position, helperRotPoint.transform.position + new Vector3(xCord2, yCord2), Color.black);
            }
            

            // if checks which one of the two fond points is closest and uses that one
            // the other point wold be on the other intersection point for the ray on the circle 
            if (yCord1 > yCord2){
                xCord = xCord2;
                yCord = yCord2;
            } else {
                xCord = xCord1;
                yCord = yCord1;
            }

            float ang2 = this.RadianToDegree(Math.Atan2(yCord, xCord));
            float deltaAng =ang2 - z_angle;
            z_angle -= deltaAng;
        } else {
            // use the v2 as the gun angle, technically not correct but it works
            float ang2 = this.RadianToDegree(Math.Atan2(V2.y, V2.x));
            float deltaAng =ang2 - z_angle;
            z_angle -= deltaAng;
        }

        if (debug) {


            

            Debug.DrawLine(helperRotPoint.transform.position,helperRotPoint.transform.position+ v_RP_GP, Color.green);
            Debug.DrawLine(RotationPoint.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Color.magenta);

            Debug.DrawRay(RotationPoint.transform.position, RotationPoint.transform.right * 100, Color.gray);
            Debug.DrawRay(FirePoint.transform.position, FirePoint.transform.right * 100, Color.cyan);
            Debug.DrawRay(FirePoint.transform.position, helperAimPoint.transform.right * 100, Color.white);
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

    /// <summary>
    /// Converts degrees to radians
    /// </summary>
    /// <param name="angle">the angle to convert</param>
    /// <returns>the angle in radians</returns>
    private double DegreeToRadian(double angle) {
        return (Math.PI * angle / 180.0);
    }

    /// <summary>
    /// Converts radians to degrees
    /// </summary>
    /// <param name="angle">the angle to convert</param>
    /// <returns>the angle in degrees</returns>
    private float RadianToDegree(double angle) {
        return (float) (angle * (180.0 / Math.PI));
    }

}