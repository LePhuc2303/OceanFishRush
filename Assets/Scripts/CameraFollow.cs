using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private Transform target; // Fish
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);
    [SerializeField] private float smoothSpeed = 2f;
    
    [Header("Limits")]
    [SerializeField] private float minY = -3f;
    [SerializeField] private float maxY = 3f;
    
    private void Start()
    {
        // Tự động tìm Fish nếu chưa gán
        if (target == null)
        {
            GameObject fish = GameObject.FindWithTag("Player");
            if (fish != null)
                target = fish.transform;
        }
    }
    
    private void LateUpdate()
    {
        if (target == null) return;
        
        // Tính toán vị trí mong muốn
        Vector3 desiredPosition = target.position + offset;
        
        // Giới hạn Y để không camera không đi quá cao/thấp
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
        
        // Smooth follow
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}