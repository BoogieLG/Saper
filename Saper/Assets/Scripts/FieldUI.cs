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
    }
    public void SwitchFlagStatus()
    {
        isFlagged = !isFlagged;
        flag.SetActive(isFlagged);
    }
    public void SetMineInfo()
    {
        if (fieldData.isMine) fieldValue.text = "X";

        else fieldValue.text = fieldData.GetNeigboursMineCount().ToString();
    }
}
