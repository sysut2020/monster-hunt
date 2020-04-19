using System.Collections.Generic;

/// <summary>
/// Shuffles a List<T>
/// It does not return anything, it alters the List directly.
/// 
/// Usage> someList.Shuffle()
/// 
/// https://forum.unity.com/threads/clever-way-to-shuffle-a-list-t-in-one-line-of-c-code.241052/
/// </summary>
public static class IListExtensions {
	/// <summary>
	/// Shuffles the element order of the specified list.
	/// </summary>
	public static void Shuffle<T>(this IList<T> ts) {
		var count = ts.Count;
		var last = count - 1;
		for (var i = 0; i < last; ++i) {
			var r = UnityEngine.Random.Range(i, count);
			var tmp = ts[i];
			ts[i] = ts[r];
			ts[r] = tmp;
		}
	}
}