using System;
using CoreGame;
using Game;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  
    public GameObject player; // The player object
    public float speed = 5f; // Movement speed
    public float stopDistance = 150f; // Stop moving when within this range of the mouse

    [SerializeField] private Vector3 _topLeftPosition;
    [SerializeField] private Vector3 _topRightPosition;
    [SerializeField] private Vector3 _bottomLeftPosition;
    [SerializeField] private Vector3 _bottomRightPosition;
    private void Start()
    {
        var landCtrl = Locator<LandController>.Instance;
        _topLeftPosition = landCtrl.TopLeftPosition;
        _topRightPosition = landCtrl.TopRightPosition;
        _bottomLeftPosition = landCtrl.BottomLeftPosition;
        _bottomRightPosition = landCtrl.BottomRightPosition;
    }

     private void Update()
    {
        if (Input.GetMouseButton(0)) // Check if the left mouse button is held
        {
            // Get the mouse position
            Vector3 mousePos = Input.mousePosition;

            // Calculate direction towards the mouse
            Vector3 direction = (mousePos - transform.position).normalized;

            // Calculate distance between player and mouse
            float distanceToMouse = Vector3.Distance(transform.position, mousePos);

            // If the object is within stopDistance, stop moving
            if (distanceToMouse <= stopDistance)
            {
                return; // Do nothing (stop movement)
            }

            // If direction is valid, rotate towards the mouse
            if (direction != Vector3.zero)
            {
                float angle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }

            // Create movement vector
            Vector3 moveDirection = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) // Move forward towards the mouse
                moveDirection += direction;

            if (Input.GetKey(KeyCode.S)) // Move backward away from the mouse
                moveDirection -= direction;

            if (Input.GetKey(KeyCode.A)) // Move left (perpendicular to the mouse direction)
                moveDirection += new Vector3(-direction.y, direction.x, 0);

            if (Input.GetKey(KeyCode.D)) // Move right (perpendicular to the mouse direction)
                moveDirection += new Vector3(direction.y, -direction.x, 0);

            // Calculate new position
            Vector3 newPosition = transform.position + moveDirection.normalized * speed;

            // **Restrict movement within defined boundaries**
            newPosition.x = Mathf.Clamp(newPosition.x, _bottomLeftPosition.x, _bottomRightPosition.x); // Clamp X within left & right
            newPosition.y = Mathf.Clamp(newPosition.y, _bottomLeftPosition.y, _topLeftPosition.y); // Clamp Y within bottom & top

            // Apply the clamped position
            transform.position = newPosition;
        }
    }
}