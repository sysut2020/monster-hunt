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

    

    // -- private -- // 

    public static bool isWordValid(string w){
        return Array.BinarySearch(words, w) > 0;
    }
}
