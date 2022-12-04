using UnityEngine;
using UnityEngine.UI;

public class MapOfFields : MonoBehaviour
{
    public int height;
    public int width;
    public int MineCount;
    public FieldUI prefab;
    public Transform canvasParent;
    int minesLeft;
    FieldUI[] fields;
    public EventManager eventManager;
    Camera mainCamera;

    public void Init(EventManager eventManager, Camera mainCamera)
    {
        this.mainCamera = mainCamera;
        this.eventManager = eventManager;
        eventManager.gameOver += GameOver;
        eventManager.firstFieldOpened += GenerateMines;
        SetMapParameters();
        SetCameraParameters();
        makeMapOfFields();

    }

    void SetMapParameters()
    {
        fields = new FieldUI[height * width];
        minesLeft = MineCount;
        if (minesLeft >= height * width) minesLeft = height * width / 2;

    }
    void makeMapOfFields()
    {
        FieldUI.graphicRaycaster = canvasParent.gameObject.AddComponent<GraphicRaycaster>();
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

        fields[i] = Instantiate<FieldUI>(prefab, pos, Quaternion.identity, canvasParent);
        FieldData data = new FieldData();
        fields[i].Init(eventManager, data, new NeigbourComponent(data));
    }
    void SetCameraParameters()
    {
        Vector3 cameraPos = new Vector3((float)height / 2 - 0.5f, (float)width / 2 - 0.5f, -10f);
        mainCamera.transform.position = cameraPos;
    }

    void SetFieldData(int x, int y, int i)
    {
        if (x > 0)
        {
            SetNeigbours(fields[i], fields[i - 1], FieldDirection.West);
        }
        if (y > 0)
        {
            SetNeigbours(fields[i], fields[i - width], FieldDirection.South);
            if (x > 0)
            {
                SetNeigbours(fields[i], fields[i - width - 1], FieldDirection.WestSouth);
            }
            if (x < width - 1)
            {
                SetNeigbours(fields[i], fields[i - width + 1], FieldDirection.EastSouth);
            }
        }
    }
    void SetNeigbours(FieldUI currentFieldData, FieldUI neigbour, FieldDirection neigbourDirection)
    {
        currentFieldData.neigbourComponent.SetNeigbours(neigbourDirection, neigbour);
        neigbour.neigbourComponent.SetNeigbours(neigbourDirection.Opposite(), currentFieldData);
    }
    void GenerateMines(FieldUI skipField)
    {

        while (minesLeft > 0)
        {
            int rand = Random.Range(0, height * width);

            if (!fields[rand].fieldData.isMine)
            {
                if (fields[rand] != skipField)
                {
                    fields[rand].fieldData.isMine = true;
                    minesLeft--;
                }

            }
        }
        foreach (FieldUI fieldUI in fields)
        {
            fieldUI.SetMineInfo();
        }
    }
    void GameOver()
    {
        foreach (FieldUI fieldUI in fields)
        {
            if (fieldUI.fieldData.isMine)
            {
                fieldUI.GameOverOpening();
            }
        }
    }
    [ContextMenu("Restart")]
    void Restart()
    {
        eventManager.RestartTheGame();
        FieldUI.graphicRaycaster.enabled = true;
        foreach (var data in fields)
        {
            data.fieldData.isMine = false;
            data.fieldData.minesNearField = 0;
        }
        minesLeft = MineCount;
    }
}
