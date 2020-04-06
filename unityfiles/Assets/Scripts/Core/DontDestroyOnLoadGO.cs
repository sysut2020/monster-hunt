using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Place on a game object To make it survive over scene changes
/// </summary>
public class DontDestroyOnLoadGO : MonoBehaviour{

    public static GameObject Instance;

    void Start(){
        if(DontDestroyOnLoadGO.Instance != this.gameObject && DontDestroyOnLoadGO.Instance != null){
            GameObject.Destroy(this.gameObject);   
        }else{
            DontDestroyOnLoadGO.Instance = this.gameObject;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
