using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Field : MonoBehaviour
{
    public bool isMine;
    //public int minesNear;

    public Text fieldValue;
    public GameObject buttton;

    public void CheckForMine()
    {
        buttton.SetActive(false);
        if (isMine)
        {
            Debug.Log("Game Over");
        }


    }


    public void SetValue(int index)
    {
        if(index == 9)
        {
            fieldValue.text = "X";
            return;
        }
        //imageValue.sprite = images[index];
        //minesNear = index;
        fieldValue.text = index.ToString();
    }
}
