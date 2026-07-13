using UnityEngine;

public enum PlayerAnimationState
{
    Idel,
    FrontWalk,
    BackWalk,
    Skilled,
    ULT,
    Damaged,
    Die
}

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;

    PlayerAnimationState current_animationState;


    private void Awake()
    {
        animator = GetComponent<Animator>();

        current_animationState = PlayerAnimationState.Idel;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A) && current_animationState != PlayerAnimationState.BackWalk)
        {
            current_animationState = PlayerAnimationState.BackWalk;
            PlayBackWalk();
        }


        if(Input.GetKeyUp(KeyCode.A) && current_animationState != PlayerAnimationState.Idel)
        {
            current_animationState = PlayerAnimationState.Idel;
            PlayIdle();
        }
    }

    public void PlayBackWalk()
    {
        animator.CrossFade("Base Layer.OwnerBackWalk", 0.1f);
    }

    public void PlayIdle()
    {
        animator.CrossFade("Base Layer.OwnerIdle", 0.1f);
    }
}
