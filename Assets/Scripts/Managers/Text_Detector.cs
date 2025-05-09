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
        Debug.Log($"Text_Detector: {textComponent} detected.");
        if (GlobalTextData.textComponents.Contains(textComponent)) return;
        
        GlobalTextData.textComponents.Add(textComponent);
        Debug.Log($"Text_Detector: {GetComponent<TextMeshProUGUI>()} added to GlobalTextData list.");
        Destroy(this);
    }
}
