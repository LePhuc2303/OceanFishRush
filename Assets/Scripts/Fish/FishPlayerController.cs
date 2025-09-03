using UnityEngine;
using UnityEngine.UI;

namespace OceanFishRush.Fish
{
    public class FishPlayerController : MonoBehaviour
    {
        [Header("Fish Stats")]
        public int maxHealth = 100;
        public float maxStamina = 100f;
        public float swimSpeed = 5f;
        public int lives = 5;
        
        [Header("Movement")]
        public float flapForce = 8f;
        public float gravity = 9.8f;
        public float maxFallSpeed = -10f;
        
        [Header("References")]
        public Image healthBar;
        public Image staminaBar;
        public Text livesText;
        public GameObject shieldEffect;
        public GameObject speedEffect;
        
        [HideInInspector] public int currentHealth;
        [HideInInspector] public float currentStamina;
        [HideInInspector] public bool hasShield = false;
        [HideInInspector] public bool isSpeedBoosted = false;
        
        private Rigidbody2D rb;
        private bool isDead = false;
        private float originalSpeed;
        
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            originalSpeed = swimSpeed;
            
            InitializeFish();
        }
        
        void InitializeFish()
        {
            currentHealth = maxHealth;
            currentStamina = maxStamina;
            UpdateUI();
        }
        
        void Update()
        {
            if (isDead || !GameManager.Instance.gameRunning) return;
            
            HandleInput();
            ApplyGravity();
            ClampVelocity();
            UpdateStamina();
            UpdateUI();
        }
        
        void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                if (currentStamina >= 10f)
                {
                    Flap();
                    currentStamina -= 10f;
                }
            }
        }
        
        void Flap()
        {
            rb.linearVelocity = new Vector2(swimSpeed, flapForce);
        }
        
        void ApplyGravity()
        {
            if (rb.linearVelocity.y > maxFallSpeed)
            {
                rb.linearVelocity -= new Vector2(0, gravity * Time.deltaTime);
            }
        }
        
        void ClampVelocity()
        {
            if (rb.linearVelocity.y < maxFallSpeed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxFallSpeed);
            }
        }
        
        void UpdateStamina()
        {
            if (currentStamina < maxStamina)
            {
                currentStamina += Time.deltaTime * 5f;
            }
        }
        
        void UpdateUI()
        {
            if (healthBar != null)
                healthBar.fillAmount = (float)currentHealth / maxHealth;
            
            if (staminaBar != null)
                staminaBar.fillAmount = currentStamina / maxStamina;
            
            if (livesText != null)
                livesText.text = "Lives: " + lives;
        }
        
        void OnTriggerEnter2D(Collider2D other)
        {
            if (isDead) return;
            
            if (other.CompareTag("Obstacle"))
            {
                TakeDamage(10);
            }
            else if (other.CompareTag("StaminaItem"))
            {
                currentStamina = maxStamina;
                Destroy(other.gameObject);
            }
            else if (other.CompareTag("FinishLine"))
            {
                GameManager.Instance.PlayerFinished(this);
            }
        }
        
        public void TakeDamage(int damage)
        {
            if (hasShield) return;
            
            currentHealth -= damage;
            
            if (currentHealth <= 0)
            {
                Die();
            }
        }
        
        void Die()
        {
            isDead = true;
            lives--;
            
            if (lives <= 0)
            {
                GameManager.Instance.GameOver();
            }
            else
            {
                Invoke("Respawn", 2f);
            }
        }
        
        void Respawn()
        {
            isDead = false;
            currentHealth = maxHealth;
            currentStamina = maxStamina;
            transform.position = GameManager.Instance.GetSpawnPoint();
            rb.linearVelocity = Vector2.zero;
        }
        
        public void ApplyShield(float duration)
        {
            hasShield = true;
            shieldEffect.SetActive(true);
            Invoke("RemoveShield", duration);
        }
        
        void RemoveShield()
        {
            hasShield = false;
            shieldEffect.SetActive(false);
        }
        
        public void ApplySpeedBoost(float multiplier, float duration)
        {
            isSpeedBoosted = true;
            swimSpeed *= multiplier;
            speedEffect.SetActive(true);
            Invoke("RemoveSpeedBoost", duration);
        }
        
        void RemoveSpeedBoost()
        {
            isSpeedBoosted = false;
            swimSpeed = originalSpeed;
            speedEffect.SetActive(false);
        }
    }
}