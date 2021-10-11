using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MNG_OBJECT : SingleTon<MNG_OBJECT>
{
    [HideInInspector] // 인스펙터 상에서 숨겨주기 위해 넣음
    public bool isPause; // 일시정지 구분을 위해 만든 변수
    public float Score; // 점수를 넣어주는 변수
    public float Speed; // 속도를 넣어주는 변수
    public GameObject HpBar; // 체력을 표시해주는 오브젝트를 넣어줄 변수
    public GameObject BG; // 배경화면 오브젝트를 넣어줄 변수
    public GameObject Obj; // 방해 오브젝트를 넣어줄 부모 오브젝트 변수
    public GameObject PausePopup, GameOverPopup; // 팝업창 오브젝트를 넣어줄 변수
    public GameObject Character; // 캐릭터를 넣어줄 변수
    public UILabel ScoreLabel, GameOverScore; // 점수, 게임오버시 표시될 점수 텍스트 변수
}