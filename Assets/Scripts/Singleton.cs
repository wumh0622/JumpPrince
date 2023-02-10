using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public virtual bool ShouldDestoryOnLoad()
    {
        return false;
    }
    public static T instance
    {
        get
        {
            if (instancePrivate == null)
            {
                T[] managers = Object.FindObjectsOfType(typeof(T)) as T[];
                if (managers.Length != 0)
                {
                    if (managers.Length == 1)
                    {
                        instancePrivate = managers[0];
                        //instancePrivate.gameObject.name = typeof(T).Name;
                        return instancePrivate;
                    }
                    else
                    {
                        Debug.LogError("Class " + typeof(T).Name + " exists multiple times in violation of singleton pattern. Destroying all copies");
                        foreach (T manager in managers)
                        {
                            Destroy(manager.gameObject);
                        }
                    }
                }
                var go = new GameObject(typeof(T).Name, typeof(T));
                instancePrivate = go.GetComponent<T>();
                if(!instancePrivate.ShouldDestoryOnLoad())
                {
                    DontDestroyOnLoad(go);
                }

            }
            return instancePrivate;
        }
        set
        {
            instancePrivate = value as T;
        }
    }
    private static T instancePrivate;
}
