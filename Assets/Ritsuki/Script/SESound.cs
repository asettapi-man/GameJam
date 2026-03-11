using UnityEngine;
using UnityEngine.UI;

public class SESound : MonoBehaviour
{
    [Header("SEİ’è")]
    public AudioSource audioSource;
    public AudioClip buttonSE;
    public Button[] buttons; // •¡”‚Ìƒ{ƒ^ƒ“‚ğ“o˜^

    void Start()
    {
        // ‘Sƒ{ƒ^ƒ“‚ÉSE‚ğ“o˜^
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(PlaySE);
        }
    }

    void PlaySE()
    {
        if (buttonSE == null)
        {
            Debug.LogWarning("SE‚ªİ’è‚³‚ê‚Ä‚¢‚Ü‚¹‚ñ");
            return;
        }

        audioSource.PlayOneShot(buttonSE);
    }
}
