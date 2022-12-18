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
    public TextMeshProUGUI timerText;
    public GameObject restartConteiner;
    public TextMeshProUGUI resultTimerText;
    public TextMeshProUGUI resultStatus;

    bool isStopped = true;
    float timer;

    MapOfFields mapOfFields;
    EventManager eventManager;

    public void Init(EventManager eventManager, MapOfFields map)
    {
        this.eventManager = eventManager;
        mapOfFields = map;
        eventManager.gameOver += GameOver;
        eventManager.victory += Victory;
    }
    public void StartGame()
    {
        isStopped = false;
        mapOfFields.StartGame();
        UIMapParametrs.SetActive(false);
    }
    public void SetFieldSize()
    {
        int value = (int)FieldSizeSlider.value;
        mapOfFields.mapSize = value;
        SetMineMinMaxCount(value);
        UpdateParametersUI();
    }
    public void SetMineCount()
    {
        mapOfFields.MineCount = (int)MineCountSlider.value;
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
    void Update()
    {
        if (isStopped)
        {
            return;
        }
        UpdateTimer();
    }

    void UpdateTimer()
    {
        timer += Time.deltaTime;
        timerText.text = timer.ToString("0.00") + "s";
    }

    void GameOver()
    {
        resultStatus.text = "<color=red>Lose!</color>";
        isStopped = true;
        restartConteiner.SetActive(true);
        resultTimerText.text = timer.ToString("0.00") + "s";
        timerText.text = null;
        timer = 0;
    }
    void Victory()
    {
        resultStatus.text = "<color=green>Win!</color>";
        isStopped = true;
        restartConteiner.SetActive(true);
        resultTimerText.text = timer.ToString("0.00") + "s";
        timerText.text = null;
        timer = 0;
    }
    public void ChangeParameters()
    {
        mapOfFields.ClearField();
        UIMapParametrs.SetActive(true);
        restartConteiner.SetActive(false);
    }
    public void RestartGame()
    {
        isStopped = false;
        restartConteiner.SetActive(false);
        mapOfFields.Restart();
    }
    void UpdateParametersUI()
    {
        FieldSizeText.text = FieldSizeSlider.value.ToString();
        MineCountText.text = MineCountSlider.value.ToString();
    }
    void OnDestroy()
    {
        eventManager.gameOver -= GameOver;
        eventManager.victory -= Victory;
    }
}
