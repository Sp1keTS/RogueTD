using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private LayoutElement layoutElement;
    [SerializeField] private int charCountLimit;
    
    private RectTransform rectTransform;
    
    public Vector2 Size => rectTransform ? rectTransform.sizeDelta : Vector2.zero;
    
    public string Header { get => headerText.text; set => headerText.text = value; }
    public string Content { get => contentText.text; set => contentText.text = value; }
    
    private int headerLength;
    private int contentLength;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            rectTransform = gameObject.AddComponent<RectTransform>();
        }
    }

    private void Update()
    {
        headerLength = headerText.textInfo.characterCount;
        contentLength = contentText.textInfo.characterCount;

        layoutElement.enabled = (headerLength > charCountLimit || contentLength > charCountLimit);
    }

    public void SetActive(bool state)
    {
        gameObject.SetActive(state);
    }
}