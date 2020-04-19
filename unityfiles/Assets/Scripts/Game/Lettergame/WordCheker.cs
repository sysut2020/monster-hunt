using System;
using Monsterhunt.Fileoperation;
using UnityEngine;

public class WordChecker {

	private String[] wordlist;

	public WordChecker(string[] wordlist, bool sort) {
		this.wordlist = wordlist;
		if (sort) {
			SortWordlist();
		}
	}

	private void SortWordlist() {
		Array.Sort(this.wordlist);
	}

	/// <summary>
	/// Returns true if word is a valid word, else false
	/// </summary>
	/// <param name="word"></param>
	/// <returns>true if valid word, else false</returns>
	public bool isWordValid(string word) {
		// Returns the index in the array where it found the word. Negetive value if nothing was found
		return Array.BinarySearch(this.wordlist, word.ToLower()) >= 0;
	}
}