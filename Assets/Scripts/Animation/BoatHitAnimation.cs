using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "TweenAnimation/BoatHitAnimation")]
public class BoatHitAnimation : TweenAnimation
{
    [Header("Settings (Boat Animation):")]
    [SerializeField] protected Vector3 punch = Vector3.one;
    [SerializeField] protected int vibrato = 10;
    [Range(0, 1)]
    [SerializeField] protected float elasticity = 1;

    override public GeneratedAnimation GenerateAnimation(Transform target) {
        Sequence animation = DOTween.Sequence();
        animation.Append(target.DOPunchRotation(punch, duration, vibrato, elasticity));
        // if (reverse) {
        //     animation.Append(target.DORotate(rotation, duration / 2));
        //     animation.Append(target.DORotate(target.localEulerAngles, duration / 2));
        // } else {
        //     animation.Append(target.DORotate(rotation, duration));
        // }
        animation.SetAutoKill(false);
        animation.SetLoops(loops, loopType);
        animation.SetEase(generalEase);
        animation.Pause();
        return new GeneratedAnimation(animation, Name);
    }
}
