using UnityEngine;

public class PersistentCanvas : MonoBehaviour
{
    private static PersistentCanvas instance;

    void Awake()
    {
        // If another canvas exists → destroy this one
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Register this canvas as the instance
        instance = this;

        // Make this Canvas persistent
        DontDestroyOnLoad(gameObject);
    }
}
