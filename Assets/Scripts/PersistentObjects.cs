using UnityEngine;

public class PersistentObjects : MonoBehaviour
{
    public static PersistentObjects Instance;

    public GameObject[] objectsToKeep;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (GameObject obj in objectsToKeep)
        {
            if (obj != null)
            {
                DontDestroyOnLoad(obj);
            }
        }
    }
}