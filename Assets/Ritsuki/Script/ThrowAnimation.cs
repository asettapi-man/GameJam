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
        //ç∂ÉNÉäÉbÉNÇ≈ìäÇ∞ÇÈ
        if (Mouse.current.leftButton.wasPressedThisFrame && !GameManager.Instance.isGameOver)
        {
            animator.SetTrigger("isThrow");
        }
    }
}
