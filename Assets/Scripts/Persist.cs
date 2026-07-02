using UnityEngine;

public class Persist : MonoBehaviour
{
    private static Persist instance;

    private void Awake()
    {
        // If an instance already exists destroy this new one
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // Otherwise this is the first instance keep it
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
}
