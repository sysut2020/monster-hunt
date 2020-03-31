using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Place on a game object To make it survive over scene changes
/// </summary>
public class DontDestroyOnLoadGO : MonoBehaviour{

    private static GameObject Instance;

    void Start(){
        if(DontDestroyOnLoadGO.Instance != this){
            GameObject.Destroy(this);   
        }else{
            DontDestroyOnLoadGO.Instance = this.gameObject;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
