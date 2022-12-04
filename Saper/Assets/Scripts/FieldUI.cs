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
    public Dictionary<FieldDirection, FieldUI> neigbours;
    EventManager eventManager;
    public FieldUI()
    {
        neigbours = new Dictionary<FieldDirection, FieldUI>();
    }
    public void Init(EventManager manager)
    {
        eventManager = manager;
        eventManager.restartGame += ResetData;
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
            int mineCount = GetNeigboursMineCount();
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
    }
    void CheckFieldStatus()
    {
        if (fieldData.isMine) eventManager.GameOver();
        else if (fieldData.minesNearField == 0) CheckNeignboursField();
    }
    private void OnDestroy()
    {
        eventManager.restartGame -= ResetData;
    }
}
