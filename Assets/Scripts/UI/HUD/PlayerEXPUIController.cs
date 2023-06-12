using MyTimer;
using Services;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEXPUIController : MonoBehaviour
{
    public Image image;
    public Text currentEXPText;
    public Text maxEXPText;
    public Text gradeText;

    private EXPScrollTimer scrollTimer;
    private PlayerStatusInfo playerStatusInfo;

    private void Start()
    {
        if (image == null || currentEXPText == null || maxEXPText == null || gradeText == null)
            Debug.LogError("PlayerEXPUIController的外部引用未设置");

        scrollTimer = new EXPScrollTimer(image);
        playerStatusInfo = ServiceLocator.Get<Player>().PlayerStatusInfo;

        playerStatusInfo.OnChangeEXP += OnChangeEXP;
        playerStatusInfo.OnChangePlayerGrade += OnChangePlayerGrade;
    }

    public void OnChangeEXP(float exp)
    {
        currentEXPText.text = $"EXP: {Mathf.RoundToInt(exp)}";
        scrollTimer.Initialize(exp / playerStatusInfo.maxEXP);
    }

    public void OnChangePlayerGrade(int before, int after)
    {
        gradeText.text = $"LV.{after}";
        maxEXPText.text = $" / {Mathf.RoundToInt(playerStatusInfo.maxEXP)}";
        scrollTimer.ChangeScrollCount(Mathf.Max(after - before, 0), 
                playerStatusInfo.EXP / playerStatusInfo.maxEXP);
    }
}

public class EXPScrollTimer : EaseFloatTimer
{
    private readonly Image image;
    private readonly float avgSpeed;
    private float ultimateTarget;
    private int scrollCount;

    public EXPScrollTimer(Image image, EaseType easeType = EaseType.Linear, 
        float avgSpeed = 0.5f) : base(true, easeType)
    {
        this.image = image;
        this.avgSpeed = avgSpeed;
        scrollCount = 0;
        OnTick += x => image.fillAmount = x;
    }

    public void Initialize(float target)
    {
        if (scrollCount == 0) SetTimer(image.fillAmount, target);
        else ultimateTarget = target;
    }

    public void ChangeScrollCount(int toChange, float target)
    {
        if (toChange == 0) return;
        if (scrollCount == 0 && Paused)
        {
            ultimateTarget = target;
            SetTimer(image.fillAmount, toChange > 0 ? 1f : 0f);
        }
        if (toChange > 0) while (toChange-- > 0) AddOneScrollCount();
        else while (toChange++ < 0) ReduceOneScrollCount();
    }

    private void SetTimer(float origin, float target)
    {
        Initialize(origin, target, avgSpeed * Mathf.Abs(target - origin));
    }

    private void AddOneScrollCount(float target = 0f)
    {
        // 这样反复调整OnComplete比起在OnTick中判断性能更好
        switch (scrollCount)
        {
            case -2:
                OnComplete -= ChangeFromOneToZero;
                OnComplete += ChangeFromOne;
                break;
            case -1:
                OnComplete -= ChangeFromOne;
                break;
            case 0:
                OnComplete += ChangeFromZero;
                if (Paused)
                {
                    ultimateTarget = target;
                    SetTimer(image.fillAmount, 1f);
                }
                break;
            case 1:
                OnComplete -= ChangeFromZero;
                OnComplete += ChangeFromZeroToOne;
                break;
        }
        scrollCount++;
    }

    private void ReduceOneScrollCount(float target = 0f)
    {
        switch (scrollCount)
        {
            case 2:
                OnComplete -= ChangeFromZeroToOne;
                OnComplete += ChangeFromZero;
                break;
            case 1:
                OnComplete -= ChangeFromZero;
                break;
            case 0:
                OnComplete += ChangeFromOne;
                if (Paused)
                {
                    ultimateTarget = target;
                    SetTimer(image.fillAmount, 0f);
                }
                break;
            case -1:
                OnComplete -= ChangeFromOne;
                OnComplete += ChangeFromOneToZero;
                break;
        }
        scrollCount--;
    }

    private void ChangeFromZeroToOne()
    {
        SetTimer(0f, 1f);
        ReduceOneScrollCount();
    }

    private void ChangeFromZero()
    {
        SetTimer(0f, ultimateTarget);
        ReduceOneScrollCount();
    }

    private void ChangeFromOneToZero()
    {
        SetTimer(1f, 0f);
        AddOneScrollCount();
    }

    private void ChangeFromOne()
    {
        SetTimer(1f, ultimateTarget);
        AddOneScrollCount();
    }
}
