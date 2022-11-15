using System;
using System.Collections.Generic;

[Serializable]
public class FieldData
{
    public FieldUI fieldUI;
    public bool isMine = false;
    public int minesNearField;
    public int xPos;
    public int yPos;
    public Dictionary<FieldDirection, FieldData> neigbours;
    public FieldData(int xPos, int yPos, FieldUI fieldUI)
    {
        this.fieldUI = fieldUI;
        this.xPos = xPos;
        this.yPos = yPos;
        neigbours = new Dictionary<FieldDirection, FieldData>();
    }
    public void SetNeigbour(FieldDirection direction, FieldData field)
    {
        neigbours[direction] = field;
        field.neigbours[direction.Opposite()] = this;

    }
    public int GetNeigboursMineCount()
    {
        minesNearField = 0;
        for (int i = 0; i < 8; i++)
        {
            if (neigbours.ContainsKey((FieldDirection)i))
            {
                minesNearField += neigbours[(FieldDirection)i].isMine ? 1 : 0;
            }
        }
        return minesNearField;
    }
    public void CheckNeignboursField()
    {
        if (minesNearField != 0) return;
        foreach (FieldData field in neigbours.Values)
        {
            if (field.fieldUI.buttton.activeInHierarchy) field.fieldUI.OpenField(); // I am not sure if that would be correct writing... repeating field.fieldUI

        }
    }
}
