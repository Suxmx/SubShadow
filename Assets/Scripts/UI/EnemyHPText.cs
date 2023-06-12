using UnityEngine;

// 用于测试的敌人血量显示，后续会删，如果需要显示敌人血量改用UGUI
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
