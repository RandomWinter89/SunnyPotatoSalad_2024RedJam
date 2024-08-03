using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimatorExtensions 
{
    public static void PlayLastFrame(this Animator anim)
    {
        // Assuming you're interested in the Base Layer (index 0)
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        // Get the clip info for the current state
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);

        if (clipInfo.Length > 0)
        {
            AnimationClip currentClip = clipInfo[0].clip;
            float normalizedTime = (currentClip.length * currentClip.frameRate - 1) / (currentClip.length * currentClip.frameRate);
            anim.Play(stateInfo.fullPathHash, 0, normalizedTime);
        }
    }
}
