using UnityEngine;
using System.Collections.Generic;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }

    [Tooltip("A list of all possible card data assets that can be drawn in the game.")]
    public List<CardData> masterCardList;

    private PlayerStats playerStats;
    // This list will hold the cards the player has chosen during the run.
    private List<CardData> playerDeck;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        playerDeck = new List<CardData>();
    }

    void Start()
    {
        // Find the PlayerStats component in the scene.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStats>();
        }

        if (playerStats == null)
        {
            Debug.LogError("CardManager could not find the PlayerStats component on the Player.", this);
        }
    }

    /// <summary>
    /// Applies the effects of a chosen card to the player.
    /// </summary>
    /// <param name="cardToApply">The ScriptableObject of the card to apply.</param>
    public void ApplyCard(CardData cardToApply)
    {
        if (playerStats == null) return;

        Debug.Log($"Applying card: {cardToApply.cardName}");
        playerDeck.Add(cardToApply);

        // Iterate through all defined effects on the card and apply them.
        foreach (var effect in cardToApply.effectValues)
        {
            playerStats.ModifyStat(effect.key, effect.value);
        }
    }

    /// <summary>
    /// Gets a specified number of random, unique card options from the master list.
    /// </summary>
    /// <param name="count">The number of card options to return.</param>
    /// <returns>A list of random CardData objects.</returns>
    public List<CardData> GetRandomCardOptions(int count)
    {
        List<CardData> chosenCards = new List<CardData>();
        // Create a temporary pool to draw from, preventing modification of the master list.
        List<CardData> drawPool = new List<CardData>(masterCardList);

        for (int i = 0; i < count; i++)
        {
            if (drawPool.Count == 0) break;

            int randomIndex = Random.Range(0, drawPool.Count);
            chosenCards.Add(drawPool[randomIndex]);
            // Remove the chosen card to prevent it from being picked again in the same draw.
            drawPool.RemoveAt(randomIndex);
        }

        return chosenCards;
    }
}
