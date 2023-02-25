using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FieldUI : MonoBehaviour, IPointerClickHandler
{
    public static GraphicRaycaster GraphicRaycaster;

    public Text FieldValue;
    public GameObject Buttton;
    public GameObject Flag;
    public FieldData FieldData;
    public NeigbourComponent NeigbourComponent;

    bool _isFlagged;
    EventController _eventManager;

    public void Init(EventController manager, FieldData fieldData, NeigbourComponent neigbourComponent)
    {
        _eventManager = manager;
        _eventManager.OnRestartGame += ResetData;
        this.FieldData = fieldData;
        this.NeigbourComponent = neigbourComponent;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OpenField();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            SwitchFlagStatus();
        }
    }

    public void OpenField()
    {
        if (!Buttton.activeInHierarchy)
        {
            return;
        }
        Buttton.SetActive(false);
        CheckFieldStatus();
    }

    public void SwitchFlagStatus()
    {
        _isFlagged = !_isFlagged;
        Flag.SetActive(_isFlagged);
    }

    public void SetMineInfo()
    {
        if (FieldData.IsMine)
        {
            FieldValue.text = "X";
        }
        else
        {
            int mineCount = NeigbourComponent.GetNeigboursMineCount();
            if (mineCount > 0)
            {
                FieldValue.text = mineCount.ToString();
            }
            else FieldValue.text = "";
        }
    }

    public void FirstFieldOpening()
    {
        GraphicRaycaster.enabled = false;
        _eventManager.OnFirstFieldOpened(this);
        OpenField();
    }

    public void GameOverOpening()
    {
        Buttton.SetActive(false);
    }

    void ResetData()
    {
        Buttton.SetActive(true);
        Flag.SetActive(false);
    }

    void CheckFieldStatus()
    {
        if (FieldData.IsMine)
        {
            _eventManager.GameOver();
            return;
        }
        else if (FieldData.MinesNearField == 0)
        {
            NeigbourComponent.CheckNeignboursField();
        }
        _eventManager.OnOpenedField();
    }

    void OnDestroy()
    {
        _eventManager.OnRestartGame -= ResetData;
    }
}
