using System.Collections.Generic;
using UnityEngine;

public class MapOfFields : MonoBehaviour
{

    public int RowCount;
    public int ColumnCount;
    public int MineCount;
    public GameObject prefab;
    public Transform canvas;

    int minesLeft;
    Field[,] map;
    Camera mainCamera;


    void Start()
    {
        map = new Field[RowCount, ColumnCount];
        minesLeft = MineCount;
        if (minesLeft >= RowCount * ColumnCount) minesLeft = RowCount * ColumnCount - 1;
        mainCamera = Camera.main;
        makeMapOfFields();

    }

    void makeMapOfFields() // TODO refactore for fast restart and making first choose 100% no mine
    {

        for (int r = 0; r < RowCount; r++)
        {
            for (int c = 0; c < ColumnCount; c++)
            {
                Vector2 pos = new Vector2(c, -r);
                map[r, c] = Instantiate(prefab, pos, Quaternion.identity, canvas).GetComponent<Field>();
                map[r, c].isMine = IsMine();
            }
        }

        while (minesLeft > 0) // TODO change for more performance
        {
            for (int r = 0; r < RowCount; r++)
            {
                for (int c = 0; c < ColumnCount; c++)
                {
                    if (!map[r, c].isMine)
                    {
                        map[r, c].isMine = IsMine();
                    }
                }
            }
        }

        SetValues();

        float xPos = map[ColumnCount / 2, RowCount / 2].transform.position.x;
        float yPos = map[ColumnCount / 2, RowCount / 2].transform.position.y;
        mainCamera.transform.position = new Vector3(xPos, yPos, -10);
    }

    void SetValues()
    {
        for (int r = 0; r < RowCount; r++)
        {
            for (int c = 0; c < ColumnCount; c++)
            {
                if (map[r, c].isMine) map[r, c].SetValue(9);
                else
                {
                    map[r, c].SetValue(MinesAround(r, c));
                }
            }
        }
    }
    int MinesAround(int row, int column)
    {
        int value = 0;
        List<Field> tempFields = new List<Field>();

        if (column > 0)
        {
            tempFields.Add(map[row, column - 1]);
            if (row > 0)
            {
                tempFields.Add(map[row - 1, column]);
                tempFields.Add(map[row - 1, column - 1]);
                if (row < RowCount - 1)
                {
                    tempFields.Add(map[row + 1, column]);
                    tempFields.Add(map[row + 1, column - 1]);
                }
            }
            else
            {
                tempFields.Add(map[row + 1, column]);
                tempFields.Add(map[row + 1, column - 1]);
            }

            if (column < ColumnCount - 1)
            {
                tempFields.Add(map[row, column + 1]);
                if (row > 0)
                {
                    tempFields.Add(map[row - 1, column + 1]);
                }
                if (row < RowCount - 1)
                {
                    tempFields.Add(map[row + 1, column + 1]);
                }
            }
        }
        else
        {
            tempFields.Add(map[row, column + 1]);
            if (row > 0)
            {
                tempFields.Add(map[row - 1, column]);
                tempFields.Add(map[row - 1, column + 1]);
            }
            if (row < RowCount - 1)
            {
                tempFields.Add(map[row + 1, column]);
                tempFields.Add(map[row + 1, column + 1]);
            }

        }

        foreach (Field field in tempFields)
        {
            if (field.isMine) value++;
        }

        return value;
    }
    bool IsMine()
    {
        if (minesLeft <= 0) return false;
        float chance = (RowCount * ColumnCount) / MineCount / 100f;
        float random = Random.Range(0f, 1f);
        if (random <= chance)
        {
            minesLeft--;
            return true;
        }
        else return false;
    }

}
