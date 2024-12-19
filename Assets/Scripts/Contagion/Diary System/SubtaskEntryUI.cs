using UnityEngine;
using TMPro;
public class SubtaskEntryUI : MonoBehaviour
{
    [SerializeField] private TMP_Text subtaskTitle;
    
    public void SetSubtaskTitle(string title)
    {
        subtaskTitle.text = title;
    }
}