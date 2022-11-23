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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) OpenField();
        else if (eventData.button == PointerEventData.InputButton.Right) SwitchFlagStatus();
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
            else fieldValue.text = "";
        }
    }
    public void FirstFieldOpening()
    {
        graphicRaycaster.enabled = false;
        EventManager.instance.firstFieldOpened(this);
        OpenField();
    }
    public void GameOverOpening()
    {
        buttton.SetActive(false);
    }
    void Start()
    {
        EventManager.instance.restartGame += ResetData;
    }
    void ResetData()
    {
        buttton.SetActive(true);
    }
    void CheckFieldStatus()
    {
        if (fieldData.isMine) EventManager.instance.GameOver();
        else if (fieldData.minesNearField == 0) fieldData.CheckNeignboursField();
    }
    private void OnDestroy()
    {
        EventManager.instance.restartGame -= ResetData;
    }
}
