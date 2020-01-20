using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LetterFreq
{
    public List<string> letter;
    public List<float> weight;

      


    public string GetLetterByFreq()
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