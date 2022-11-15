using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FieldUI : MonoBehaviour, IPointerClickHandler
{
    public Text fieldValue;
    public GameObject buttton;
    public GameObject flag;
    public FieldData fieldData;

    bool isFlagged;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left) OpenField();
        else if(eventData.button == PointerEventData.InputButton.Right) SwitchFlagStatus();
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
        if (fieldData.isMine) fieldValue.text = "X";
        else
        {
            int mineCount = fieldData.GetNeigboursMineCount();
            if (mineCount > 0) fieldValue.text = mineCount.ToString();
        }
    }
    void CheckFieldStatus()
    {
        if (fieldData.isMine) EventManager.instance.GameOver();
        else if (fieldData.minesNearField == 0) fieldData.CheckNeignboursField();
    }

    public void GameOverOpening()
    {
        buttton.SetActive(false);
    }
}
