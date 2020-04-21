using System.Collections.Generic;

/// <summary>
/// Data class holding the SetValuePairs for the different word lengths
/// </summary>
[System.Serializable]
public class SetValuePairs {
    public List<SetValuePair> L_1;
    public List<SetValuePair> L_2;
    public List<SetValuePair> L_3;
    public List<SetValuePair> L_4;
    public List<SetValuePair> L_5;
    public List<SetValuePair> L_6;
    public List<SetValuePair> L_7;
    public List<SetValuePair> L_8;
    public List<SetValuePair> L_9;

    public List<SetValuePair> L_10;
    public List<SetValuePair> L_11;
    public List<SetValuePair> L_12;
    public List<SetValuePair> L_13;
    public List<SetValuePair> L_14;
    public List<SetValuePair> L_15;
    public List<SetValuePair> L_16;
    public List<SetValuePair> L_17;
    public List<SetValuePair> L_18;
    public List<SetValuePair> L_19;
    public List<SetValuePair> L_20;
    public List<SetValuePair> L_21;
    public List<SetValuePair> L_22;
    public List<SetValuePair> L_23;
    public List<SetValuePair> L_24;
    public List<SetValuePair> L_25;
    public List<SetValuePair> L_26;
    public List<SetValuePair> L_27;
    public List<SetValuePair> L_28;
    public List<SetValuePair> L_29;

    public List<SetValuePair> GetListFromIndex(int index) {
        switch (index) {
            case 1:
                return L_1;
            case 2:
                return L_2;
            case 3:
                return L_3;
            case 4:
                return L_4;
            case 5:
                return L_5;
            case 6:
                return L_6;
            case 7:
                return L_7;
            case 8:
                return L_8;
            case 9:
                return L_9;
            case 10:
                return L_10;
            case 11:
                return L_11;
            case 12:
                return L_12;
            case 13:
                return L_13;
            case 14:
                return L_14;
            case 15:
                return L_15;
            case 16:
                return L_16;
            case 17:
                return L_17;
            case 18:
                return L_18;
            case 19:
                return L_19;
            case 20:
                return L_20;
            case 21:
                return L_21;
            case 22:
                return L_22;
            case 23:
                return L_23;
            default:
                return L_24;

        }
    }

}
