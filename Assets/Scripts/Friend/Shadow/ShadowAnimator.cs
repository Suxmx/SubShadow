using UnityEngine;

public enum EShadowMotion
{
    /// <summary>
    /// ���ö���
    /// </summary>
    Set,
    /// <summary>
    /// ���춯��
    /// </summary>
    Create,
    /// <summary>
    /// ��������
    /// </summary>
    Stay,
}

public class ShadowAnimator : MonoBehaviour
{
    private Shadow shadow;
    private Animator animator;

    private Bijection<EShadowMotion, int> MotionAndId;

    private void Awake()
    {
        shadow = GetComponentInParent<Shadow>();
        animator = GetComponent<Animator>();

        MotionAndId = new Bijection<EShadowMotion, int>();
        System.Type type = typeof(EShadowMotion);
        foreach (EShadowMotion value in System.Enum.GetValues(type))
        {
            int hashId = Animator.StringToHash(System.Enum.GetName(type, value));
            MotionAndId.Add(value, hashId);
        }
    }

    public void PlayMotion(EShadowMotion motion)
    {
        animator.Play(MotionAndId[motion]);
    }

    public void PlayMotion(EShadowMotion motion, float normalizedTime)
    {
        animator.Play(MotionAndId[motion], 0, normalizedTime);
    }

    public void EndCreateShadow()
    {
        shadow.CompleteCreating();
    }
}
