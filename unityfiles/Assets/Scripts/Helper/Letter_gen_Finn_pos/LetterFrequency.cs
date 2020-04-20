using System.Collections.Generic;

/// <summary>
/// Holds the different letter and their associated frequency
/// 
/// Can return a weighted by frequency random letter
/// </summary>
[System.Serializable]
public class LetterFrequency {
    public List<string> letter;
    public List<float> weight;

    /// <summary>
    /// Returns a weighted random letter by use frequency
    /// </summary>
    /// <returns>the chosen letter</returns>
    public string GetLetterByFrequency() {
        var r = new System.Random();
        int val = r.Next(1001);

        for (int i = 0; i < weight.Count; i++) {
            if (weight[i] >= val) {
                return letter[i];
            }

        }
        return null;
    }

}