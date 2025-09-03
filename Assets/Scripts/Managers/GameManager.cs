using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using OceanFishRush.Fish;

namespace OceanFishRush.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        [Header("Game Settings")]
        public int maxLives = 5;
        public float gameDuration = 120f;
        public Transform[] spawnPoints;
        public Transform finishLine;
        
        [Header("Player Settings")]
        public GameObject playerPrefab;
        public FishPlayerController currentPlayer;
        
        [Header("UI References")]
        public GameObject gameUI;
        public GameObject menuUI;
        public GameObject gameOverUI;
        public Text timerText;
        public Text coinsText;
        public Button[] itemButtons;
        
        [Header("Item Settings")]
        public int[] itemCosts = { 30, 40, 50, 60 };
        public float[] itemCooldowns = { 10f, 15f, 20f, 25f };
        
        [HideInInspector] public bool gameRunning = false;
        [HideInInspector] public int playerCoins = 1000;
        
        private float currentTime;
        private int selectedCharacter = 0;
        private bool[] itemCooldownActive;
        private float[] itemCooldownTimers;
        
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            
            itemCooldownActive = new bool[4];
            itemCooldownTimers = new float[4];
        }
        
        void Start()
        {
            ShowMainMenu();
        }
        
        void Update()
        {
            if (gameRunning)
            {
                UpdateGameTimer();
                UpdateItemCooldowns();
            }
        }
        
        public void StartGame()
        {
            gameRunning = true;
            currentTime = gameDuration;
            
            SpawnPlayer();
            
            menuUI.SetActive(false);
            gameUI.SetActive(true);
            gameOverUI.SetActive(false);
            
            StartCoroutine(SpawnItemsRoutine());
            StartCoroutine(SpawnObstaclesRoutine());
        }
        
        void SpawnPlayer()
        {
            Vector3 spawnPos = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
            GameObject playerObj = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
            currentPlayer = playerObj.GetComponent<FishPlayerController>();
            
            SetupCharacter(selectedCharacter);
        }
        
        void SetupCharacter(int characterIndex)
        {
            switch(characterIndex)
            {
                case 0:
                    currentPlayer.maxHealth = 100;
                    currentPlayer.maxStamina = 100;
                    currentPlayer.swimSpeed = 5f;
                    break;
                case 1:
                    currentPlayer.maxHealth = 120;
                    currentPlayer.maxStamina = 110;
                    currentPlayer.swimSpeed = 5.5f;
                    break;
                case 2:
                    currentPlayer.maxHealth = 140;
                    currentPlayer.maxStamina = 120;
                    currentPlayer.swimSpeed = 6f;
                    break;
                case 3:
                    currentPlayer.maxHealth = 160;
                    currentPlayer.maxStamina = 130;
                    currentPlayer.swimSpeed = 6.5f;
                    break;
                case 4:
                    currentPlayer.maxHealth = 180;
                    currentPlayer.maxStamina = 140;
                    currentPlayer.swimSpeed = 7f;
                    break;
            }
            
            currentPlayer.InitializeFish();
        }
        
        void UpdateGameTimer()
        {
            currentTime -= Time.deltaTime;
            
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            
            if (currentTime <= 0)
            {
                EndGame();
            }
        }
        
        void UpdateItemCooldowns()
        {
            for (int i = 0; i < 4; i++)
            {
                if (itemCooldownActive[i])
                {
                    itemCooldownTimers[i] -= Time.deltaTime;
                    itemButtons[i].interactable = false;
                    
                    if (itemCooldownTimers[i] <= 0)
                    {
                        itemCooldownActive[i] = false;
                        itemButtons[i].interactable = true;
                    }
                }
            }
        }
        
        System.Collections.IEnumerator SpawnItemsRoutine()
        {
            while (gameRunning)
            {
                yield return new WaitForSeconds(10f);
                SpawnRandomItem();
            }
        }
        
        System.Collections.IEnumerator SpawnObstaclesRoutine()
        {
            while (gameRunning)
            {
                yield return new WaitForSeconds(5f);
                SpawnObstacle();
            }
        }
        
        void SpawnRandomItem()
        {
            // Implementation for spawning random items
        }
        
        void SpawnObstacle()
        {
            // Implementation for spawning obstacles
        }
        
        public void UseItem(int itemIndex, FishPlayerController player)
        {
            if (itemCooldownActive[itemIndex] || playerCoins < itemCosts[itemIndex]) return;
            
            playerCoins -= itemCosts[itemIndex];
            UpdateCoinsUI();
            
            itemCooldownActive[itemIndex] = true;
            itemCooldownTimers[itemIndex] = itemCooldowns[itemIndex];
            
            switch(itemIndex)
            {
                case 0:
                    UseLightningItem(player);
                    break;
                case 1:
                    UseTrapItem(player);
                    break;
                case 2:
                    UseShieldItem(player);
                    break;
                case 3:
                    UseSpeedBoostItem(player);
                    break;
            }
        }
        
        void UseLightningItem(FishPlayerController player)
        {
            Debug.Log("Lightning item used!");
        }
        
        void UseTrapItem(FishPlayerController player)
        {
            Debug.Log("Trap item used!");
        }
        
        void UseShieldItem(FishPlayerController player)
        {
            player.ApplyShield(3f);
        }
        
        void UseSpeedBoostItem(FishPlayerController player)
        {
            player.ApplySpeedBoost(1.5f, 5f);
        }
        
        public void PlayerFinished(FishPlayerController player)
        {
            int reward = 200;
            playerCoins += reward;
            GameOver();
        }
        
        void EndGame()
        {
            gameRunning = false;
            GameOver();
        }
        
        public void GameOver()
        {
            gameRunning = false;
            gameUI.SetActive(false);
            gameOverUI.SetActive(true);
        }
        
        public void ShowMainMenu()
        {
            gameRunning = false;
            menuUI.SetActive(true);
            gameUI.SetActive(false);
            gameOverUI.SetActive(false);
        }
        
        public void SelectCharacter(int characterIndex)
        {
            selectedCharacter = characterIndex;
        }
        
        public bool UseItem(int cost)
        {
            if (playerCoins >= cost)
            {
                playerCoins -= cost;
                UpdateCoinsUI();
                return true;
            }
            return false;
        }
        
        public bool PurchaseCharacter(int characterIndex, int cost)
        {
            if (playerCoins >= cost)
            {
                playerCoins -= cost;
                UpdateCoinsUI();
                return true;
            }
            return false;
        }
        
        void UpdateCoinsUI()
        {
            coinsText.text = "Coins: " + playerCoins;
        }
        
        public Vector3 GetSpawnPoint()
        {
            return spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        }
        
        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}