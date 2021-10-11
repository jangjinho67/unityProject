using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MNG_INTRO : SingleTon<MNG_INTRO>
{
    public GameObject BG1, BG2; // 배경화면 오브젝트 변수

    void Start ()
    {
        StartCoroutine(ChagneAlpha()); // 알파값을 변경해주는 코루틴 시작
    }

    public IEnumerator ChagneAlpha()
    {
        yield return new WaitForSeconds(1.5f); // 1.5초 동안 이 코루틴을 멈춰준다

        TweenAlpha.Begin(BG1, 1.5f, 0); // 배경 1 (인하공전 로고)의 알파값을 1.5초동안 서서히 줄여 0으로 바꿔준다

        yield return new WaitForSeconds(1.75f); // 1.75초 동안 이 코루틴을 멈춰준다

        BG2.SetActive(true); // 배경2의 active를 true로 변경

        yield return new WaitForSeconds(1.5f); // 1.5초 동안 이 코루틴을 멈춰준다

        // 배경 2 (팀 로고)의 알파값을 1.5초동안 서서히 줄여 0으로 바꿔준다
        TweenAlpha.Begin(BG2.transform.GetChild(0).gameObject, 1.5f, 0);
        TweenAlpha.Begin(BG2.transform.GetChild(1).gameObject, 1.5f, 0);

        yield return new WaitForSeconds(1.75f); // 1.75초 동안 이 코루틴을 멈춰준다

        SceneManager.LoadScene("1_MENU"); // 메뉴화면으로 넘어간다
    }
}
