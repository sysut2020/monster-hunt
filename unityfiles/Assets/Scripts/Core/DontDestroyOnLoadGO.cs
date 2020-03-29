using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadGO : MonoBehaviour{

    private static GameObject Instance;

    void Start(){
        if(DontDestroyOnLoadGO.Instance != this){
            GameObject.Destroy(this);   
        }else{
            DontDestroyOnLoadGO.Instance = this.gameObject;
        }
    }
}
