using UnityEngine;
using UnityEngine.UI;

public class Field : MonoBehaviour
{
    public Text fieldValue;
    public GameObject buttton;

    public FieldData fieldData;

    public void OpenField()
    {
        buttton.SetActive(false);
    }
    public void SetMineInfo()
    {
        if (fieldData.isMine) fieldValue.text = "X";

        else fieldValue.text = fieldData.GetNeigboursMineCount().ToString();
    }
}
