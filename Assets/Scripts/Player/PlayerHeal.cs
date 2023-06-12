using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeal : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator animator;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        GetComponentInParent<PlayerStatusInfo>().OnGetHealed += PlayHealAnimation;
    }

    private void PlayHealAnimation()
    {
        sr.enabled = true;
        animator.Play(0, 0, 0f);
    }

    public void ResetHealAnimator()
    {
        sr.enabled = false;
    }
}
