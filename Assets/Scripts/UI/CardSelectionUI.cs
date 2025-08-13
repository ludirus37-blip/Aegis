using UnityEngine;
using System.Collections.Generic;
// In a real project, you would import TextMeshPro and UI namespaces.
// using TMPro;
// using UnityEngine.UI;

public class CardSelectionUI : MonoBehaviour
{
    [Tooltip("Assign the UI panel GameObjects that represent the card choices.")]
    public GameObject[] cardOptionSlots; // Should have 3 slots assigned in the Unity Editor.

    // This method is called by the GameManager when the UI should appear.
    public void DisplayCardOptions()
    {
        List<CardData> options = CardManager.Instance.GetRandomCardOptions(3);

        if (options.Count == 0)
        {
            Debug.LogWarning("No card options were available to display.");
            GameManager.Instance.ResumeGameAfterSelection();
            return;
        }

        for (int i = 0; i < cardOptionSlots.Length; i++)
        {
            if (i < options.Count)
            {
                Debug.Log($"Presenting option {i + 1}: {options[i].cardName}");

                // In a real Unity project, you would get the Button component on the slot
                // and add a listener to its onClick event. This would be configured in the editor.
                // e.g., cardOptionSlots[i].GetComponent<Button>().onClick.AddListener(() => OnCardSelected(options[i]));
                // The CardButtonSimulator class below is a stand-in for that editor-side logic.
                var cardButton = cardOptionSlots[i].AddComponent<CardButtonSimulator>();
                cardButton.Initialize(options[i], this);

                cardOptionSlots[i].SetActive(true);
            }
            else
            {
                cardOptionSlots[i].SetActive(false);
            }
        }
    }

    // This method would be called by the UI button's OnClick event.
    public void OnCardSelected(CardData selectedCard)
    {
        if (selectedCard != null)
        {
            Debug.Log($"Card '{selectedCard.cardName}' was selected via UI.");
            CardManager.Instance.ApplyCard(selectedCard);
            GameManager.Instance.ResumeGameAfterSelection();
        }
    }
}

/// ================================================================================================
/// NOTE: This is a helper class to SIMULATE UI button behavior in a code-only environment.
/// In a real Unity project, this class would NOT exist. Instead, you would use the
/// built-in 'UnityEngine.UI.Button' component. You would drag the 'CardSelectionUI' GameObject
/// into the 'OnClick' event field in the Unity Editor and select the 'OnCardSelected' method.
/// ================================================================================================
public class CardButtonSimulator : MonoBehaviour
{
    private CardData assignedCard;
    private CardSelectionUI uiController;

    public void Initialize(CardData card, CardSelectionUI controller)
    {
        assignedCard = card;
        uiController = controller;
    }

    // This method simulates the button click. In a real scenario, this would be invoked by the UI system.
    public void SimulateClick()
    {
        uiController.OnCardSelected(assignedCard);
    }
}
