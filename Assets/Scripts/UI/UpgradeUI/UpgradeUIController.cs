using Services;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeUIController : UIController
{
    private Player player;
    private GameManager gameManager;
    private SkillManager skillManager;
    private SkillUIController skillUIController;
    private List<Animator> animators;
    private List<NormalButton> buttons;
    private float animatorTime;

    private UIScaleExpander scaleExpander;
    private List<UpgradeSkillUI> upgradeSkillUIs;
    public UpgradeSkillUI currentSkillUI;
    public UpgradeSkillHoverUI UpgradeSkillHoverUI { get; private set; }

    public override bool Visible
    {
        get => base.Visible;
        set
        {
            base.Visible = value;
            skillUIController.Visible = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        upgradeSkillUIs = GetComponentsInChildren<UpgradeSkillUI>().ToList();
        UpgradeSkillHoverUI = GetComponentInChildren<UpgradeSkillHoverUI>();
        scaleExpander = new UIScaleExpander(transform.GetChild(1), 0f, 1f);
        animators = GetComponentsInChildren<Animator>().ToList();
        buttons = GetComponentsInChildren<NormalButton>().ToList();
        animatorTime = animators[0].runtimeAnimatorController.animationClips[0].length;
        fadeTime = 0.01f;
    }

    protected override void Start()
    {
        base.Start();
        player = ServiceLocator.Get<Player>();
        gameManager = ServiceLocator.Get<GameManager>();
        skillManager = ServiceLocator.Get<SkillManager>();
        skillUIController = UIControllerLocator.Get<SkillUIController>();
    }

    public void Upgrade()
    {
        ResetUI();
        Visible = true;
        transform.GetChild(0).position = Camera.main.WorldToScreenPoint(player.transform.position);
        animators.ForEach(x => x.Play(0, 0, 0));
        StartCoroutine(PlayAnimator());
    }

    private IEnumerator PlayAnimator()
    {
        yield return new WaitForSecondsRealtime(animatorTime);
        ShowUsableSkills();
    }

    private void ShowUsableSkills()
    {
        currentSkillUI = null;
        List<Skill> usableSkills = skillManager.GetUsableSkills();

        if (usableSkills != null && usableSkills.Count > 0)
        {
            scaleExpander.ExpandScale();
            int i = 0;
            for (; i < usableSkills.Count; i++)
            {
                upgradeSkillUIs[i].gameObject.SetActive(true);
                upgradeSkillUIs[i].Initialize(usableSkills[i]);
            }
            for (; i < 3; i++) upgradeSkillUIs[i].gameObject.SetActive(false);
        }
        else
        {
            Visible = false;
            gameManager.CloseUpgradeUI();
        }
    }

    public void ConfirmUpgradeSkill()
    {
        if (currentSkillUI != null)
        {
            currentSkillUI.skill.UpgradeSkill();
            alphaTimer.OnComplete += AfterChooseOnCompleteAlphaTimer;
            Visible = false;
        }
    }

    public void SkipUpgradeSkill()
    {
        alphaTimer.OnComplete += AfterChooseOnCompleteAlphaTimer;
        Visible = false;
    }

    private void AfterChooseOnCompleteAlphaTimer()
    {
        gameManager.CloseUpgradeUI();
        alphaTimer.OnComplete -= AfterChooseOnCompleteAlphaTimer;
    }

    private void ResetUI()
    {
        upgradeSkillUIs.ForEach(x => x.ResetUI());
        UpgradeSkillHoverUI.Hide();
        scaleExpander.ResetScale();
        buttons.ForEach(x => x.ResetButton());
    }
}
