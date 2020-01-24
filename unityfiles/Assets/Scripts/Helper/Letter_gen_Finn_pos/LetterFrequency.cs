using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// 
/// JSON SERIALIZATION STUFF NO TOUCHY
/// 
[System.Serializable]
public class LetterFrequency
{
    public List<string> letter;
    public List<float> weight;

      


    public string GetLetterByFrequency()
    {
        var r = new System.Random();
        int val = r.Next(1001);

        //Debug.Log(val);


        for (int i = 0; i < weight.Count; i++)
        {
            if (weight[i] >= val)
            {
                return letter[i];
            }
        
        }
        return null;
    }

}