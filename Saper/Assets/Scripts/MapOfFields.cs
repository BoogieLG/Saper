using UnityEditor.UIElements;
using UnityEngine;

public class MapOfFields : MonoBehaviour
{
    public int height;
    public int width;
    public int MineCount;
    public Field prefab;
    public Transform canvasParent;

    int minesLeft;
    Field[] fields;

    Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    void Start()
    {
        SetMapParameters();
        SetCameraParameters();
        makeMapOfFields();
        GenerateMines();
    }

    void SetMapParameters()
    {
        fields = new Field[height * width];
        minesLeft = MineCount;
        if (minesLeft >= height * width) minesLeft = height * width - 1;

    }
    void makeMapOfFields()
    {
        for (int y = 0, i = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateField(x, y, i);
                SetFieldData(x, y, i);
                i++;
            }
        }
    }
    void CreateField(int x, int y, int i)
    {
        Vector2 pos = new Vector2(x, y);
        fields[i] = Instantiate<Field>(prefab, pos, Quaternion.identity, canvasParent);
    }
    void SetCameraParameters()
    {
        Vector3 cameraPos = new Vector3((float)height / 2 - 0.5f, (float)width / 2 - 0.5f, -10f);
        mainCamera.transform.position = cameraPos;
    }

    void SetFieldData(int x, int y, int i)
    {
        FieldData data = fields[i].fieldData = new FieldData(x, y);
        if (x > 0)
        {
            data.SetNeigbour(FieldDirection.West, fields[i - 1].fieldData);
        }
        if (y > 0)
        {
            data.SetNeigbour(FieldDirection.South, fields[i - width].fieldData);
            if (x > 0)
            {
                data.SetNeigbour(FieldDirection.WestSouth, fields[i - width - 1].fieldData);
            }
            if (x < width - 1)
            {
                data.SetNeigbour(FieldDirection.EastSouth, fields[i - width + 1].fieldData);
            }
        }
    }
    void GenerateMines()
    {
        while (minesLeft > 0)
        {
            int rand = Random.Range(0, height*width);
            if (!fields[rand].fieldData.isMine)
            {
                fields[rand].fieldData.isMine = true;
                minesLeft--;
            }
        }
        foreach(Field field in fields)
        {
            field.SetMineInfo();
        }
    }
}
