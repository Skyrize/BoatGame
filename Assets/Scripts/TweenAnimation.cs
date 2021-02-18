using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class ScaleAnimation
{
    protected Sequence animation = null;
    [SerializeField] protected string _name = "default name";
    [SerializeField] protected float duration = 1f;
    [SerializeField] protected Vector3 scale = new Vector3(2f, 2f, 2f);
    [SerializeField] protected bool reverse = true;
    [SerializeField] protected int loops = 0;
    [SerializeField] protected LoopType loopType = LoopType.Yoyo;
    public string name {
        get {
            return _name;
        }
    }

    public void Initialize(Transform target)
    {
        animation = DOTween.Sequence();
        if (reverse) {
            animation.Append(target.DOScale(scale, duration / 2));
            animation.Append(target.DOScale(target.localScale, duration / 2));
        } else {
            animation.Append(target.DOScale(scale, duration));
        }
        animation.SetAutoKill(false);
        animation.SetLoops(loops, loopType);
        animation.Pause();
        
    }

    public void Play()
    {
        animation.Restart();
    }
}

public class TweenAnimation : MonoBehaviour
{
    [Header("Animations")]
    [SerializeField]
    private int awakeAnimationIndex = -1;
    [SerializeField]
    private ScaleAnimation[] animations = null;

    // Start is called before the first frame update
    void Start()
    {
        foreach (ScaleAnimation animation in animations)
        {
            animation.Initialize(transform);
        }
        if (awakeAnimationIndex != -1) {
            Play(awakeAnimationIndex);
        }
    }

    public void Play(int animationIndex)
    {
        animations[animationIndex].Play();
    }

    public void Play(string animationName)
    {
        foreach (ScaleAnimation anim in animations)
        {
            if (anim.name == animationName) {
                anim.Play();
                return;
            }
        }
        Debug.LogWarning("Cant find animation named '" + animationName + "' !");
    }

}
