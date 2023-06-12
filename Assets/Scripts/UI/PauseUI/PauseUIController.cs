using Services;

public class PauseUIController : UIController
{
    private GameManager gameManager;
    private SceneController sceneController;
    private SkillUIController skillUIController;
    private UIScaleExpander scaleExpander;
    private NormalButton[] buttons;

    public override bool Visible
    {
        get => base.Visible;
        set
        {
            base.Visible = value;
            skillUIController.Visible = value;
        }
    }

    protected override void Start()
    {
        base.Start();
        gameManager = ServiceLocator.Get<GameManager>();
        sceneController = ServiceLocator.Get<SceneController>();
        skillUIController = UIControllerLocator.Get<SkillUIController>();
        scaleExpander = new UIScaleExpander(transform.GetChild(0), 0f, 1f);
        buttons = GetComponentsInChildren<NormalButton>();
    }

    public void PauseGame()
    {
        if (!Visible)
        {
            Visible = true;
            foreach (var button in buttons) button.ResetButton();
            //scaleExpander.ResetScale();
            //scaleExpander.ExpandScale();
            scaleExpander.SetScale(1f);
        }
    }

    public void ResumeGame()
    {
        if (Visible)
        {
            alphaTimer.OnComplete += AfterClickOnCompleteAlphaTimer;
            Visible = false;
        }
    }

    private void AfterClickOnCompleteAlphaTimer()
    {
        gameManager.ClosePauseUI();
        alphaTimer.OnComplete -= AfterClickOnCompleteAlphaTimer;
    }

    public void ReturnToMenu()
    {
        sceneController.LoadScene(1);
        //sceneController.Quit();
    }
}
