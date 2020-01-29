


/*
        // the algorithm can be further optimized this is bench marking
        DateTime t1 = System.DateTime.UtcNow;
        for (int i = 0; i < 500; i++)
        {
            this.GenerateLetter();
        }
        float rt = System.DateTime.UtcNow.Subtract(t1).Ticks;
        print("avg run time per gen in ms=" + rt/500);
        //


        // print("acont  = " + activeAr.Count());
                // print("bcount = " + usedSett.letter_sett.Count());
                // string aaa = "";
                // foreach (string Let in activeAr){aaa += Let;}

                // string bbb = "";
                // foreach (string Let in usedSett.letter_sett){bbb += Let;}

                // string ccc = "";
                // foreach (string Let in WUArrays.RemoveAllAFromB(activeAr, usedSett.letter_sett)){ccc += Let;}

                // print($"remm all A=<<{aaa}>>  B=<<{bbb}>> result =<<{ccc}>>");
                // print($"is a sub b A=<<{aaa}>>  B=<<{bbb}>> result =<<{WUArrays.IsASubsetB(activeAr, usedSett.letter_sett)}>>");


                 // // -- debug / optimizing
                // // print($"of {nextUsedSets.Count} possible words num {val} was chosen");
                //cor seq
                // string currSeq = "";
                // foreach (string Let in activeAr){currSeq += Let;}
                // print($"curr seq {currSeq}");

                // if (nextUsedSets.Count < 5)
                // {
                //     foreach (SetValuePair item in nextUsedSets)
                //     {
                //         string tmpSett = "";
                //         foreach (string Let in item.letter_sett)
                //         {
                //             tmpSett += Let;
                //         }
                //         // print($"Lettersett {tmpSett} with score {item.value} is subsett?: {WUArrays.IsASubsetB(activeAr, item.letter_sett)}" );
                //     }
                // }
                // //

*/

class DebugTimer
{
    
}