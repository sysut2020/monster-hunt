using System;

public class WordChecker {

    //REMOVE AND READ FROM FILE
    private static String[] words = { "AAB", "ABA", "BOB", "HELLO", "UNCLE" };
    //REMOVE AND READ FROM FILE

    // -- private -- // 

    public static bool isWordValid(string w) {
        // Have to sort the array, event if it is meade sorted
        Array.Sort(words);
        // Returns the index in the array where it found the word. Negetive value if nothing was found
        return Array.BinarySearch(words, w) >= 0;
    }
}