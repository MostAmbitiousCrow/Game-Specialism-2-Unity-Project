using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInTransition : MonoBehaviour // Made by Khayne Lutchmun
{   
    [SerializeField] private GameObject FadeObject; // The button to play the game.
    [SerializeField] private Image Fade ; // The image that will fade out the screen.

    private void Start()
    {
        Fade.color = new Color(0, 0, 0, 1); // Set initial color black
        FadeObject.SetActive(true); // Show the fade object at the start
        StartCoroutine(FadeInCoroutine());
    }

    IEnumerator FadeInCoroutine()
    {
        //fade in the screen
        float fadeTime = 1f; // Time to fade in
        
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            Fade.color = new Color(0, 0, 0, Mathf.Clamp01(1 - (t / fadeTime))); // Fade to clear
            yield return null;
        }
        
        Fade.color = new Color(0, 0, 0, 0); // Ensure it's fully clear
        FadeObject.SetActive(false); // Hide the fade object after fading in
    }
}
