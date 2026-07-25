using UnityEngine;

public class PersistentObjects : MonoBehaviour
{
    public static PersistentObjects Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }
}