using MyTimer;
using Services;
using UnityEngine;
using UnityEngine.UI;

public class TimerUIController : MonoBehaviour
{
    private TimerOnly gameTimer;

    private Text text;

    private void Start()
    {
        gameTimer = ServiceLocator.Get<GameManager>().gameTimer;
        text = GetComponent<Text>();
    }

    private void Update()
    {
        text.text = $"{Mathf.FloorToInt(gameTimer.Current) / 60:0}:{gameTimer.Current % 60:00.}";
    }
}
