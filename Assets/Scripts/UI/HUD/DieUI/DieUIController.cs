using MyTimer;
using Services;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class DieUIController : UIController
{
    private GameManager gameManager;
    private SceneController sceneController;
    private HUD hud;
    private UIScaleExpander scaleExpander;
    private NormalButton[] buttons;
    private Image backgroundImage;
    public Image gameOverImage;
    private Text[] texts;
    private Animator animator;
    private EaseFloatTimer fadeInTimer;

    public override bool Visible
    {
        get => base.Visible;
        set
        {
            base.Visible = value;
            if (value)
            {
                animator.Play(0, 0, 0);
                fadeInTimer.Restart();
            }
        }
    }

    protected override void Start()
    {
        if (gameOverImage == null)
        {
            Debug.LogError("DieUIController的外部引用未设置");
        }
        base.Start();
        gameManager = ServiceLocator.Get<GameManager>();
        sceneController = ServiceLocator.Get<SceneController>();
        hud = UIControllerLocator.Get<HUD>();
        scaleExpander = new UIScaleExpander(transform.GetChild(0), 0f, 1f);
        buttons = GetComponentsInChildren<NormalButton>();
        backgroundImage = transform.GetChild(0).GetComponent<Image>();
        animator = GetComponentInChildren<Animator>();
        texts = GetComponentsInChildren<Text>();
        fadeInTimer = new EaseFloatTimer(true, EaseType.OutCubic);
        fadeInTimer.Initialize(0f, 1f, 2f, false);
        fadeInTimer.OnTick += x =>
        {
            backgroundImage.color = gameOverImage.color = new Color(1f, 1f, 1f, x);
            texts.ForEach(t => t.color = new Color(t.color.r, t.color.g, t.color.b, x));
        };
    }

    public void PlayerDie()
    {
        if (!Visible)
        {
            Visible = true;
            foreach (var button in buttons) button.ResetButton();
            //scaleExpander.ResetScale();
            //scaleExpander.ExpandScale();
            scaleExpander.SetScale(1f);
            hud.Visible = false;
        }
    }

    public void TryAgain()
    {
        sceneController.LoadScene(2);
    }

    public void ReturnToMenu()
    {
        sceneController.LoadScene(1);
        //sceneController.Quit();
    }

    public void ResumeGame()
    {
        if (Visible)
        {
            alphaTimer.OnComplete += AfterClickOnCompleteAlphaTimer;
            Visible = false;
            hud.Visible = true;
        }
    }

    private void AfterClickOnCompleteAlphaTimer()
    {
        gameManager.CloseDieUI();
        alphaTimer.OnComplete -= AfterClickOnCompleteAlphaTimer;
    }
}
