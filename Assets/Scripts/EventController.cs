using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public Action OnGameOver;
    public Action<FieldUI> OnFirstFieldOpened;
    public Action OnRestartGame;
    public Action OnOpenedField;
    public Action OnVictory;

    public void GameOver()
    {
        OnGameOver?.Invoke();
    }

    public void FirstFieldOpened(FieldUI skipField)
    {
        OnFirstFieldOpened?.Invoke(skipField);
    }

    public void RestartTheGame()
    {
        OnRestartGame?.Invoke();
    }

    public void OpenedField()
    {
        OnOpenedField?.Invoke();
    }

    public void Victory()
    {
        OnVictory?.Invoke();
    }
}
