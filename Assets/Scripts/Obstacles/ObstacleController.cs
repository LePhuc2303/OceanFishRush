using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public int damage = 10;
    public float moveSpeed = 3f;
    
    private void Update()
    {
        // Move obstacle
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        
        // Destroy if out of screen
        if (transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FishPlayerController player = other.GetComponent<FishPlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}