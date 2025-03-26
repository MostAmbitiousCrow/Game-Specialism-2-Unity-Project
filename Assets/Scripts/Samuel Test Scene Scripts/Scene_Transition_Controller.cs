using System.Collections;
using UnityEngine;

public class Scene_Transition_Controller : MonoBehaviour // By Samuel
{
    [SerializeField] float t;
    private Coroutine c;
    [SerializeField] Material shaderMaterial;
    [SerializeField] int startPixelation = 256;
    [SerializeField] int endPixelation = 2;
    private int pixelation;
    [SerializeField] int startPosterize = 256;
    [SerializeField] int endPosterize = 1;
    private int posterization;

    private void Start()
    {
        if (shaderMaterial == null) return;
        pixelation = shaderMaterial.GetInt("_Pixel_Size");
        posterization = shaderMaterial.GetInt("_Posterize_Value");
    }

    public void StartTransition(float time)
    {
        print("Button Pressed");
        if (c != null) return;
        c = StartCoroutine(ShaderTransition(time));
    }

    IEnumerator ShaderTransition(float time)
    {
        shaderMaterial.SetFloat("_Pixelate", 1);
        shaderMaterial.SetFloat("_Posterize", 1);
        print($"Transition Started {time}");
        while (t < time)
        {
            t += Time.deltaTime;
            shaderMaterial.SetFloat("_Pixel_Size", Mathf.Lerp(endPixelation, startPixelation, t/time));
            shaderMaterial.SetFloat("_Posterize_Value", Mathf.Lerp(startPosterize, endPosterize, t/time));
            yield return null;
        }
        t = 0;

        yield return new WaitForSeconds(.5f);

        while (t < time)
        {
            t += Time.deltaTime;
            shaderMaterial.SetFloat("_Pixel_Size", Mathf.Lerp(startPixelation, endPixelation, t / time));
            shaderMaterial.SetFloat("_Posterize_Value", Mathf.Lerp(endPosterize, startPosterize, t / time));
            yield return null;
        }
        t = 0;
        shaderMaterial.SetFloat("_Pixelate", 0);
        shaderMaterial.SetFloat("_Posterize", 0);
        c = null;
        yield break;
    }
}
