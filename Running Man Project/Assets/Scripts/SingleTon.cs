using UnityEngine;
using System.Collections;

public class SingleTon<T> : MonoBehaviour where T : MonoBehaviour // 싱글 턴 함수
{
    protected static T instance = null;
    
    public static T H
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(T)) as T;

                if (instance == null)
                {
                    Debug.Log("Null : " + instance.ToString());
                    return null;
                }
            }

            return instance;
        }
    }
}
