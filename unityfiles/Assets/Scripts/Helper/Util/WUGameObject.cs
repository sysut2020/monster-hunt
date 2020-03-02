using System.Collections.Generic;
using UnityEngine;

static class WUGameObjects{


    
    /// <summary>
    /// Get's a list off all the children of the provided game object
    /// </summary>
    /// <param name="root"> the GO to find the children from</param>
    /// <returns>A list of the Game objects children</returns>
    public static List<GameObject> GetGOChildren(GameObject root)
    {
        List<GameObject> children = new List<GameObject>();

        int numSpawnpoints = root.transform.childCount;

        for (int i = 0; i < numSpawnpoints; i++)
        {
            children.Add(root.transform.GetChild(i).gameObject);
        }   
        
        return children;
    }

    /// <summary>
    /// Get's a list off all the children of the provided game object
    /// whos name equal the provided string
    /// </summary>
    /// <param name="root">the root node to sertch thrugh the children</param>
    /// <param name="nameLike">the name to find</param>
    /// <returns></returns>
    public static List<GameObject> GetChildWithNameLike(GameObject root, string nameLike){
        List<GameObject> children = new List<GameObject>();

        int numSpawnpoints = root.transform.childCount;

        for (int i = 0; i < numSpawnpoints; i++)
        {
            GameObject child = root.transform.GetChild(i).gameObject;
            if (child.name.Equals(nameLike)){
                children.Add(child);
            }
        }   
        
        return children;
    }


    /// <summary>
    /// Get's a list off all the children of the provided game object
    /// whos tag equal the provided string 
    /// </summary>
    /// <param name="root">the root node to sertch thrugh the children</param>
    /// <param name="tagLike">the tag to find</param>
    /// <returns></returns>
    public static List<GameObject> GetChildWithTagLike(GameObject root, string tagLike){
        List<GameObject> children = new List<GameObject>();

        int numSpawnpoints = root.transform.childCount;

        for (int i = 0; i < numSpawnpoints; i++)
        {
            GameObject child = root.transform.GetChild(i).gameObject;
            if (child.tag.Equals(tagLike)){
                children.Add(child);
            }
        }   
        
        return children;
    }
    
    

}
