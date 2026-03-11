using UnityEngine;

/// <summary>
/// リザルトUIのアニメーション管理
/// </summary>
public class ResultAnimation : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Play()
    {
        animator.SetTrigger("Play");
    }
}