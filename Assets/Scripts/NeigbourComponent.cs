using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeigbourComponent
{
    public Dictionary<FieldDirection, FieldUI> Neigbours;

    FieldData _fieldData;


    public NeigbourComponent(FieldData fieldData)
    {
        Neigbours = new Dictionary<FieldDirection, FieldUI>();
        this._fieldData = fieldData;
    }

    public int GetNeigboursMineCount()
    {
        _fieldData.MinesNearField = 0;
        for (int i = 0; i < 8; i++)
        {
            if (Neigbours.ContainsKey((FieldDirection)i))
            {
                _fieldData.MinesNearField += Neigbours[(FieldDirection)i].FieldData.IsMine ? 1 : 0;
            }
        }
        return _fieldData.MinesNearField;
    }

    public void CheckNeignboursField()
    {
        if (_fieldData.MinesNearField != 0) return;
        foreach (FieldUI neigbour in Neigbours.Values)
        {
            if (neigbour.Buttton.activeInHierarchy) neigbour.OpenField();
        }
    }

    public void SetNeigbours(FieldDirection direction, FieldUI fieldUI)
    {
        Neigbours[direction] = fieldUI;
    }
}
