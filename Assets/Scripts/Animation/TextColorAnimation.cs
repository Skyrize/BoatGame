using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "TweenAnimation/TextColorAnimation")]
public class TextColorAnimation : TweenAnimation
{
    [SerializeField] protected Color targetColor = Color.red;

    override public GeneratedAnimation GenerateAnimation(Transform target) {
        Sequence animation = DOTween.Sequence();
        TMPro.TMP_Text text = target.GetComponent<TMPro.TMP_Text>();
        if (!text)
            throw new MissingComponentException("Missing TMP_Text component for animation.");
        if (reverse) {
            animation.Append(text.DOColor(targetColor, duration / 2));
            animation.Append(text.DOColor(text.color, duration / 2));
        } else {
            animation.Append(text.DOColor(targetColor, duration));
        }
        animation.SetAutoKill(false);
        animation.SetLoops(loops, loopType);
        animation.SetEase(generalEase);
        animation.Pause();
        return new GeneratedAnimation(animation, Name);
    }
}