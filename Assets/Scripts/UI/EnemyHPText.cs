using UnityEngine;

// ���ڲ��Եĵ���Ѫ����ʾ��������ɾ�������Ҫ��ʾ����Ѫ������UGUI
public class EnemyHPText : MonoBehaviour
{
    private TextMesh textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMesh>();
        GetComponentInParent<EnemyStatusInfo>().OnChangeHP += UpdateText;
    }

    public void UpdateText(float _, float HP)
    {
        textMesh.text = Mathf.RoundToInt(HP).ToString();
    }
}
