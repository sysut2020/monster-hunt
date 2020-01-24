using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/*
    TODO:
        - mulig lurt og kjør på en bakgrunstråd og cashe opp en del enheta der som kan brukes
        - mulig kan optimaliser litt snitte atm på 2 ms not good not bad
*/
public class SudoRandomLetterGenerator : MonoBehaviour
{

    private static SudoRandomLetterGenerator instance;
    public static SudoRandomLetterGenerator Instance
    {
     get{
         if (instance == null)
         {

             instance = GameObject.FindObjectOfType<SudoRandomLetterGenerator>();
             
             if (instance == null)
             {
                 GameObject gameObj = new GameObject("SudoRandomLetterGenerator");
                 instance = gameObj.AddComponent<SudoRandomLetterGenerator>();
             }
         }
     
         return instance;
        }
    }

    // -- Config
    private readonly int reduceWordSizeMin = 2;
    private readonly int reduceWordSizeMax = 6;
    private readonly int startWordSearchAt = 2;




    private SetValuePairs allSettValuePairs;
    private LetterFrequency letterFrequency;
    

    // Maby have stert insted
    void Awake()
    {
        using (StreamReader r = new StreamReader("Assets/Scripts/Helper/Letter_gen_Finn_pos/letter_sett_lists/letter_frequensy_weight_100.json"))
        {
            string jsonText = r.ReadToEnd();
            this.letterFrequency = JsonUtility.FromJson<LetterFrequency>(jsonText);

        }
        
        using (StreamReader r = new StreamReader("Assets/Scripts/Helper/Letter_gen_Finn_pos/letter_sett_lists/score_sorted_sett_size_dict.json"))
        {
            string jsonText = r.ReadToEnd();
            this.allSettValuePairs = JsonUtility.FromJson<SetValuePairs>(jsonText);
        }

        /*
        // the algorithm can be further optimized this is bench marking
        DateTime t1 = System.DateTime.UtcNow;
        for (int i = 0; i < 300; i++)
        {
            print(this.GenerateLetter());
        }
        float rt = System.DateTime.UtcNow.Subtract(t1).Milliseconds;
        print("avg run time per gen in ms=" + rt/300);
        //*/
        
    }


    // --utillity funticions (aka shuld be in standard lib)
    List<string> RemoveAllOfAFromB(List<string> A, List<string> B)
    {
        DateTime t1 = System.DateTime.UtcNow;
        //print("A count: " + A.Count);
        //print("B count: " + B.Count);

        // tad faster
        LinkedList<string> tmp = new LinkedList<string>(B);
        foreach (string item in A)
        {
            tmp.Remove(item);
        }
        List<string> rett = new List<string>(tmp);
        //float rt = System.DateTime.UtcNow.Subtract(t1).Ticks;
        //print("remove all a from b time tick=" + rt);
        return rett;  
    }


    // Checks if A contains all elements in B
    bool IsAsubsettB(List<String> A, List<String> B)
    {
        //DateTime t1 = System.DateTime.UtcNow;

        bool isSubset = true;

        foreach (string letter in A)
        {
            int numInB = B.FindAll(bElement => bElement.Equals(letter)).Count;
            int numInA = A.FindAll(aElement => aElement.Equals(letter)).Count;
            
            if (numInB < numInA)
            {
                isSubset = false;
                break;
            }
        }
        //float rt = System.DateTime.UtcNow.Subtract(t1).Ticks;
        //print("AsubB run time ms=" + rt);

        return isSubset;
    }


    // -- Algorithm

    private List<String> letterSet = new List<String>();
    private List<String> activeLetterSet = new List<String>();

    public List<String> LetterSet {get; internal set;}

    public String GenerateLetter()
    {

        List<SetValuePair> nextUsedSets = new List<SetValuePair>();

        // if to short return weighted random
        if (this.activeLetterSet.Count <= 2)
        {
            String l = this.letterFrequency.GetLetterByFrequency();
            this.activeLetterSet.Add(l);
            this.letterSet.Add(l);
            return l;
        }

        // try normal generation

        nextUsedSets = this.FindNextLetterSetts(this.activeLetterSet);


        // no normal found try find from reduced
        if (nextUsedSets.Count == 0)
        {
            nextUsedSets = this.GetFromReduced();
        } 

        if (nextUsedSets.Count > 0)
        {
            // TODO:
            //      Find an algorithm for selecting words based on diffeculty
            //
            var rand = new System.Random();
            int val = rand.Next(0, nextUsedSets.Count);
            SetValuePair usedSett = nextUsedSets[val];

            /*
            // -- debug / optimizing
            print($"of {nextUsedSets.Count} possible words num {val} was chosen");
            // cor seq
            string currSeq = "";
            foreach (string Let in this.activeLetterSet){currSeq += Let;}
            print($"curr seq {currSeq}");

            if (nextUsedSets.Count < 5)
            {
                foreach (SetValuePair item in nextUsedSets)
                {
                    string tmpSett = "";
                    foreach (string Let in item.letter_sett)
                    {
                        tmpSett += Let;
                    }
                    print($"Lettersett {tmpSett} with score {item.value} is subsett?: {this.IsAsubsettB(this.activeLetterSet, item.letter_sett)}" );
                }
            }
            */

           

            //print(this.RemoveAllOfAFromB(this.activeLetterSet, usedSett.letter_sett).Count);
            String nextLetter = this.RemoveAllOfAFromB(this.activeLetterSet, usedSett.letter_sett)[0];
            
            this.activeLetterSet.Add(nextLetter);
            this.letterSet.Add(nextLetter);
            return nextLetter;
        } else
        {

            String l = this.letterFrequency.GetLetterByFrequency();
            this.activeLetterSet.Add(l);
            this.letterSet.Add(l);
            return l; 
        }
        
        
    }


    //   Trys to find the next letter given the provided letter stt
    //      returns none if no possible setts
    List<SetValuePair> FindNextLetterSetts(List<string> letterSett)
    {
        
        List<SetValuePair> possibleLetterSetts = new List<SetValuePair>();

        foreach (SetValuePair svp in this.allSettValuePairs.GetListFromIndex(letterSett.Count + 1))
        {
            if (this.IsAsubsettB(letterSett, svp.letter_sett)){
                possibleLetterSetts.Add(svp);
            }
        }
        return possibleLetterSetts;

    }

    List<SetValuePair> GetFromReduced()
    {
        int activeSetLen = this.activeLetterSet.Count;
        int tmpRedWordSize = this.reduceWordSizeMin;

        List<SetValuePair> possibleLetterSetts = new List<SetValuePair>();

        var rand = new System.Random();

        while (true)
        {
        
            if (tmpRedWordSize >= activeSetLen - this.startWordSearchAt || tmpRedWordSize >= this.reduceWordSizeMax)
            {
                return possibleLetterSetts;
            }

            List<SetValuePair> possibleReductWords = this.GetPossibleReductions(tmpRedWordSize);
            int tmpCount = possibleReductWords.Count;
            for (int i = 0; i < tmpCount; i++)
            { 
                int currentTest = rand.Next(0, possibleReductWords.Count);
                SetValuePair posR = possibleReductWords[currentTest];

                List<String> tmpActiveSequence = this.RemoveAllOfAFromB(posR.letter_sett, this.activeLetterSet);
                List<SetValuePair> possibleSetts = this.FindNextLetterSetts(tmpActiveSequence);
                if (possibleSetts.Count > 0)
                {
                    // print($"Reduced letter sett by {tmpRedWordSize} caracters");
                    this.activeLetterSet = tmpActiveSequence;
                    return possibleSetts;
                }

                possibleReductWords.RemoveAt(currentTest);
            }

            /*
            // begge disse skal fjenes senere for en faktisk algoritme (for vanskelighetsgrad stuff) så ikke fjern
            foreach (SetValuePair posR in possibleReductWords)
            {
                List<String> tmpActiveSequence = this.RemoveAllOfAFromB(posR.letter_sett, this.activeLetterSet);
                List<SetValuePair> possibleSetts = this.FindNextLetterSetts(tmpActiveSequence);
                if (possibleSetts.Count > 0)
                {
                    print($"Reduced letter sett by {tmpRedWordSize} caracters");
                    this.activeLetterSet = tmpActiveSequence;
                    return possibleSetts;
                }

            }
            */

            tmpRedWordSize += 1;

        }
    }

    List<SetValuePair> GetPossibleReductions(int reductionWordSize)
    {
        List<SetValuePair> possibleReductionSetts = new List<SetValuePair>();
        foreach (SetValuePair svp in this.allSettValuePairs.GetListFromIndex(reductionWordSize))
        {
            if (this.IsAsubsettB(svp.letter_sett, this.activeLetterSet)){
                possibleReductionSetts.Add(svp);
            }
        } 
        return possibleReductionSetts;
    }
}
