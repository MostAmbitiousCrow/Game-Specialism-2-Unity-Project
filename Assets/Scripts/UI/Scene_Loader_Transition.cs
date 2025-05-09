using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene_Loader_Transition : MonoBehaviour // Made by Samuel White
{
    //========================================
    // This script is used to load a scene with a simple black screen transition effect.
    //========================================

    [SerializeField] private GameObject FadeObject;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeTime = 1f;
    public static Scene_Loader_Transition Instance;
    
    public enum SceneNames
    {
        Main_Menu, Level_1, Level_2, Level_3, Level_4, Level_5, Level_6, Level_7, Level_8, Level_9, Level_10
    }
    public static SceneNames sceneName;

    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Destroy any duplicate scene loaders.
        }
    }

    private void Start()
    {
        FadeObject.SetActive(false);
    }

    // Load Scene Function
    public static void LoadScene(SceneNames sceneName)
    {
        if (SceneManager.sceneCountInBuildSettings < (int)sceneName)
        {
            Debug.LogError("Scene not found in the array of scenes.");
            return;
        }
        Instance.StartCoroutine(Instance.LoadSceneCoroutine(SceneManager.GetSceneByBuildIndex((int)sceneName))); // Start the coroutine to load the scene.
    }

    private IEnumerator LoadSceneCoroutine(Scene scene)
    {
        FadeObject.SetActive(true);
        fadeImage.raycastTarget = true;

        // Fade in
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            float alpha = Mathf.Clamp01(t / fadeTime);
            SetFadeAlpha(alpha);
            yield return null;
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene.buildIndex);
        asyncLoad.allowSceneActivation = false;

        // Load Scene
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

        // Fade out
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            float alpha = Mathf.Clamp01(1 - (t / fadeTime));
            SetFadeAlpha(alpha); // Set the alpha value of the fade object.
            yield return null;
        }
        fadeImage.raycastTarget = false;

        FadeObject.SetActive(false); // Hide the fade object after fading in.
    }

    // Update the image transparency
    private void SetFadeAlpha(float alpha)
    {
        Color color = fadeImage.color; // Get the current color of the fade object.
        color.a = alpha; // Set the alpha value.
    }
}
