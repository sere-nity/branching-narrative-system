using UnityEngine;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

[System.Serializable] 
public class BooleanDebugManager : MonoBehaviour
{
    public GameObject booleanButtonPrefab;
    public Transform contentTransform;

    private Dictionary<string, BooleanToggleButton> buttons = new Dictionary<string, BooleanToggleButton>();

    void Start()
    {
        PopulateButtons();
    }

    void PopulateButtons()
    {
        var variables = DialogueManager.masterDatabase.variables;
        foreach (var variable in variables)
        {
            if (variable.Type == FieldType.Boolean)
            {
                CreateButton(variable);
            }
        }
    }

    void CreateButton(Variable variable)
    {
        GameObject buttonObj = Instantiate(booleanButtonPrefab, contentTransform);
        BooleanToggleButton button = buttonObj.GetComponent<BooleanToggleButton>();
        button.Initialize(variable.Name, GetVariableValue(variable.Name));
        buttons[variable.Name] = button;
    }

    bool GetVariableValue(string variableName)
    {
        return DialogueLua.GetVariable(variableName).AsBool;
    }

    public void UpdateButtonState(string variableName, bool newValue)
    {
        if (buttons.TryGetValue(variableName, out BooleanToggleButton button))
        {
            button.UpdateState(newValue);
        }
    }
}