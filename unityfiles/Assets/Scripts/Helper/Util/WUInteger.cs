

static class WUInteger
{

    /// <summary>
    /// Checks if the value is between low (inclusive) to high (exclusive)
    /// </summary>
    /// <param name="low">the lower bound (inclusive) to check from</param>
    /// <param name="val">the value t check</param>
    /// <param name="high">the higher bound (exclusive) to check from</param>
    /// <returns>true if the value is in the range</returns>
    public static bool IsInRange(int val, int low, int high){
        return (low <= val && high > val);
    }
}
