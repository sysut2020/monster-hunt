using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;



public class SudoRandomLetterGenerator : MonoBehaviour
{

    // -- Config
    private int reduceWordSizeMin = 2;
    private int reduceWordSizeMax = 6;
    private int startWordSearchAt = 2;




    private SetValuePairs allSettValuePairs;
    private LetterFrequency letterFreq;
    void Start()
    {
       using (StreamReader r = new StreamReader("Assets/Scripts/Helper/Letter_gen_Finn_pos/letter_sett_lists/letter_frequensy_weight_100.json"))
        {
            string jsonText = r.ReadToEnd();
            this.letterFreq = JsonUtility.FromJson<LetterFrequency>(jsonText);

        }
        
        using (StreamReader r = new StreamReader("Assets/Scripts/Helper/Letter_gen_Finn_pos/letter_sett_lists/score_sorted_sett_size_dict.json"))
        {
            string jsonText = r.ReadToEnd();
            this.allSettValuePairs = JsonUtility.FromJson<SetValuePairs>(jsonText);
        }

        /*
        // the algorithm can be further optimized this is benshmarking
        float t1 = System.DateTime.Now.Millisecond;
        for (int i = 0; i < 100; i++)
        {
            print(this.GenerateLetter());
        }
        float rt = System.DateTime.Now.Millisecond - t1;
        print("avg run time per gen in ms=" + rt/100);
        */
        
    }

    // --utillity funticions (aka shuld be in standard lib)
    List<string> RemoveAllOfAFromB(List<string> A, List<string> B)
    {
        List<string> ret = new List<string>(B);

        foreach (string item in A)
        {
            ret.Remove(item);
        }
        return ret;  
    }


    // Checks if A contains all elements in B
    bool IsAsubsettB(List<String> A, List<String> B)
    {
        bool isSubset = true;

        foreach (string letter in A)
        {
            int numInB = B.FindAll(bElement => bElement.Equals(letter)).Count;
            int numInA = A.FindAll(aElement => aElement.Equals(letter)).Count;
            if (numInB > numInA)
            {
                isSubset = false;
                break;
            }
        }

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
            String l = this.letterFreq.GetLetterByFrequency();
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

            //print(this.RemoveAllOfAFromB(this.activeLetterSett, usedSett.letter_sett).Count);
            String nextLetter = this.RemoveAllOfAFromB(this.activeLetterSet, usedSett.letterSet)[0];
            
            this.activeLetterSet.Add(nextLetter);
            this.letterSet.Add(nextLetter);
            return nextLetter;
        } else
        {
            String l = this.letterFreq.GetLetterByFrequency();
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
            if (this.IsAsubsettB(letterSett, svp.letterSet)){
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

        while (true)
        {
        
            if (tmpRedWordSize >= activeSetLen - this.startWordSearchAt || tmpRedWordSize >= this.reduceWordSizeMax)
            {
                return possibleLetterSetts;
            }

            List<SetValuePair> possibleReductWords = this.GetPossibleReductions(tmpRedWordSize);

            foreach (SetValuePair posR in possibleReductWords)
            {
                List<String> tmpActiveSequence = this.RemoveAllOfAFromB(posR.letterSet, this.activeLetterSet);
                List<SetValuePair> possibleSetts = this.FindNextLetterSetts(tmpActiveSequence);
                if (possibleSetts.Count > 0)
                {
                    this.activeLetterSet = tmpActiveSequence;
                    return possibleSetts;
                }

            }

            tmpRedWordSize += 1;

        }
    }

    List<SetValuePair> GetPossibleReductions(int reductionWordSize)
    {
        List<SetValuePair> possibleReductionSetts = new List<SetValuePair>();
        foreach (SetValuePair svp in this.allSettValuePairs.GetListFromIndex(reductionWordSize))
        {
            if (this.IsAsubsettB(svp.letterSet, this.activeLetterSet)){
                possibleReductionSetts.Add(svp);
            }
        } 
        return possibleReductionSetts;
    }
}
