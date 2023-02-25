using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class GameLoader : MonoBehaviour

{
    public Camera MainCamera;
    public EventController EventControll;
    public MapOfFields MapOfFields;
    public UIManager UiManager;
    void Awake()
    {
        MapOfFields.Init(EventControll, MainCamera);
        UiManager.Init(EventControll, MapOfFields);
    }
}
