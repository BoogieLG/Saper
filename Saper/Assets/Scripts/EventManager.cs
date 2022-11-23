using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    public Action gameOver;
    public Action<FieldUI> firstFieldOpened;
    public Action restartGame;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void GameOver()
    {
        gameOver?.Invoke();
    }
    public void FirstFieldOpened(FieldUI skipField)
    {
        firstFieldOpened?.Invoke(skipField);
    }
    public void RestartTheGame()
    {
        restartGame?.Invoke();
    }
}
