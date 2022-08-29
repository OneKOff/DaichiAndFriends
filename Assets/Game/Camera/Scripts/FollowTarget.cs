using System;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float minSpeed = 5f;    
    [SerializeField] private float maxSpeed = 50f;
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float maxDistance = 10f;

    [Header("Bounds")]
    [SerializeField] private Vector2 leftBottomBound;
    [SerializeField] private Vector2 rightTopBound;

    private Vector2 _movement;

    private void Update()
    {
        var targetPos = (Vector2)target.position;
        var cameraPos = (Vector2)transform.position;
        var movementVector = targetPos - cameraPos;
        
        _movement = movementVector.normalized * 
                    (minSpeed + (maxSpeed - minSpeed) * 
                        Mathf.Clamp((movementVector.magnitude - minDistance) / maxDistance, 0f, 1f));
        
        if (movementVector.sqrMagnitude < minDistance * minDistance)
        {
            _movement = _movement.normalized * minSpeed;
        }
        
        if (targetPos.x >= cameraPos.x + leftBottomBound.x && targetPos.x <= cameraPos.x + rightTopBound.x &&
            targetPos.y >= cameraPos.y + leftBottomBound.y && targetPos.y <= cameraPos.y + rightTopBound.y)
        {
            _movement = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3)_movement * Time.fixedDeltaTime;
    }
    
    private void OnDrawGizmos()
    {
        var position = (Vector2)transform.position;
        
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(position + leftBottomBound, 
            position + new Vector2(leftBottomBound.x, rightTopBound.y));
        Gizmos.DrawLine(position + new Vector2(leftBottomBound.x, rightTopBound.y), 
            position + rightTopBound);
        Gizmos.DrawLine(position + leftBottomBound, 
            position + new Vector2(rightTopBound.x, leftBottomBound.y));
        Gizmos.DrawLine(position + new Vector2(rightTopBound.x, leftBottomBound.y), 
            position + rightTopBound);
    }
}
