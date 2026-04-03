using UnityEngine;
using UnityEngine.InputSystem;

public class ThrowAnimation : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //左クリックまたは画面タッチで投げる
        if (Mouse.current.leftButton.wasPressedThisFrame || Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            if (!GameManager.Instance.isGameOver && !PauseManager.Instance.isPause)
            {
                animator.SetTrigger("isThrow");
            }
        }
    }
}
