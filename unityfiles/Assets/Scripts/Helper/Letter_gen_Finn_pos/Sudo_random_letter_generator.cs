using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;



public class Sudo_random_letter_generator : MonoBehaviour
{

    // -- Config
    private int reduceWordSizeMin = 2;
    private int reduceWordSizeMax = 6;
    private int startWordSertchAt = 2;




    private SettValuePairs allSVP;
    private LetterFreq letterFreq;
    void Start()
    {
       using (StreamReader r = new StreamReader("Assets/Scripts/Helper/Letter_gen_Finn_pos/letter_sett_lists/letter_frequensy_weight_100.json"))
        {
            string jsonText = r.ReadToEnd();
            this.letterFreq = JsonUtility.FromJson<LetterFreq>(jsonText);

        }
        
        using (StreamReader r = new StreamReader("Assets/Scripts/Helper/Letter_gen_Finn_pos/letter_sett_lists/score_sorted_sett_size_dict.json"))
        {
            string jsonText = r.ReadToEnd();
            this.allSVP = JsonUtility.FromJson<SettValuePairs>(jsonText);
        }


        float t1 = System.DateTime.Now.Millisecond;
        for (int i = 0; i < 100; i++)
        {
            print(this.GenerateLetter());
        }
        float rt = System.DateTime.Now.Millisecond - t1;
        print("avg run time per gen in ms=" + rt/100);
        
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


    // cheks if a contains all elements in b
    bool IsAsubsettB(List<String> A, List<String> B)
    {
        bool isSubsett = true;

        foreach (string letter in A)
        {
            int numInB = B.FindAll(bElement => bElement.Equals(letter)).Count;
            int numInA = A.FindAll(aElement => aElement.Equals(letter)).Count;
            if (numInB > numInA)
            {
                isSubsett = false;
                break;
            }
        }

        return isSubsett;
    }


    // -- Algorithm

    private List<String> letterSett = new List<String>();
    private List<String> activeLetterSett = new List<String>();

    public List<String> LetterSett {get; internal set;}

    public String GenerateLetter()
    {
         print("current Sequence ");
        foreach (string l in this.activeLetterSett)
        {
            print(l);
        }
        print("sq end ##");

        List<SettValuePair> nexUsedSetts = new List<SettValuePair>();

        // if to short return weighted random
        if (this.activeLetterSett.Count <= 2)
        {
            print("short random");
            String l = this.letterFreq.GetLetterByFreq();
            this.activeLetterSett.Add(l);
            this.letterSett.Add(l);
            return l;
        }

        // try normal generation

        nexUsedSetts = this.FindNextLetterSetts(this.activeLetterSett);


        // no normal found try find from reduced
        if (nexUsedSetts.Count == 0)
        {
            print("will reduce");
            nexUsedSetts = this.GettFromReduced();
        } 

        if (nexUsedSetts.Count > 0)
        {
            print("is found");
            // TODO:
            //      Find an algorithm for selecting words based on diffeculty
            //
            var rand = new System.Random();
            int val = rand.Next(0, nexUsedSetts.Count);
            SettValuePair usedSett = nexUsedSetts[val];

            //print(this.RemoveAllOfAFromB(this.activeLetterSett, usedSett.letter_sett).Count);
            String nextLetter = this.RemoveAllOfAFromB(this.activeLetterSett, usedSett.letter_sett)[0];
            
            this.activeLetterSett.Add(nextLetter);
            this.letterSett.Add(nextLetter);
            return nextLetter;
        } else
        {
            print("no solution random");
            String l = this.letterFreq.GetLetterByFreq();
            this.activeLetterSett.Add(l);
            this.letterSett.Add(l);
            return l;
        }
        
        
    }


    //   Trys to find the next letter given the provided letter stt
    //      returns none if no possible setts
    List<SettValuePair> FindNextLetterSetts(List<string> letterSett)
    {
        List<SettValuePair> possibleLetterSetts = new List<SettValuePair>();

        foreach (SettValuePair svp in this.allSVP.GetListFromIndex(letterSett.Count + 1))
        {
            if (this.IsAsubsettB(letterSett, svp.letter_sett)){
                possibleLetterSetts.Add(svp);
            }
        }
        return possibleLetterSetts;

    }

    List<SettValuePair> GettFromReduced()
    {
        int activeSetLen = this.activeLetterSett.Count;
        int tmpRedWordSize = this.reduceWordSizeMin;

        List<SettValuePair> possibleLetterSetts = new List<SettValuePair>();

        while (true)
        {
        
            if (tmpRedWordSize >= activeSetLen - this.startWordSertchAt || tmpRedWordSize >= this.reduceWordSizeMax)
            {
                return possibleLetterSetts;
            }

            List<SettValuePair> possibleReductWords = this.GetPossibleReductions(tmpRedWordSize);

            foreach (SettValuePair posR in possibleReductWords)
            {
                List<String> tmpActiveSequence = this.RemoveAllOfAFromB(posR.letter_sett, this.activeLetterSett);
                List<SettValuePair> possibleSetts = this.FindNextLetterSetts(tmpActiveSequence);
                if (possibleSetts.Count > 0)
                {
                    this.activeLetterSett = tmpActiveSequence;
                    return possibleSetts;
                }

            }

            tmpRedWordSize += 1;

        }
    }

    List<SettValuePair> GetPossibleReductions(int reductionWordSize)
    {
        List<SettValuePair> possibleReductionSetts = new List<SettValuePair>();
        foreach (SettValuePair svp in this.allSVP.GetListFromIndex(reductionWordSize))
        {
            if (this.IsAsubsettB(svp.letter_sett, this.activeLetterSett)){
                possibleReductionSetts.Add(svp);
            }
        } 
        return possibleReductionSetts;
    }
}
