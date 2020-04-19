using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

static class WUArrays {
    // string array functions

    /// <summary>
    /// Returns true if all elements in
    /// the sorted array A is in the sorted array B
    /// </summary>
    /// <param name="A">Sorted array of strings</param>
    /// <param name="B">Sorted array of strings</param>
    /// <returns> Returns true if all elements in array A is in array B  </returns>
    public static bool IsASubsetB (string[] A, string[] B) {
        bool isSubset = false;

        int aIndex = 0;
        int bIndex = 0;

        int aLen = A.Count ();
        int bLen = B.Count ();

        while (true) {
            if (A[aIndex].Equals (B[bIndex])) {
                // Debug.Log($"MATCH {B[bIndex]} - {A[aIndex]} value {B[bIndex] == A[aIndex]}");
                aIndex++;
                bIndex++;
            }
            // print(string.Compare("A","B")); = -1
            // print(string.Compare("B","A")); = 1
            else if (string.Compare (A[aIndex], B[bIndex]) < 0) {
                // Debug.Log($"KILLED {A[aIndex]} - {B[bIndex]}");
                break;
            } else {
                bIndex++;
            }

            if (aIndex == aLen) {
                isSubset = true;
                break;
            }
            if (bIndex >= bLen) {
                isSubset = false;
                break;
            }
        }

        return isSubset;
    }

    /// <summary>
    /// Removes the elements in A from B
    /// both A and b has to be sorted
    /// ie:
    /// A=["A", "B", "D"]  
    /// B=["A", "A", "B", "C", "D", "D"]
    /// 
    /// RemoveAllAFromB(A,B) = ["A", "C", "D"]
    /// 
    /// </summary>
    /// <param name="A">A list of the strings to remove from B</param>
    /// <param name="B">A list of the strings to remove from</param>
    /// <returns>B with a Removed</returns>
    public static List<string> RemoveAllAFromB (string[] A, string[] B) {
        List<String> returnValues = new List<string> ();

        int aIndex = 0;
        int bIndex = 0;

        int aLen = A.Count ();
        int bLen = B.Count ();

        while (bIndex < bLen) {
            if (!A[aIndex].Equals (B[bIndex])) {
                if (string.Compare (A[aIndex], B[bIndex]) < 0) {
                    aIndex++;
                } else {
                    returnValues.Add (B[bIndex]);
                    bIndex++;
                }
            } else {
                bIndex++;
                aIndex++;
            }
            // print(string.Compare("A","B")); = -1
            // print(string.Compare("B","A")); = 1

            if (aIndex >= aLen) {
                break;
            }
        }

        if (bIndex <= bLen - 1) {
            for (int i = bIndex; i < bLen; i++) {
                returnValues.Add (B[i]);
            }
        }

        return returnValues;
    }

    /// <summary>
    /// Returns the row at the provided y index 
    /// in the the provided 2d array
    /// </summary>
    /// <param name="ar">the array to get the type from</param>
    /// <param name="y">the y index of the row</param>
    /// <typeparam name="T">the type of the array</typeparam>
    /// <returns>the row at the given index</returns>
    public static T[] GetRow<T> (T[, ] ar, int y) {
        if (y >= ar.GetLength (1)) {
            // if the row index is outside the array throw an exeption
            throw new IndexOutOfRangeException ("Target possision out of range");
        }

        int xSize = ar.GetLength (0);
        T[] ret = new T[xSize];
        for (int i = 0; i < xSize; i++) {
            ret[i] = ar[i, y];
        }
        return ret;
    }

    /// <summary>
    /// Returns the col at the provided x index 
    /// in the the provided 2d array
    /// </summary>
    /// <param name="ar">the array to get the type from</param>
    /// <param name="x">the x index of the col</param>
    /// <typeparam name="T">the type of the array</typeparam>
    /// <returns>the col at the given index</returns>
    public static T[] GetCol<T> (T[, ] ar, int x) {
        if (x > ar.GetLength (0)) {
            // if the col index is outside the array throw an exeption
            throw new IndexOutOfRangeException ("Target possision out of range");
        }

        int ySize = ar.GetLength (1);
        T[] ret = new T[ySize];
        for (int i = 0; i < ySize; i++) {
            ret[i] = ar[x, i];
        }
        return ret;
    }

    /// <summary>
    /// Gets all the connected non null values from the
    /// provided x,y coordinates in ether x or y direction
    /// if the starting point is null null is returned
    /// </summary>
    /// <param name="ar">the array </param>
    /// <param name="x">the x cord of the starting point</param>
    /// <param name="y">the y cord of the starting point</param>
    /// <param name="axis">the axis to search 1 is Y axis(row) 0 is X axis(col)</param>
    /// <typeparam name="T">the type of the array</typeparam>
    /// <returns>An array of all the connected elements form the starting point in the given dir</returns>
    public static T[] GetConnected<T> (T[, ] ar, int x, int y, int axis) {
        int xSize = ar.GetLength (0);
        int ySize = ar.GetLength (1);

        if (x >= xSize || y >= ySize) {
            // if the targetet pos is outside the array throw an exception
            throw new IndexOutOfRangeException ("Target possision out of range");
        } else if (EqualityComparer<T>.Default.Equals (ar[x, y], default (T))) {
            // if the target pos is null return null
            return null;
        }

        int lowerBound;
        int upperBound;
        int startIndex;
        T[] valuesList;

        if (axis == 1) // Y
        {
            valuesList = WUArrays.GetCol (ar, x);
            startIndex = y;
        } else // X
        {
            valuesList = WUArrays.GetRow (ar, y);
            startIndex = x;
        }

        lowerBound = valuesList.GetLowerBound (0);
        upperBound = valuesList.GetUpperBound (0);

        // find upper bound 
        for (int i = startIndex; i <= upperBound; i++) {
            if (EqualityComparer<T>.Default.Equals (valuesList[i], default (T))) {
                upperBound = i - 1;
                break;
            }
        }

        // find lower bound 
        for (int i = startIndex; i >= lowerBound; i--) {
            if (EqualityComparer<T>.Default.Equals (valuesList[i], default (T))) {
                lowerBound = i + 1;
                break;
            }
        }

        int diff = upperBound - lowerBound;
        T[] ret = new T[diff + 1];

        for (int i = 0; i <= diff; i++) {
            ret[i] = valuesList[lowerBound + i];
        }

        return ret;
    }

    /// <summary>
    /// prints the multi dimensional array
    /// </summary>
    /// <param name="ar">the array to print</param>
    /// <typeparam name="T">the type of the array</typeparam>
    public static void PrintMultiDim<T> (T[, ] ar) {

        string p = "";
        for (int yDim = ar.GetLowerBound (1); yDim <= ar.GetUpperBound (1); yDim++) {
            for (int xDim = ar.GetLowerBound (0); xDim <= ar.GetUpperBound (0); xDim++) {
                p += ar[xDim, yDim] + ", ";
            }
            p += "\n";

        }
        Debug.Log (p);
    }

    /// <summary>
    /// returns the first occurrence of the search object in the
    /// provided array
    /// </summary>
    /// <param name="ar">the array to search through</param>
    /// <param name="sertchObj">the objet to find</param>
    /// <typeparam name="T">the type of the object and array</typeparam>
    /// <returns>the found object if found else the default val for the type T</returns>
    public static T MultiDimFind<T> (T[, ] ar, T searchObj) {
        T ret = default;
        if (ar == null || EqualityComparer<T>.Default.Equals (searchObj, default (T))) { return ret; }
        T currentElement = default;
        for (int xDim = ar.GetLowerBound (0); xDim <= ar.GetUpperBound (0); xDim++) {
            for (int yDim = ar.GetLowerBound (1); yDim <= ar.GetUpperBound (1); yDim++) {
                currentElement = ar[xDim, yDim];
                if (currentElement != null) {
                    if (currentElement.Equals (searchObj)) { ret = searchObj; break; }
                }
            }
            if (ret != null) break;
        }

        return ret;
    }
}