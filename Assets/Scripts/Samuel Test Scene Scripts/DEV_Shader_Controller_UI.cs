using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DEV_Shader_UI_Controller : MonoBehaviour // By Samuel White
{
    [SerializeField] Material shaderMaterial;

    [SerializeField] Slider pixelSlider;
    [SerializeField] Slider posterizeSlider;

    [SerializeField] Text pixelText;
    [SerializeField] Text posterizeText;

    [SerializeField] Toggle pixelToggle;
    [SerializeField] Toggle posterizeToggle;

    private bool b;
    private bool c;

    // void Start()
    // {
    //     float p = pixelSlider.value = shaderMaterial.GetFloat("_Pixel_Size");
    //     pixelText.text = p.ToString();
    //     p = posterizeSlider.value = shaderMaterial.GetFloat("_Posterize_Value");
    //     posterizeText.text = p.ToString();

    //     if (shaderMaterial.GetFloat("_Pixelate") == 0)
    //         b = false;
    //     else b = true;
    //     pixelToggle.isOn = b;

    //     if (shaderMaterial.GetFloat("_Posterize") == 0)
    //         c = false;
    //     else c = true;
    //     posterizeToggle.isOn = c;
    // }

    public void UpdatePixelation()
    {
        float v = (int)pixelSlider.value;
        shaderMaterial.SetFloat("_Pixel_Size", v);
        pixelText.text = v.ToString();
    } 

    public void UpdatePosterize()
    {
        float v = (int)posterizeSlider.value;
        shaderMaterial.SetFloat("_Posterize_Value", v);
        posterizeText.text = v.ToString();
    } 

    public void TogglePixelation()
    {
        b = !b;
        shaderMaterial.SetFloat("_Pixelate", b ? 1 : 0);
        pixelToggle.isOn = b;
    }

    public void TogglePosterization()
    {
        c = !c;
        shaderMaterial.SetFloat("_Posterize", c ? 1 : 0);
        posterizeToggle.isOn = c;
    }
}
