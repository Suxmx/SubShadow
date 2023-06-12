using Services;

public class HUD : UIController
{
    public PlayerHPUIController PlayerHPUIController { get; private set; }
    public ShadowUIController ShadowUIController { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        PlayerHPUIController = GetComponentInChildren<PlayerHPUIController>();
        ShadowUIController = GetComponentInChildren<ShadowUIController>();
    }

    protected override void Start()
    {
        base.Start();
        eventSystem.AddListener<int>(EEvent.AfterLoadScene, AfterLoadScene);
    }

    protected override void BeforeLoadScene(int _)
    {
        base.BeforeLoadScene(_);
        ShadowUIController.ResetUI();
    }

    private void AfterLoadScene(int _)
    {
        if (SceneController.InGame)
        {
            ShadowUIController.Initialize();
            Visible = true;
        }
    }
}
