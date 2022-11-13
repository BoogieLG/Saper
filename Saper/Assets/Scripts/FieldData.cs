using System.Collections.Generic;

public class FieldData
{
    public bool isMine = false;
    public int xPos;
    public int yPos;
    public Dictionary<FieldDirection, FieldData> neigbours;
    public FieldData(int xPos, int yPos)
    {
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
        int amount = 0;
        for (int i = 0; i < 8; i++)
        {
            if (neigbours.ContainsKey((FieldDirection)i))
            {
                amount += neigbours[(FieldDirection)i].isMine ? 1 : 0;
            }
        }
        return amount;
    }

}
