using Services;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Whirl : Skill
{
    private readonly WhirlInfo whirlInfo;
    private readonly Dictionary<Shadow, List<Whirl>> whirls;
    private readonly ObjectManager objectManager;

    public Skill_Whirl(SkillInfo skillInfo, SkillInfoData skillInfoData, EventSystem eventSystem) 
        : base(skillInfo, skillInfoData, eventSystem, true)
    {
        whirlInfo = new WhirlInfo
        {
            whirlLength = skillInfoData.whirlLength_Whirl,
            whirlAngularVelocity = skillInfoData.whirlAngularVelocity1_Whirl,
            whirlDamage = skillInfoData.whirlDamage1_Whirl,
        };
        whirls = new Dictionary<Shadow, List<Whirl>>();
        objectManager = ServiceLocator.Get<ObjectManager>();
    }

    protected override void OnUpgradeSkillGrade1()
    {
        eventSystem.AddListener<Shadow>(EEvent.AfterCreateShadow, GenerateWhirl1);
        eventSystem.AddListener<Shadow>(EEvent.BeforeRecallShadow, RemoveWhirl);
    }

    protected override void OnRemoveSkillGrade1()
    {
        eventSystem.RemoveListener<Shadow>(EEvent.AfterCreateShadow, GenerateWhirl1);
        eventSystem.RemoveListener<Shadow>(EEvent.BeforeRecallShadow, RemoveWhirl);
        foreach (var whirlList in whirls.Values)
        {
            foreach (var whirl in whirlList)
            {
                whirl.DestroySelf();
            }
            whirlList.Clear();
        }
        whirls.Clear();
    }

    private void GenerateWhirl1(Shadow shadow)
    {
        float angle = Random.Range(0f, 360f);
        Whirl whirl = objectManager.Activate(EObject.Whirl, shadow.centerTransform.position, 
            shadow.transform).Transform.GetComponent<Whirl>();
        whirl.Initialize(whirlInfo, angle, shadow);
        whirls.Add(shadow, new List<Whirl> { whirl });
    }

    private void RemoveWhirl(Shadow shadow)
    {
        if (whirls.ContainsKey(shadow))
        {
            foreach (var whirl in whirls[shadow])
            {
                whirl.DestroySelf();
            }
            whirls[shadow].Clear();
            whirls.Remove(shadow);
        }
    }

    protected override void OnUpgradeSkillGrade2()
    {
        whirlInfo.whirlDamage = skillInfoData.whirlDamage2_Whirl;
    }

    protected override void OnRemoveSkillGrade2()
    {
        whirlInfo.whirlDamage = skillInfoData.whirlDamage1_Whirl;
    }

    protected override void OnUpgradeSkillGrade3()
    {
        whirlInfo.whirlAngularVelocity = skillInfoData.whirlAngularVelocity2_Whirl;
    }

    protected override void OnRemoveSkillGrade3()
    {
        whirlInfo.whirlAngularVelocity = skillInfoData.whirlAngularVelocity1_Whirl;
    }

    protected override void OnUpgradeSkillGrade4()
    {
        eventSystem.RemoveListener<Shadow>(EEvent.AfterCreateShadow, GenerateWhirl1);
        eventSystem.AddListener<Shadow>(EEvent.AfterCreateShadow, GenerateWhirl2);
    }

    protected override void OnRemoveSkillGrade4()
    {
        eventSystem.RemoveListener<Shadow>(EEvent.AfterCreateShadow, GenerateWhirl2);
        eventSystem.AddListener<Shadow>(EEvent.AfterCreateShadow, GenerateWhirl1);
    }

    private void GenerateWhirl2(Shadow shadow)
    {
        float angle = Random.Range(0f, 360f);
        Whirl whirl1 = objectManager.Activate(EObject.Whirl, shadow.centerTransform.position,
            shadow.transform).Transform.GetComponent<Whirl>();
        whirl1.Initialize(whirlInfo, angle, shadow);
        Whirl whirl2 = objectManager.Activate(EObject.Whirl, shadow.centerTransform.position,
            shadow.transform).Transform.GetComponent<Whirl>();
        whirl2.Initialize(whirlInfo, angle + 180f, shadow);
        whirls.Add(shadow, new List<Whirl> { whirl1, whirl2 });
    }
}
