// Assets/Scripts/UI/UIManager.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Player UI")]
    public Text healthText;
    public Text staminaText;
    public Text coinsText;
    public Text livesText;
    public Text timeText;
    
    [Header("Item UI")]
    public Button[] itemButtons;
    public Text[] itemCooldownTexts;
    
    [Header("Screens")]
    public GameObject gameScreen;
    public GameObject characterSelectScreen;
    public GameObject gameOverScreen;
    
    void Update()
    {
        UpdatePlayerUI();
        UpdateGameUI();
    }
    
    void UpdatePlayerUI()
    {
        if (GameManager.Instance.localPlayer != null)
        {
            FishPlayerController player = GameManager.Instance.localPlayer;
            healthText.text = $"Health: {player.health}";
            staminaText.text = $"Stamina: {Mathf.RoundToInt(player.stamina)}";
            livesText.text = $"Lives: {player.lives}";
        }
        
        coinsText.text = $"Coins: {GameManager.Instance.playerCoins}";
    }
    
    void UpdateGameUI()
    {
        // Update game timer
        int minutes = Mathf.FloorToInt(GameManager.Instance.currentGameTime / 60);
        int seconds = Mathf.FloorToInt(GameManager.Instance.currentGameTime % 60);
        timeText.text = $"{minutes:00}:{seconds:00}";
    }
    
    public void OnItemButtonClick(int itemIndex)
    {
        // Handle item usage
        // This will need to be connected to your item system
    }
    
    public void ShowGameOver()
    {
        gameScreen.SetActive(false);
        gameOverScreen.SetActive(true);
    }
    
    public void ShowCharacterSelect()
    {
        characterSelectScreen.SetActive(true);
        gameScreen.SetActive(false);
    }
}