using UnityEngine;
using UnityEngine.SceneManagement;
public class Singleton : MonoBehaviour
{
    static Singleton singleton; //used to keep sure this gameobject is not deleted

    void Awake()
    {
        SingletonForGameObject();
    }

    void SingletonForGameObject() //This makes sure this gameobject with this script is the only one that can exist.
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
