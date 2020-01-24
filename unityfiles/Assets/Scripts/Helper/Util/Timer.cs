using System;

class Timer
{

    public bool SholdRestart {get; internal set;}


    private float waitTime;
    private DateTime completeTime = new DateTime();

    Timer(float millisec)
    {
        waitTime = millisec;
        this.Restart();
    }

    public bool done()
    {
        if (completeTime.CompareTo(DateTime.Now) > 0)
        {
            return true;
        }
        return false;
    }

    private void Restart()
    {
        completeTime.AddMilliseconds(waitTime);
    }
}