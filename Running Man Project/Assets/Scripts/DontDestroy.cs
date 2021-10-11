using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this); // 이 스크립트가 포함된 오브젝트는 Scene이 바뀌어도 사라지지 않게 설정
    }
}