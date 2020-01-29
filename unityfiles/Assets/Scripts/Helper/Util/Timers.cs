using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// A object who keeps track of multiple timers.
/// can set, check if done, remove, and reset timers
/// </summary>
public class Timers {
    private Dictionary<string, Timer> timers = new Dictionary<string, Timer> ();

    // -- properties -- //

    // -- public -- //

    /// <summary>
    /// Removes the timer with the given timer id
    /// </summary>
    /// <param name="timerID">the id of the timer to remove</param>
    public void Remove (string timerID) {
        timers.Remove (timerID);
    }

    /// <summary>
    /// Updates the wait time and resets the timer of the given id.
    /// If no timer is found returns false
    /// </summary>
    /// <param name="timerID">the id of the timer to update</param>
    /// <param name="newTimeInMillisec">the new time in millisec</param>
    /// <returns>true if successful false if not</returns>
    public bool Update (string timerID, float newTimeInMillisec) {
        bool suc = false;
        if (timers.Keys.Contains (timerID)) {
            Timer toUpdate = timers[timerID];
            suc = true;
        }
        return suc;
    }

    /// <summary>
    /// Create a new timer with the given id and wait time
    /// If a timer of the given id exists false is returned
    /// </summary>
    /// <param name="timerID"></param>
    /// <param name="waitTimeInMillisec">the time for the new timer to wait in millisec</param>
    /// <returns></returns>
    public bool Set (string timerID, float waitTimeInMillisec) {
        bool suc = false;
        if (!timers.Keys.Contains (timerID)) {
            Timer newTimer = new Timer (waitTimeInMillisec);
            timers.Add (timerID, newTimer);
            suc = true;
        }
        return suc;
    }

    /// <summary>
    /// returns a list of the ids off all the completed timers
    /// </summary>
    /// <returns>a list of all the completed timers</returns>
    public List<string> GetCompletedTimers () {
        List<string> retList = new List<string> ();
        foreach (string key in this.timers.Keys) {
            if (this.timers[key].done ()) {
                retList.Add (key);
            }
        }
        return retList;
    }

    /// <summary>
    /// Checks if a timer with the given id exists
    ///     if it does return true if not return false
    /// </summary>
    /// <param name="timerID">the timer id to check</param>
    /// <returns>true if a timer of the given id exists false if not</returns>
    public bool Exists (string timerID) {
        return timers.Keys.Contains (timerID);
    }

    /// <summary>
    /// returns the time left in the timer of the given id
    /// if no timer has the id -1 is returned 
    /// if timer has passed 0 is returned
    /// </summary>
    /// <param name="timerID">the timer to get time left from</param>
    /// <returns>the time left in millisec</returns>
    public int TimeLeft (string timerID) {
        int ret = -1;
        if (timers.Keys.Contains (timerID)) {
            ret = this.timers[timerID].TimeLeft ();
            if (ret < 0) {
                ret = 0;
            }
        }
        return ret;
    }

    // -- overloads -- //

    /// <summary>
    /// Checks if a timer is complete returns true if it is false if not
    /// if the timer is complete and restart is true the timer is restarted
    /// if the timer is complete and restart is false the timer is removed
    /// if no timer of the given id exists false is returned
    /// </summary>
    /// <param name="timerID">the id of the timer to check</param>
    /// <param name="restart">If the timer should be restarted</param>
    /// <returns>true if timer is done false if not</returns>
    public bool Done (string timerID, bool restart) => this.isTimerDone (timerID, restart);

    /// <summary>
    /// Checks if a timer is complete returns true if it is false if not
    /// if the timer is complete the timer is removed
    /// if no timer of the given id exists false is returned
    /// </summary>
    /// <param name="timerID">the id of the timer to check</param>
    /// <returns>true if timer is done false if not</returns>
    public bool Done (string timerID) => this.isTimerDone (timerID, false);

    // -- private -- // 

    /// <summary>
    /// Checks if a timer is complete returns true if it is false if not
    /// if the timer is complete and restart is true the timer is restarted
    /// if the timer is complete and restart is false the timer is removed
    /// if no timer of the given id exists false is returned
    /// </summary>
    /// <param name="timerID">the id of the timer to check</param>
    /// <param name="restart">If the timer should be restarted</param>
    /// <returns>true if timer is done false if not</returns>
    private bool isTimerDone (string timerID, bool restart) {
        bool retObj = false;
        if (timers.ContainsKey (timerID)) {
            Timer t = timers[timerID];
            if (t.done ()) {
                if (restart) {
                    t.Restart ();
                } else {
                    timers.Remove (timerID);
                }
                retObj = true;
            } else {
                retObj = false;
            }
        }
        return retObj;
    }
}