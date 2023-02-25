using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Field Parameters References")]
    public GameObject UIMapParametrs;
    public TextMeshProUGUI FieldSizeText;
    public Slider FieldSizeSlider;
    public TextMeshProUGUI MineCountText;
    public Slider MineCountSlider;

    [Header("InGame UI References")]
    public TextMeshProUGUI TimerText;
    public GameObject RestartConteiner;
    public TextMeshProUGUI ResultTimerText;
    public TextMeshProUGUI ResultStatus;

    bool _isStopped = true;
    float _timer;

    MapOfFields _mapOfFields;
    EventController _eventManager;

    public void Init(EventController eventManager, MapOfFields map)
    {
        _eventManager = eventManager;
        _mapOfFields = map;
        eventManager.OnGameOver += GameOver;
        eventManager.OnVictory += Victory;
    }

    public void StartGame()
    {
        _isStopped = false;
        _mapOfFields.StartGame();
        UIMapParametrs.SetActive(false);
    }

    public void SetFieldSize()
    {
        int value = (int)FieldSizeSlider.value;
        _mapOfFields.MapSize = value;
        SetMineMinMaxCount(value);
        UpdateParametersUI();
    }

    public void SetMineCount()
    {
        _mapOfFields.MineCount = (int)MineCountSlider.value;
        UpdateParametersUI();
    }

    public void SetMineMinMaxCount(int value)
    {
        MineCountSlider.minValue = (int)(value * value) / 5;
        MineCountSlider.maxValue = (int)(value * value) / 5 * 2;
        if (MineCountSlider.value < MineCountSlider.minValue)
        {
            MineCountSlider.value = MineCountSlider.minValue;
        }
        else if (MineCountSlider.value > MineCountSlider.maxValue)
        {
            MineCountSlider.value = MineCountSlider.maxValue;
        }
    }

    public void ChangeParameters()
    {
        _mapOfFields.ClearField();
        UIMapParametrs.SetActive(true);
        RestartConteiner.SetActive(false);
    }

    public void RestartGame()
    {
        _isStopped = false;
        RestartConteiner.SetActive(false);
        _mapOfFields.Restart();
    }

    void Update()
    {
        if (_isStopped)
        {
            return;
        }
        UpdateTimer();
    }

    void UpdateTimer()
    {
        _timer += Time.deltaTime;
        TimerText.text = _timer.ToString("0.00") + "s";
    }

    void GameOver()
    {
        ResultStatus.text = "<color=red>Lose!</color>";
        _isStopped = true;
        RestartConteiner.SetActive(true);
        ResultTimerText.text = _timer.ToString("0.00") + "s";
        TimerText.text = null;
        _timer = 0;
    }

    void Victory()
    {
        ResultStatus.text = "<color=green>Win!</color>";
        _isStopped = true;
        RestartConteiner.SetActive(true);
        ResultTimerText.text = _timer.ToString("0.00") + "s";
        TimerText.text = null;
        _timer = 0;
    }

    void UpdateParametersUI()
    {
        FieldSizeText.text = FieldSizeSlider.value.ToString();
        MineCountText.text = MineCountSlider.value.ToString();
    }

    void OnDestroy()
    {
        _eventManager.OnGameOver -= GameOver;
        _eventManager.OnVictory -= Victory;
    }
}
