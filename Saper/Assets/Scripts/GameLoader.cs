using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class GameLoader : MonoBehaviour

{
    public Camera mainCamera;
    public EventManager eventManager;
    public MapOfFields mapOfFields;
    public UIManager uiManager;
    void Awake()
    {
        mapOfFields.Init(eventManager, mainCamera);
        uiManager.Init(eventManager, mapOfFields);
    }
}
