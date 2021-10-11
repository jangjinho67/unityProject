using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MNG_GAME : SingleTon<MNG_GAME>
{
    private IEnumerator ObjectCoroutine;
    
    void Start ()
    {
        StartSettings(); // 초기화 설정 함수호출
    }
	
    void StartSettings() // 초기화 함수
    {
        ObjectCoroutine = CreateObject();
        StartCoroutine(ObjectCoroutine); // 방해물 오브젝트 생성 코루틴 시작
        MNG_OBJECT.H.isPause = false; // 일시정지 구분을 위한 변수 기본값인 false로 초기화

        MNG_OBJECT.H.BG.GetComponent<UISprite>().spriteName = MNG_INFO.H.StageName; // 스테이지 이름으로 스테이지를 구분해서 배경화면을 바꿔준다
        MNG_OBJECT.H.BG.transform.GetChild(0).GetComponent<UISprite>().spriteName = MNG_INFO.H.StageName; // 2번째 이어지는 배경도 위와 똑같이 설정
        MNG_OBJECT.H.HpBar.transform.GetChild(0).GetComponent<UISprite>().fillAmount = 1f; // 체력바를 최대치로 설정
        MNG_OBJECT.H.Speed = 150; // 기본 스피드 설정
        MNG_OBJECT.H.Score = 0; // 점수 초기화
        MNG_OBJECT.H.ScoreLabel.text = "0"; // 점수 라벨 초기화

        MNG_SOUND.H.MusicPause(); // 현재 재생중인 음악을 멈춰주고
        MNG_SOUND.H.MusicSet("GAME"); // 게임 음악을 틀어준다
        MNG_SOUND.H.MusicSetLoop(true); // 반복재생 설정

        if (MNG_SOUND.H.MusicIsPlay()) // 음악이 이미 재생중인경우 끝
            return;
        else
            MNG_SOUND.H.MusicPlay(); // 아닌경우 재생
    }

    void Update () // 업데이트 함수 (매 프레임마다 갱신된다)
    {
        if (!MNG_OBJECT.H.isPause) // 일시정지중이 아니면 들어오게 설정
        {
            if (MNG_OBJECT.H.BG.transform.localPosition.x > -1600) // 캐릭터가 움직이는 것 처럼 보이게 하기 위해 배경화면을 2개 넣어 배경화면을 움직여서 반복시킴
                MNG_OBJECT.H.BG.transform.localPosition = new Vector3(MNG_OBJECT.H.BG.transform.localPosition.x - (Time.deltaTime * (700 + MNG_OBJECT.H.Speed)), 0, 0); //Time.deltaTime * 750 이 캐릭터의 속도를 정의했습니다.
            else
                MNG_OBJECT.H.BG.transform.localPosition = Vector3.zero; // 배경화면 반복을 위해 위치 초기화

            if (MNG_OBJECT.H.HpBar.transform.GetChild(0).GetComponent<UISprite>().fillAmount <= 0) // 체력이 0인 경우 게임오버 함수를 호출해준다
                GameOver();

            MNG_OBJECT.H.Score += Time.deltaTime * 20; // 시간이 흐름에 따라 점수 증가
            MNG_OBJECT.H.ScoreLabel.text = ((int)MNG_OBJECT.H.Score).ToString(); // 점수를 오른쪽 상단에 있는 점수 텍스트에 넣어준다

            if (MNG_OBJECT.H.Speed <= 200) // 방해 오브젝트 속도의 최대치보다 현재 오브젝트 속도가 작을 경우 오브젝트 속도를 서서히 늘려준다
                MNG_OBJECT.H.Speed += Time.deltaTime * 2; // 방해 오브젝트 속도 증가
        }
    }

    void GameOver() // 게임오버 함수
    {
        MNG_OBJECT.H.isPause = true; // 일시정지를 시켜주고
        MNG_OBJECT.H.GameOverPopup.SetActive(true); // 게임오버 팝업창을 띄어준다
        MNG_OBJECT.H.Character.GetComponent<UISpriteAnimation>().framesPerSecond = 0; // 캐릭터의 애니메이션 재생을 멈춰주고
        MNG_SOUND.H.MusicPause(); // 배경음악 정지
        MNG_OBJECT.H.GameOverScore.text = MNG_OBJECT.H.ScoreLabel.text; // 게임오버 팝업에 있는 점수 텍스트에 현재 점수 텍스트를 넣어준다
        StopCoroutine(ObjectCoroutine); // 오브젝트 생성 코루틴을 정지시킨다
        ResetObject(); // 방해 오브젝트 초기화 함수호출
        SendScore(); // 서버로 점수를 보내주는 함수호출
    }

    void SendScore() // 서버로 점수를 보내는 함수
    {
        // 이건 너가해라
    }

    public void Pause() // 일시정지 함수
    {
        MNG_OBJECT.H.isPause = true; // 일시정지 변수 true로 설정
        MNG_OBJECT.H.PausePopup.SetActive(true); // 일시정지 팝업 띄어주기
        MNG_OBJECT.H.Character.GetComponent<UISpriteAnimation>().framesPerSecond = 0; // 캐릭터의 애니메이션 재생속도 0으로 바꾸어 멈추게 하기
        MNG_SOUND.H.MusicPause(); // 음악 일시정지
    }

    public void Resume() // 일시정지 해제 함수
    {
        MNG_OBJECT.H.isPause = false; // 일시정지 변수 false로 설정
        MNG_OBJECT.H.PausePopup.SetActive(false); // 일시정지 팝업 꺼주기
        MNG_OBJECT.H.Character.GetComponent<UISpriteAnimation>().framesPerSecond = 15; // 캐릭터의 애니메이션 재생속도 초기화
        MNG_SOUND.H.MusicPlay(); // 음악 다시 재생
    }

    public void BackHome() // 메인메뉴로 돌아가기 위한 함수
    {
        MNG_INFO.H.isBack = true; // 게임에서 메뉴로 돌아가는 것을 확인하기 위한 변수 true로 설정
        SceneManager.LoadScene("1_MENU"); // 메뉴로 돌아가기
    }

    public void Replay() // 게임다시하기 함수
    {
        MNG_OBJECT.H.GameOverPopup.SetActive(false); // 게임오버 팝업을 꺼주기
        MNG_OBJECT.H.Character.GetComponent<UISpriteAnimation>().framesPerSecond = 15; // 캐릭터의 애니메이션 재생속도 초기화
        StartSettings(); // 게임설정 초기화
    }

    void ResetObject() // 방해 오브젝트 초기화 함수
    {
        for(int i = 0; i < MNG_OBJECT.H.Obj.transform.childCount; i++)
        {
            for(int j = 0; j < MNG_OBJECT.H.Obj.transform.GetChild(i).childCount; j++)
            {
                if(MNG_OBJECT.H.Obj.transform.GetChild(i).GetChild(j).CompareTag("OBJECT_FLOOR"))
                    MNG_OBJECT.H.Obj.transform.GetChild(i).GetChild(j).localPosition = new Vector3(950, 0, 0); // 오브젝트 좌표 초기화
                else if(MNG_OBJECT.H.Obj.transform.GetChild(i).GetChild(j).CompareTag("OBJECT_SKY"))
                    MNG_OBJECT.H.Obj.transform.GetChild(i).GetChild(j).localPosition = new Vector3(950, 125, 0); // 오브젝트 좌표 초기화

                MNG_OBJECT.H.Obj.transform.GetChild(i).GetChild(j).gameObject.SetActive(false); // 오브젝트 active를 false로 초기화
            }
        }
    }

    IEnumerator CreateObject() // 방해 오브젝트 생성 코루틴
    {
        ResetObject(); // 오브젝트 초기화 함수 호출

        while (true) // 무한 반복문
        {
            yield return new WaitForSeconds(Random.Range(1.2f, 2.0f)); // 오브젝트 생성을 1.2초 ~ 2초 사이의 랜덤으로 생성해주기 위해 저 시간만큼 코루틴이 돌지 않게 설정

            if(!MNG_OBJECT.H.isPause) // 일시정지중이 아닌경우
            {
                GameObject Obj = MNG_OBJECT.H.Obj.transform.GetChild(Random.Range(0, MNG_OBJECT.H.Obj.transform.childCount)).gameObject; // 방해 오브젝트중 랜덤선택

                for (int i = 0; i < Obj.transform.childCount; i++)
                {
                    if (!Obj.transform.GetChild(i).gameObject.activeInHierarchy) // 선택된 오브젝트의 자식중에서 활성화가 안되있는걸 찾아서 활성화 시켜준다
                    {
                        Obj.transform.GetChild(i).gameObject.SetActive(true);
                        break;
                    }
                    else if (i == Obj.transform.childCount - 1)
                    {
                        Obj = MNG_OBJECT.H.Obj.transform.GetChild(Random.Range(0, MNG_OBJECT.H.Obj.transform.childCount)).gameObject;
                        i = 0;
                    }
                }
            }
        }
    }
}