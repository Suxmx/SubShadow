using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    protected Animator animator;
    [SerializeField, Label("¶¯»­Ëø")]
    protected Lock animatorLock;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        animatorLock = new Lock(LockAnimator, UnlockAnimator);
    }

    public void AddAnimatorLock()
    {
        animatorLock++;
    }

    public void RemoveAnimatorLock()
    {
        if (!animatorLock.Unlocked)
            animatorLock--;
    }

    protected virtual void LockAnimator()
    {
        animator.speed = 0f;
    }

    protected virtual void UnlockAnimator()
    {
        animator.speed = 1f;
    }

    public virtual void ResetAnimator()
    {
        animatorLock.Reset();
    }
}
