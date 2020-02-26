using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WordChecker
{

    //REMOVE AND READ FROM FILE
    private static string[] words = {"AAB", "ABA", "BOB", "HELLO", "UNCLE"};
    //REMOVE AND READ FROM FILE



    // -- properties -- //
    // -- public -- //



    public bool IsSeriesWord(LgLetter[] series){
        string maybeForwardsWord = "";
        string maybeBackwardsWord = "";
        int seriesLen = series.Length;

        // forward pass
        for (int i = 0; i < seriesLen; i++)
        {
            maybeForwardsWord += series[i].Letter;
        }

        for (int i = seriesLen - 1; i >= 0; i--)
        {
            maybeBackwardsWord += series[i].Letter;
        }

        return isWordValid(maybeBackwardsWord) || isWordValid(maybeForwardsWord);
    }

    // -- private -- // 

    public static bool isWordValid(string w){
        return Array.BinarySearch(words, w) > 0;
    }
}
