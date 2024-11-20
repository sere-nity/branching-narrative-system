using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
using TMPro;

public class BooleanToggleButton : MonoBehaviour
{
    private TMP_Text nameText;
    private Image backgroundImage;
    public Color trueColor = Color.green;
    public Color falseColor = Color.red;

    private string variableName;
    private bool isToggled;

    private void Awake()
    {
        // Get the Image component on this GameObject
        backgroundImage = GetComponent<Image>();
        
        // Get the TMP_Text component from the child object
        nameText = GetComponentInChildren<TMP_Text>();

        if (backgroundImage == null)
            Debug.LogError("BooleanToggleButton: No Image component found on this GameObject.");
        if (nameText == null)
            Debug.LogError("BooleanToggleButton: No TMP_Text component found in children.");
    }

    public void Initialize(string name, bool initialState)
    {
        variableName = name;
        nameText.text = name;
        UpdateState(initialState);
    }

    public void UpdateState(bool newState)
    {
        isToggled = newState;
        backgroundImage.color = isToggled ? trueColor : falseColor;
    }

    public void OnButtonClick()
    {
        isToggled = !isToggled;
        DialogueLua.SetVariable(variableName, isToggled);
        UpdateState(isToggled);
    }
}