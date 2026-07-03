using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformManager : MonoBehaviour
{
    public static PlatformManager Instance;

    public Vector2 MoveInput { get; private set; }

    private PlatformController currentPlatform;

    private void Awake()
    {
        // If an instance already exists destroy this new one
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // Otherwise this is the first instance keep it
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetPlatform(PlatformController platform)
    {
        if (platform == null)
        {
            Debug.Log("Platform removed");
        }
        if (platform != null && platform != currentPlatform)
        {
            Debug.Log("Platfrom changed");
        }
        currentPlatform = platform;
        
       
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();

        currentPlatform?.ReceiveInput(MoveInput);
    }
}