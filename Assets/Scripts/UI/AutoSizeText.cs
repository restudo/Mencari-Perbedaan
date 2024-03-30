using TMPro;
using UnityEngine;

public class AutoSizeText : MonoBehaviour
{
    [SerializeField] private float minTextSize;
    [SerializeField] private float maxTextSize;

    private TextMeshProUGUI text;
    private RectTransform rectTransform;
    private float minWidth;
    private float maxWidth;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();

        maxWidth = rectTransform.rect.width;

        Invoke("UpdateTextSize", 0.5f);
    }

    void UpdateTextSize()
    {
        // Get the number of characters in the text
        int charCount = text.text.Length;

        // Calculate the desired text size based on the number of characters
        float desiredSize = Mathf.Clamp(maxTextSize - (charCount * 2), minTextSize, maxTextSize);

        // Set the text size
        text.fontSize = desiredSize;

        // Debug.Log(text.text + " " + desiredSize);
    }
}
