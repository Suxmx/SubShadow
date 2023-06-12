using MyTimer;
using Services;
using UnityEngine;
using UnityEngine.UI;

public class WinUIController : UIController
{
    private SceneController sceneController;
    private HUD hud;
    private UIScaleExpander scaleExpander;
    private NormalButton[] buttons;
    private Image backgroundImage;
    public Image winImage;
    private Text text;
    private Animator animator;
    private EaseFloatTimer fadeInTimer;
    private Player player;

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
        if (winImage == null)
        {
            Debug.LogError("DieUIController���ⲿ����δ����");
        }
        base.Start();
        sceneController = ServiceLocator.Get<SceneController>();
        player = ServiceLocator.Get<Player>();
        hud = UIControllerLocator.Get<HUD>();
        scaleExpander = new UIScaleExpander(transform.GetChild(0), 0f, 1f);
        buttons = GetComponentsInChildren<NormalButton>();
        backgroundImage = transform.GetChild(0).GetComponent<Image>();
        animator = GetComponentInChildren<Animator>();
        text = GetComponentInChildren<Text>();
        fadeInTimer = new EaseFloatTimer(true, EaseType.OutCubic);
        fadeInTimer.Initialize(0f, 1f, 2f, false);
        fadeInTimer.OnTick += x =>
        {
            backgroundImage.color = winImage.color = new Color(1f, 1f, 1f, x);
            text.color = new Color(text.color.r, text.color.g, text.color.b, x);
        };
    }

    public void PlayerWin()
    {
        if (!Visible)
        {
            Visible = true;
            foreach (var button in buttons)
                button.ResetButton();
            //scaleExpander.ResetScale();
            //scaleExpander.ExpandScale();
            scaleExpander.SetScale(1f);
            hud.Visible = false;
        }
    }

    public void ReturnToMenu()
    {
        player.transform.Find("Body").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);//����ұ�ز�͸����ֹ��һ����Ϸʱ�����ʧ
        sceneController.LoadScene(1);
    }
}
