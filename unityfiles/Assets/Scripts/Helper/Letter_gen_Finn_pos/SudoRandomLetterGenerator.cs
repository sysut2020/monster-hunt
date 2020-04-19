using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/*


    current benchmark times 500avg= 258 010.4
                                    301 961.3
                                    155 282.7
                                    235 651

*/

/// <summary>
/// Generates letters sudo randomly. This means that every new letter should
/// in combination with the previously selected letters always form one or more words
/// 
/// </summary>
public class SudoRandomLetterGenerator {

    
    /// <summary>
    /// Constructor reading the needed files from disk
    /// </summary>
    private SudoRandomLetterGenerator(){
        // reads the letter freq 
        using (StreamReader r = new StreamReader ("Assets/Scripts/Helper/Letter_gen_Finn_pos/letter_sett_lists/letter_frequensy_weight_100.json")) {
            string jsonText = r.ReadToEnd ();
            this.letterFrequency = JsonUtility.FromJson<LetterFrequency> (jsonText);

        }

        // reads the letter setts 
        using (StreamReader r = new StreamReader ("Assets/Scripts/Helper/Letter_gen_Finn_pos/letter_sett_lists/score_sorted_sett_size_dict.json")) {
            string jsonText = r.ReadToEnd ();
            this.allSettValuePairs = JsonUtility.FromJson<SetValuePairs> (jsonText);
        }

        // bench marking 
        // DateTime t1 = System.DateTime.UtcNow;
        // for (int i = 0; i < 500; i++)
        // {
        //     this.GenerateLetter();
        // }
        // float rt = System.DateTime.UtcNow.Subtract(t1).Ticks;
        // print("avg run time per gen in tics=" + rt/500);
    }


    
    private static SudoRandomLetterGenerator instance;
    public static SudoRandomLetterGenerator Instance {
        get {
            if (instance == null) {
                instance = new SudoRandomLetterGenerator();
            }

            return instance;
        }
    }

    
    private readonly int reduceWordSizeMin = 3;
    private readonly int reduceWordSizeMax = 6;
    private readonly int startWordSearchAt = 2;

    private SetValuePairs allSettValuePairs;
    private LetterFrequency letterFrequency;


    

    private readonly List<String> letterSet = new List<String> ();
    private List<String> activeLetterSet = new List<String> ();

    public List<String> LetterSet { get; internal set; }

    /// <summary>
    /// Adds a letter to the Active letter sett and the letterSet
    /// sorts the active sett as well
    /// </summary>
    /// <param name="l">the letter to add to the letter set</param>
    private void AddLetter (string l) {

        letterSet.Add (l);

        activeLetterSet.Add (l);
        activeLetterSet.Sort ();

    }

    /// <summary>
    /// Generates a letter that will in combination with the 
    /// Earlier generated letters for on or more words*
    /// 
    /// *if the algorithm cant find or reduce down to get a letter a random one will be picked
    /// </summary>
    /// <returns> the generated letter</returns>
    public String GenerateLetter () {

        List<SetValuePair> nextUsedSets = new List<SetValuePair> ();
        string[] activeAr = this.activeLetterSet.ToArray ();

        if (this.activeLetterSet.Count >= this.startWordSearchAt) {

            // try normal generation

            nextUsedSets = this.FindNextLetterSetts (activeAr);

            // no normal found try find from reduced
            if (nextUsedSets.Count == 0) {
                nextUsedSets = this.GetFromReduced ();
            }

            if (nextUsedSets.Count > 0) {
                // TODO:
                //      Find an algorithm for selecting words based on difficulty
                //
                activeAr = this.activeLetterSet.ToArray (); // if it has been reduced in reduce
                var rand = new System.Random ();
                int val = rand.Next (0, nextUsedSets.Count);
                SetValuePair usedSett = nextUsedSets[val];

                String nextLetter = WUArrays.RemoveAllAFromB (activeAr, usedSett.letter_sett) [0];

                this.AddLetter (nextLetter);
                return nextLetter;
            }
        }

        // ether the letter search yielded no results or the word was to short
        String l = this.letterFrequency.GetLetterByFrequency ();
        this.AddLetter (l);
        return l;

    }

    /// <summary>
    /// try's to find a letterset that is an subsett of length one bigger
    /// than the lettersett provided.
    /// If no sett is found an empty array is returned
    /// </summary>
    /// <param name="letterSett">the set to find candidate setts toc</param>
    /// <returns>an array of candidate lettersets, can be empty if none are found</returns>
    List<SetValuePair> FindNextLetterSetts (string[] letterSett) {

        List<SetValuePair> possibleLetterSetts = new List<SetValuePair> ();

        foreach (SetValuePair svp in this.allSettValuePairs.GetListFromIndex (letterSett.Count () + 1)) {
            if (WUArrays.IsASubsetB (letterSett, svp.letter_sett)) {
                possibleLetterSetts.Add (svp);
            }
        }
        return possibleLetterSetts;

    }

    /// <summary>
    /// try's to get candidate setts from the active letter sett by
    /// reducing it with different words, the extent of the trying 
    /// is hardcoded
    /// 
    /// if a set is found the active letter sett is automatically changed 
    /// </summary>
    /// <returns>a list of candidate sets, can be empty if none are found</returns>
    List<SetValuePair> GetFromReduced () {
        int activeSetLen = this.activeLetterSet.Count;
        int tmpRedWordSize = this.reduceWordSizeMin;
        string[] activeAr = this.activeLetterSet.ToArray ();

        List<SetValuePair> possibleLetterSetts = new List<SetValuePair> ();

        var rand = new System.Random ();

        while (true) {

            if (tmpRedWordSize >= activeSetLen - this.startWordSearchAt || tmpRedWordSize >= this.reduceWordSizeMax) {
                return possibleLetterSetts;
            }

            List<SetValuePair> possibleReductWords = this.GetPossibleReductions (tmpRedWordSize);
            int tmpCount = possibleReductWords.Count;
            for (int i = 0; i < tmpCount; i++) {
                int currentTest = rand.Next (0, possibleReductWords.Count);
                SetValuePair posR = possibleReductWords[currentTest];

                List<String> tmpActiveSequence = WUArrays.RemoveAllAFromB (posR.letter_sett, activeAr);
                tmpActiveSequence.Sort ();

                List<SetValuePair> possibleSetts = this.FindNextLetterSetts (tmpActiveSequence.ToArray ());
                if (possibleSetts.Count > 0) {
                    //print($"Reduced letter sett by {tmpRedWordSize} caracters");
                    this.activeLetterSet = tmpActiveSequence;
                    return possibleSetts;
                }

                possibleReductWords.RemoveAt (currentTest);
            }

            tmpRedWordSize += 1;

        }
    }

    /// <summary>
    /// returns a list of the possible reduction sets 
    /// to the active set of size reductionWordSize
    /// </summary>
    /// <param name="reductionWordSize">the size of the reduction words</param>
    /// <returns>the possible reduction setts</returns>
    List<SetValuePair> GetPossibleReductions (int reductionWordSize) {
        List<SetValuePair> possibleReductionSetts = new List<SetValuePair> ();
        string[] activeAr = this.activeLetterSet.ToArray ();
        foreach (SetValuePair svp in this.allSettValuePairs.GetListFromIndex (reductionWordSize)) {
            if (WUArrays.IsASubsetB (svp.letter_sett, activeAr)) {
                possibleReductionSetts.Add (svp);
            }
        }
        return possibleReductionSetts;
    }
}