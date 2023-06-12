using Services;
using UnityEngine;
using UnityEngine.UI;

public class ReminderUIController : MonoBehaviour
{
    private ShadowManager shadowManager;
    private GameManager gameManager;

    public Text text;
    private CanvasGroup canvasGroup;

    private bool reminderOn;
    public bool ReminderOn
    {
        get => reminderOn;
        set
        {
            reminderOn = value;
            canvasGroup.alpha = reminderOn ? 1f : 0f;
        }
    }

    private void Start()
    {
        if (text == null)
            Debug.LogError("ReminderUIController的text未设置");
        shadowManager = ServiceLocator.Get<ShadowManager>();
        gameManager = ServiceLocator.Get<GameManager>();
        canvasGroup = GetComponent<CanvasGroup>();

        ReminderOn = true;
        UpdateText();
    }

    // 测试用所以直接放在Update里了，后续会删掉
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ReminderOn = !ReminderOn;
        }
        if (ReminderOn) UpdateText();
    }

    public void UpdateText()
    {
        text.text = "关闭/打开提示(Tab)\n重置玩家所有状态(M)\n" +
            "升级或移除技能 [白黄橙紫]\n(数字1-8/Shift+1-8)\n" +
            "玩家直接升一级(P)\n" +
            $"影子充能上限(Q/E)：{shadowManager.shadowChargeTimer.MaxShadowChargeCount}\n" +
            $"影子充能时间(R/T)：{shadowManager.shadowChargeTimer.ShadowChargeCD:0.#}s\n" +
            $"影子滞留时间(Y/U)：{shadowManager.shadowInfo.stayingDuration:0.#}s\n" +
            $"影子基础伤害(F/G)：{shadowManager.shadowInfo.Damage:0.#}\n" +
            $"自动生成敌人(Space)：{(gameManager.enemySpawnOn ? "开" : "关")}\n" +
            $"生成单个测试用手(B)\n生成单个测试用大眼怪(N)\n生成单个测试用自爆怪(V)\n" +
            $"生成测试用轨道敌群(J)\n生成测试用椭圆敌群(K)\n生成测试用螺旋敌群(L)";
    }
}
