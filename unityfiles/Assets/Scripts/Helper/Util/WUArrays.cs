using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

using UnityEngine;


static class WUArrays
{
    // string array functions

    // high speed functions


    /// <summary>
    /// Returns true if all elements in
    /// the sorted array A is in the sorted array B
    /// </summary>
    /// <param name="A">Sorted array of strings</param>
    /// <param name="B">Sorted array of strings</param>
    /// <returns> Returns true if all elements in array A is in array B  </returns>
    public static bool IsASubsetB(string[] A, string[] B)
    {
        bool isSubset = false;
        
        int aIndex = 0;
        int bIndex = 0;

        int aLen = A.Count();
        int bLen = B.Count();

        
        while (true)
        {
            if (A[aIndex].Equals(B[bIndex]))
            {
                // Debug.Log($"MATCH {B[bIndex]} - {A[aIndex]} value {B[bIndex] == A[aIndex]}");
                aIndex ++;
                bIndex ++;
            }
            // print(string.Compare("A","B")); = -1
            // print(string.Compare("B","A")); = 1
            else if(string.Compare(A[aIndex],B[bIndex]) < 0)
            {
                // Debug.Log($"KILLED {A[aIndex]} - {B[bIndex]}");
                break;
            }
            else
            {
                bIndex ++;
            }

            if(aIndex == aLen)
            {
                isSubset = true;
                break;
            }
            if (bIndex >= bLen)
            {
                isSubset = false;
                break;
            }
        }

        
        



        return isSubset;
    }

   
    public static List<string> RemoveAllAFromB(string[] A, string[] B)
    {
        List<String> returnValues = new List<string>();

        int aIndex = 0;
        int bIndex = 0;

        int aLen = A.Count();
        int bLen = B.Count();


        while (bIndex < bLen)
        {
            if (!A[aIndex].Equals(B[bIndex]))
            {
                if(string.Compare(A[aIndex],B[bIndex]) < 0)
                {
                    aIndex ++;
                }
                else
                {
                    returnValues.Add(B[bIndex]);
                    bIndex ++;
                }
            }
            else
            {
                bIndex ++;
                aIndex ++;
            }
            // print(string.Compare("A","B")); = -1
            // print(string.Compare("B","A")); = 1
                

            if(aIndex >= aLen)
            {
                break;
            }
        }

        if (bIndex <= bLen -1)
        {
            for (int i = bIndex; i < bLen; i++)
            {
                returnValues.Add(B[i]);
            }
        }

        return returnValues;
    }
    
}

