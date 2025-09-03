using UnityEngine;
using UnityEngine.UI;
using OceanFishRush.Fish;
using OceanFishRush.Managers; // THÊM NÀY

namespace OceanFishRush.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Main Menu UI")]
        public GameObject characterSelectPanel;
        public Button[] characterButtons;
        public Text[] characterPrices;
        
        [Header("Game UI")]
        public Slider healthSlider;
        public Slider staminaSlider;
        public Text livesText;
        public Text coinsText;
        public Text timerText;
        
        [Header("Item UI")]
        public Button[] itemButtons;
        public Image[] itemCooldownOverlays;
        
        private GameManager gameManager;
        private FishPlayerController player;
        
        void Start()
        {
            // Tìm GameManager trong scene
            gameManager = FindObjectOfType<GameManager>();
            
            // Hoặc sử dụng singleton pattern nếu đã implement
            // gameManager = GameManager.Instance;
            
            if (gameManager == null)
            {
                Debug.LogError("GameManager not found in scene!");
            }
            else
            {
                UpdateCharacterUI();
            }
        }
        
        void Update()
        {
            if (gameManager != null && gameManager.gameRunning && player != null)
            {
                UpdateGameUI();
            }
        }
        
        void UpdateGameUI()
        {
            if (player != null)
            {
                healthSlider.value = (float)player.currentHealth / player.maxHealth;
                staminaSlider.value = player.currentStamina / player.maxStamina;
                livesText.text = "Lives: " + player.lives;
            }
            
            if (gameManager != null)
            {
                coinsText.text = "Coins: " + gameManager.playerCoins;
            }
        }
        
        void UpdateCharacterUI()
        {
            if (characterPrices == null || characterButtons == null) return;
            
            for (int i = 0; i < characterPrices.Length; i++)
            {
                if (i < characterButtons.Length)
                {
                    int cost = (i == 0) ? 0 : (i * 30 + 10);
                    characterPrices[i].text = cost == 0 ? "FREE" : cost.ToString();
                    
                    // Check if character is purchased
                    characterButtons[i].interactable = cost == 0 || PlayerPrefs.GetInt("Character_" + i, 0) == 1;
                }
            }
        }
        
        public void OnCharacterSelected(int index)
        {
            if (gameManager == null) return;
            
            int cost = (index == 0) ? 0 : (index * 30 + 10);
            
            if (cost == 0 || PlayerPrefs.GetInt("Character_" + index, 0) == 1)
            {
                gameManager.SelectCharacter(index);
            }
            else if (gameManager.playerCoins >= cost)
            {
                if (gameManager.PurchaseCharacter(index, cost))
                {
                    PlayerPrefs.SetInt("Character_" + index, 1);
                    UpdateCharacterUI();
                }
            }
        }
        
        public void OnItemButtonClick(int index)
        {
            if (player != null)
            {
                player.UseItem(index);
            }
        }
        
        public void SetPlayerReference(FishPlayerController playerRef)
        {
            player = playerRef;
        }
        
        public void UpdateItemCooldown(int index, float cooldownTime, float maxCooldown)
        {
            if (itemCooldownOverlays != null && index < itemCooldownOverlays.Length)
            {
                itemCooldownOverlays[index].fillAmount = cooldownTime / maxCooldown;
            }
        }
        
        public void OnStartButtonClick()
        {
            if (gameManager != null)
            {
                gameManager.StartGame();
            }
        }
        
        public void OnRestartButtonClick()
        {
            if (gameManager != null)
            {
                gameManager.RestartGame();
            }
        }
        
        public void OnQuitButtonClick()
        {
            if (gameManager != null)
            {
                gameManager.QuitGame();
            }
        }
        
        public void OnMainMenuButtonClick()
        {
            if (gameManager != null)
            {
                gameManager.ShowMainMenu();
            }
        }
    }
}