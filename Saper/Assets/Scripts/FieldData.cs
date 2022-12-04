using System;
using System.Collections.Generic;

[Serializable]
public class FieldData
{
    public bool isMine = false;
    public int minesNearField;
    public int xPos;
    public int yPos;

    public FieldData(int xPos, int yPos)
    {
        this.xPos = xPos;
        this.yPos = yPos;
        
    }
}
