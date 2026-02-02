using MiniJam159.GameCore;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Inspector members

    public float maxMoveSpeed;
    public float accelerationSpeed;
    public float dragFactor;

    #endregion

    public Vector3 velocity = new Vector3();

    public static PlayerController instance;
    private void Awake()
    {
        // Singleton
        if (instance == null) instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        // IDK where else to put this
        // Add keybinds
        InputManager.instance.addKeyToMap("right", KeyCode.D);
        InputManager.instance.addKeyToMap("left", KeyCode.A);
        InputManager.instance.addKeyToMap("up", KeyCode.W);
        InputManager.instance.addKeyToMap("down", KeyCode.S);
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // Handle input
        Vector3 movement = new Vector3();
        if (InputManager.instance.getKey("right")) movement.x += 1;
        if (InputManager.instance.getKey("left")) movement.x += -1;
        if (InputManager.instance.getKey("up")) movement.y += 1;
        if (InputManager.instance.getKey("down")) movement.y += -1;

        // Accelerate
        if (movement.magnitude > 0)
        {
            movement = Vector3.Normalize(movement);
            velocity += movement * accelerationSpeed * Time.fixedDeltaTime;
        }

        // Decelerate if no input or opposite on an axis
        if (movement.x == 0 || movement.x * velocity.x < 0) velocity.x *= dragFactor;
        if (movement.y == 0 || movement.y * velocity.y < 0) velocity.y *= dragFactor;

        // Clamp to max speed
        if (velocity.magnitude > maxMoveSpeed) velocity *= (maxMoveSpeed / velocity.magnitude);

        // Move player
        transform.position += velocity * Time.fixedDeltaTime;
    }
}
