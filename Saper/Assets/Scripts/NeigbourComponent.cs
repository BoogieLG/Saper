using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeigbourComponent
{
    FieldData fieldData;
    public Dictionary<FieldDirection, FieldUI> neigbours;
    public NeigbourComponent(FieldData fieldData)
    {
        neigbours = new Dictionary<FieldDirection, FieldUI>();
        this.fieldData = fieldData;
    }
    public int GetNeigboursMineCount()
    {
        fieldData.minesNearField = 0;
        for (int i = 0; i < 8; i++)
        {
            if (neigbours.ContainsKey((FieldDirection)i))
            {
                fieldData.minesNearField += neigbours[(FieldDirection)i].fieldData.isMine ? 1 : 0;
            }
        }
        return fieldData.minesNearField;
    }
    public void CheckNeignboursField()
    {
        if (fieldData.minesNearField != 0) return;
        foreach (FieldUI neigbour in neigbours.Values)
        {
            if (neigbour.buttton.activeInHierarchy) neigbour.OpenField();
        }
    }
    public void SetNeigbours(FieldDirection direction, FieldUI fieldUI)
    {
        neigbours[direction] = fieldUI;
    }
}
