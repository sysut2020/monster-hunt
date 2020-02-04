using System;

/// <summary>
/// Simple time object that can check if a given amount of time 
/// has passed
/// </summary>
class Timer {

    private float waitTime;
    private DateTime completeTime = new DateTime ();

    /// <summary>
    /// initiates the timer with the amount of millisecond to 
    ///     count
    /// </summary>
    /// <param name="millisec">The amount of time to check if has passed</param>
    public Timer (float millisec) {
        waitTime = millisec;
        this.Restart ();
    }

    /// <summary>
    /// returns true if the timer is complete, else false
    /// </summary>
    /// <returns>returns true if the timer is complete, else false</returns>
    public bool done () {
        if (completeTime.CompareTo(DateTime.Now) < 0) {
            return true;
        }
        return false;
    }

    /// <summary>
    /// returns the time left in milliseconds
    /// </summary>
    /// <returns>the time left in milliseconds</returns>
    public int TimeLeft () {
        return this.completeTime.Subtract (DateTime.Now).Milliseconds;
    }

    /// <summary>
    /// resets the timer with a new time
    /// </summary>
    /// <param name="millisec"></param>
    public void Update (float millisec) {
        completeTime = DateTime.Now.AddMilliseconds(millisec);
    }

    /// <summary>
    /// restarts the timer
    /// </summary>
    public void Restart () {
        completeTime = DateTime.Now.AddMilliseconds(waitTime);
    }
}