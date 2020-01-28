using System.Collections;
using System.Collections.Generic;

class Timers
{
    private Dictionary<string, Timer> timers = new Dictionary<string, Timer>();


    public bool d(string timerID)
    {
        bool retObj = false;
        if (timers.ContainsKey(timerID))
        {
            Timer t = timers[timerID];
            if (t.done())
            {
                if (t.SholdRestart)
                {
                    
                }
                retObj = true;
            } else
            {
                retObj = false;
            }
        }
        return retObj;
    }

    // public void set
}