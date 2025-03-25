using UnityEngine;
using UnityEngine.UI;

public class BackgroundAnimator : MonoBehaviour
{
    public Sprite frame1; // First background frame
    public Sprite frame2; // Second background frame
    public float frameRate = 0.5f; // Time between frames

    private Image image;
    private bool showingFrame1 = true;

    void Start()
    {
        image = GetComponent<Image>(); // Get Image component
        InvokeRepeating(nameof(SwapFrame), 0, frameRate); // Repeat frame swap
    }

    void SwapFrame()
    {
        image.sprite = showingFrame1 ? frame2 : frame1; // Toggle between frames
        showingFrame1 = !showingFrame1; // Flip state
    }
}