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
        //¶ƒNƒŠƒbƒN‚Å“Š‚°‚é
        if (Mouse.current.leftButton.wasPressedThisFrame && !GameManager.Instance.isGameOver && !PauseManager.Instance.isPause)
        {
            animator.SetTrigger("isThrow");
        }
    }
}
