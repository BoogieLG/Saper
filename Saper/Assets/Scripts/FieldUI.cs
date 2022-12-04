using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FieldUI : MonoBehaviour, IPointerClickHandler
{
    public Text fieldValue;
    public GameObject buttton;
    public GameObject flag;
    public FieldData fieldData;
    public static GraphicRaycaster graphicRaycaster;
    bool isFlagged;
    public NeigbourComponent neigbourComponent;

    EventManager eventManager;
    public void Init(EventManager manager, FieldData fieldData, NeigbourComponent neigbourComponent)
    {
        eventManager = manager;
        eventManager.restartGame += ResetData;
        this.fieldData = fieldData;
        this.neigbourComponent = neigbourComponent;
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
        buttton.SetActive(false);
        CheckFieldStatus();
    }
    public void SwitchFlagStatus()
    {
        isFlagged = !isFlagged;
        flag.SetActive(isFlagged);
    }
    public void SetMineInfo()
    {
        if (fieldData.isMine)
        {
            fieldValue.text = "X";
        }
        else
        {
            int mineCount = neigbourComponent.GetNeigboursMineCount();
            if (mineCount > 0)
            {
                fieldValue.text = mineCount.ToString();
            }
            else fieldValue.text = "";
        }
    }
    public void FirstFieldOpening()
    {
        graphicRaycaster.enabled = false;
        eventManager.firstFieldOpened(this);
        OpenField();
    }
    public void GameOverOpening()
    {
        buttton.SetActive(false);
    }
    void ResetData()
    {
        buttton.SetActive(true);
        flag.SetActive(false);
    }
    void CheckFieldStatus()
    {
        if (fieldData.isMine) eventManager.GameOver();
        else if (fieldData.minesNearField == 0) neigbourComponent.CheckNeignboursField();
    }
    private void OnDestroy()
    {
        eventManager.restartGame -= ResetData;
    }
}
