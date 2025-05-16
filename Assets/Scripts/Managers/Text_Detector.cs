using TMPro;
using UnityEngine;

public class Text_Detector : MonoBehaviour // By Samuel White
{
    //========================================
    // Detects text components and stores them in the GlobalTextData list.
    //========================================

    private void Start()
    {
        TextMeshProUGUI textComponent = GetComponent<TextMeshProUGUI>();
        // Debug.Log($"Text_Detector: {textComponent} detected.");
        if (GameManager.instance.textComponents.Contains(textComponent)) return;
        
        textComponent.font = Settings_Manager.dyslexiaFont ? GameManager.instance.DyslexFont : GameManager.instance.DefaultFont;
        GameManager.instance.textComponents.Add(textComponent);
        // Debug.Log($"Text_Detector: {GetComponent<TextMeshProUGUI>()} added to GlobalTextData list.");
        Destroy(this);
    }
}
