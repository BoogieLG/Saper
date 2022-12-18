using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MapOfFields : MonoBehaviour
{
    [Range(7, 15)]
    public int mapSize;
    public int MineCount;
    public FieldUI prefab;
    public Transform canvasParent;
    int minesLeft;
    FieldUI[] fields;
    public EventManager eventManager;
    Camera mainCamera;

    [SerializeField]int fieldsLeft;
    public void Init(EventManager eventManager, Camera mainCamera)
    {
        this.mainCamera = mainCamera;
        this.eventManager = eventManager;
        eventManager.gameOver += GameOver;
        eventManager.firstFieldOpened += GenerateMines;
        eventManager.openedField += OpenedField;
    }
    public void StartGame()
    {
        SetMapParameters();
        SetCameraParameters();
        makeMapOfFields();
        fieldsLeft = mapSize * mapSize;
    }
    void OpenedField()
    {
        fieldsLeft--;
        if(fieldsLeft == MineCount)
        {
            eventManager.Victory();
        }
    }
    void SetMapParameters()
    {
        fields = new FieldUI[mapSize * mapSize];
        minesLeft = MineCount;
        if (minesLeft >= mapSize * mapSize)
        {
            minesLeft = mapSize * mapSize / 2;
        }

    }

    void makeMapOfFields()
    {
        FieldUI.graphicRaycaster = canvasParent.gameObject.AddComponent<GraphicRaycaster>();
        for (int y = 0, i = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
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
        Vector3 cameraPos = new Vector3((float)mapSize / 2 - 0.5f, (float)mapSize / 2 - 0.5f, -10f);
        mainCamera.transform.position = cameraPos;
        mainCamera.orthographicSize = (int)mapSize / 2 + 1;
    }

    void SetFieldData(int x, int y, int i)
    {
        if (x > 0)
        {
            SetNeigbours(fields[i], fields[i - 1], FieldDirection.West);
        }
        if (y > 0)
        {
            SetNeigbours(fields[i], fields[i - mapSize], FieldDirection.South);
            if (x > 0)
            {
                SetNeigbours(fields[i], fields[i - mapSize - 1], FieldDirection.WestSouth);
            }
            if (x < mapSize - 1)
            {
                SetNeigbours(fields[i], fields[i - mapSize + 1], FieldDirection.EastSouth);
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
            int rand = Random.Range(0, mapSize * mapSize);

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
    public void ClearField()
    {
        foreach (FieldUI field in fields)
        {
            Destroy(field.gameObject);
        }
    }
    public void Restart()
    {
        fieldsLeft = mapSize * mapSize;
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
