using UnityEngine;

public class Singleton : MonoBehaviour
{
    private static Singleton singleton;

    private void SingletonForGameObject()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        SingletonForGameObject();
    }
}