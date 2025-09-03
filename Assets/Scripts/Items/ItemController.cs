using UnityEngine;
using OceanFishRush.Fish;

namespace OceanFishRush.Items
{
    public class ItemController : MonoBehaviour
    {
        public enum ItemType
        {
            Lightning,
            Trap,
            Shield,
            SpeedBoost
        }
        
        public ItemType itemType;
        public int cost = 30;
        public float duration = 5f;
        public float cooldown = 10f;
        
        private bool isOnCooldown = false;
        
        public void UseItem(FishPlayerController player)
        {
            if (isOnCooldown || !GameManager.Instance.UseItem(cost)) return;
            
            switch(itemType)
            {
                case ItemType.Lightning:
                    UseLightning(player);
                    break;
                case ItemType.Trap:
                    UseTrap(player);
                    break;
                case ItemType.Shield:
                    UseShield(player);
                    break;
                case ItemType.SpeedBoost:
                    UseSpeedBoost(player);
                    break;
            }
            
            StartCoroutine(StartCooldown());
        }
        
        void UseLightning(FishPlayerController player)
        {
            Debug.Log("Lightning used!");
        }
        
        void UseTrap(FishPlayerController player)
        {
            Debug.Log("Trap placed!");
        }
        
        void UseShield(FishPlayerController player)
        {
            player.ApplyShield(duration);
        }
        
        void UseSpeedBoost(FishPlayerController player)
        {
            player.ApplySpeedBoost(1.5f, duration);
        }
        
        System.Collections.IEnumerator StartCooldown()
        {
            isOnCooldown = true;
            yield return new WaitForSeconds(cooldown);
            isOnCooldown = false;
        }
    }
}