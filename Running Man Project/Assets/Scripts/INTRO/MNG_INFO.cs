using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MNG_INFO : SingleTon<MNG_INFO>
{
    [HideInInspector] // 인스펙터에서 숨겨주기 위해 넣음
    public string StageName; // 스테이지를 구분하기 위해 만든 변수
    public bool isBack = false; // 게임에서 메뉴로 돌아간 경우를 구분하기 위해 만든 변수
}
