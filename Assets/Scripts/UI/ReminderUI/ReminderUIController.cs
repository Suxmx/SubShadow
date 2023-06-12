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
            Debug.LogError("ReminderUIController��textδ����");
        shadowManager = ServiceLocator.Get<ShadowManager>();
        gameManager = ServiceLocator.Get<GameManager>();
        canvasGroup = GetComponent<CanvasGroup>();

        ReminderOn = true;
        UpdateText();
    }

    // ����������ֱ�ӷ���Update���ˣ�������ɾ��
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
        text.text = "�ر�/����ʾ(Tab)\n�����������״̬(M)\n" +
            "�������Ƴ����� [�׻Ƴ���]\n(����1-8/Shift+1-8)\n" +
            "���ֱ����һ��(P)\n" +
            $"Ӱ�ӳ�������(Q/E)��{shadowManager.shadowChargeTimer.MaxShadowChargeCount}\n" +
            $"Ӱ�ӳ���ʱ��(R/T)��{shadowManager.shadowChargeTimer.ShadowChargeCD:0.#}s\n" +
            $"Ӱ������ʱ��(Y/U)��{shadowManager.shadowInfo.stayingDuration:0.#}s\n" +
            $"Ӱ�ӻ����˺�(F/G)��{shadowManager.shadowInfo.Damage:0.#}\n" +
            $"�Զ����ɵ���(Space)��{(gameManager.enemySpawnOn ? "��" : "��")}\n" +
            $"���ɵ�����������(B)\n���ɵ��������ô��۹�(N)\n���ɵ����������Ա���(V)\n" +
            $"���ɲ����ù����Ⱥ(J)\n���ɲ�������Բ��Ⱥ(K)\n���ɲ�����������Ⱥ(L)";
    }
}
