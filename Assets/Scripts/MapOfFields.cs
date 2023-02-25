using UnityEngine;
using UnityEngine.UI;

public class MapOfFields : MonoBehaviour
{
    [Range(7, 15)]
    public int MapSize;
    public int MineCount;
    public FieldUI Prefab;
    public Transform CanvasParent;
    public EventController EventControll;

    [SerializeField] int _fieldsLeft;

    int _minesLeft;
    FieldUI[] _fields;
    Camera _mainCamera;

    public void Init(EventController evebtController, Camera mainCamera)
    {
        _mainCamera = mainCamera;
        EventControll = evebtController;
        evebtController.OnGameOver += GameOver;
        evebtController.OnFirstFieldOpened += GenerateMines;
        evebtController.OnOpenedField += OpenedField;
    }

    public void StartGame()
    {
        SetMapParameters();
        SetCameraParameters();
        MakeMapOfFields();
        _fieldsLeft = MapSize * MapSize;
    }

    public void ClearField()
    {
        foreach (FieldUI field in _fields)
        {
            Destroy(field.gameObject);
        }
    }

    public void Restart()
    {
        _fieldsLeft = MapSize * MapSize;
        EventControll.RestartTheGame();
        FieldUI.GraphicRaycaster.enabled = true;
        foreach (var data in _fields)
        {
            data.FieldData.IsMine = false;
            data.FieldData.MinesNearField = 0;
        }
        _minesLeft = MineCount;
    }

    void OpenedField()
    {
        _fieldsLeft--;
        if (_fieldsLeft == MineCount)
        {
            EventControll.Victory();
        }
    }

    void SetMapParameters()
    {
        _fields = new FieldUI[MapSize * MapSize];
        _minesLeft = MineCount;
        if (_minesLeft >= MapSize * MapSize)
        {
            _minesLeft = MapSize * MapSize / 2;
        }

    }

    void MakeMapOfFields()
    {
        FieldUI.GraphicRaycaster = CanvasParent.gameObject.AddComponent<GraphicRaycaster>();
        for (int y = 0, i = 0; y < MapSize; y++)
        {
            for (int x = 0; x < MapSize; x++)
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

        _fields[i] = Instantiate<FieldUI>(Prefab, pos, Quaternion.identity, CanvasParent);
        FieldData data = new FieldData();
        _fields[i].Init(EventControll, data, new NeigbourComponent(data));
    }

    void SetCameraParameters()
    {
        Vector3 cameraPos = new Vector3((float)MapSize / 2 - 0.5f, (float)MapSize / 2 - 0.5f, -10f);
        _mainCamera.transform.position = cameraPos;
        _mainCamera.orthographicSize = (int)MapSize / 2 + 1;
    }

    void SetFieldData(int x, int y, int i)
    {
        if (x > 0)
        {
            SetNeigbours(_fields[i], _fields[i - 1], FieldDirection.West);
        }
        if (y > 0)
        {
            SetNeigbours(_fields[i], _fields[i - MapSize], FieldDirection.South);
            if (x > 0)
            {
                SetNeigbours(_fields[i], _fields[i - MapSize - 1], FieldDirection.WestSouth);
            }
            if (x < MapSize - 1)
            {
                SetNeigbours(_fields[i], _fields[i - MapSize + 1], FieldDirection.EastSouth);
            }
        }
    }

    void SetNeigbours(FieldUI currentFieldData, FieldUI neigbour, FieldDirection neigbourDirection)
    {
        currentFieldData.NeigbourComponent.SetNeigbours(neigbourDirection, neigbour);
        neigbour.NeigbourComponent.SetNeigbours(neigbourDirection.Opposite(), currentFieldData);
    }

    void GenerateMines(FieldUI skipField)
    {

        while (_minesLeft > 0)
        {
            int rand = Random.Range(0, MapSize * MapSize);

            if (!_fields[rand].FieldData.IsMine)
            {
                if (_fields[rand] != skipField)
                {
                    _fields[rand].FieldData.IsMine = true;
                    _minesLeft--;
                }

            }
        }
        foreach (FieldUI fieldUI in _fields)
        {
            fieldUI.SetMineInfo();
        }
    }

    void GameOver()
    {
        foreach (FieldUI fieldUI in _fields)
        {
            if (fieldUI.FieldData.IsMine)
            {
                fieldUI.GameOverOpening();
            }
        }
    }

}
