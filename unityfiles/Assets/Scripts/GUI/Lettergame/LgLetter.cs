using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO: shit name, improve.
public class LgLetter{

    private readonly string letter;
    private int xPos, yPos;
    private bool isOnBoard = false;

    public LgLetter(int x, int y, string tileLetter){
        this.XPos = x;
        this.YPos = y;
        this.letter = tileLetter;
    }

    public string Letter { get => letter;}
    public bool IsOnBoard { get => isOnBoard; set => isOnBoard = value; }
    public int YPos { get => yPos; set => yPos = value; }
    public int XPos { get => xPos; set => xPos = value; }
}
