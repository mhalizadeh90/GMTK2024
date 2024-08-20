using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryBoardController : MonoBehaviour
{
    [SerializeField] private List<StoryBoard> _storyBoards;
    [SerializeField] private Image background;  // Background image that will fade in/out
    [SerializeField] private Image storyImage;  // Image that will show each storyboard's image
    [SerializeField] private TMPro.TextMeshProUGUI storyText;    // Text element for showing the storyboard text
    [SerializeField] private float delayBetweenEachStoryBoard = 2f;  // Delay between storyboard items
    [SerializeField] private float fadeDuration = 1f;  // Duration of the fade effect

    private void Start()
    {
        StartCoroutine(ShowStoryboard());
    }

    IEnumerator ShowStoryboard()
    {
        // Initially make the background fully visible and the story elements invisible
        background.color = Color.black;
        storyImage.color = new Color(1, 1, 1, 0);
        storyText.color = new Color(1, 1, 1, 0);

        // Go through each storyboard in the list
        foreach (var storyboard in _storyBoards)
        {
            // Set the image and text for the current storyboard
            storyImage.sprite = storyboard.image;
            storyText.text = storyboard.text;

            // Fade in the image and text
            yield return StartCoroutine(FadeIn(storyImage));
            yield return StartCoroutine(FadeIn(storyText));

            // Wait for the specified delay
            yield return new WaitForSeconds(delayBetweenEachStoryBoard);

            // Fade out the image and text
            yield return StartCoroutine(FadeOut(storyImage));
            yield return StartCoroutine(FadeOut(storyText));
        }

        // Once all storyboards are shown, fade the background to black
        yield return StartCoroutine(FadeOut(background));

        // Load the next scene (index 1)
        SceneManager.LoadScene(1);
    }

    IEnumerator FadeIn(Graphic uiElement)
    {
        Color originalColor = uiElement.color;
        for (float t = 0.01f; t < fadeDuration; t += Time.deltaTime)
        {
            uiElement.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(0, 1, t / fadeDuration));
            yield return null;
        }
        uiElement.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
    }

    IEnumerator FadeOut(Graphic uiElement)
    {
        Color originalColor = uiElement.color;
        for (float t = 0.01f; t < fadeDuration; t += Time.deltaTime)
        {
            uiElement.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1, 0, t / fadeDuration));
            yield return null;
        }
        uiElement.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
    }
}

[Serializable]
public struct StoryBoard
{
    public Sprite image;
    public string text;
}
