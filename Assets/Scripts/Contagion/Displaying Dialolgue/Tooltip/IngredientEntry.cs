using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Contagion
{
    public class IngredientEntry : MonoBehaviour
    {
        [SerializeField] private Image checkmarkIcon;
        [SerializeField] private Image ingredientIcon;
        [SerializeField] private TMP_Text ingredientNameText;

        public void SetIngredient(string ingredientName, bool hasIngredient)
        {
            ingredientNameText.text = ingredientName;
            checkmarkIcon.enabled = hasIngredient;
            ingredientNameText.color = hasIngredient ? Color.black : Color.gray;
            gameObject.SetActive(true);
        }
        
    }
}