using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MySql.Data.MySqlClient;
using System.Data;

public class MNG_MENU : SingleTon<MNG_MENU>
{
    [HideInInspector]
    public int RankPage; // 랭킹 페이지 변수
    public UILabel StageLabel; // 스테이지 텍스트 색상을 바꿔주기 위해 만든 변수
    public List<GameObject> Panel; // 판넬 리스트
    public GameObject PageNext, PageBack; // 페이지 넘기는 버튼
    public GameObject PageGrid; // 페이지 그리드
    public List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();  // 닉네임과 점수 저장 리스트

    void Start ()
    {
        StartSettings(); // 메뉴 초기화 함수호출
    }

    void StartSettings() // 메뉴 초기화 함수
    {
        RankSettings();
        LoadDatabase();

        if (MNG_INFO.H.isBack) // 게임에서 메뉴로 돌아온 경우
        {
            Panel[0].SetActive(false); // 0번 판넬은 게임시작 버튼이 있는곳이므로
            Panel[1].SetActive(true); // 1번 판넬인 스테이지 선택창을 띄어준다
            MNG_INFO.H.isBack = false; // 반복되면 안되므로 다시 isBack을 false로 바꿔준다
        }
        MNG_SOUND.H.MusicPause();       // 음악을 멈춰주고
        MNG_SOUND.H.MusicSet("MENU");   // 메뉴 음악으로 설정시킨다
        MNG_SOUND.H.MusicSetLoop(true);  // 반복재생 설정

        if (MNG_SOUND.H.MusicIsPlay()) // 이미 음악이 재생중이라면 끝
            return;
        else
            MNG_SOUND.H.MusicPlay(); // 아닌경우 재생
    }
	
    #region 게임시작
    public void GameStart() // 게임시작 함수
    {
        if (MNG_INFO.H.StageName != null) // 스테이지 이름이 null이 아니면 게임시작 화면으로 넘어간다
            SceneManager.LoadScene("2_GAME");
    }
    #endregion

    #region 메뉴관련 함수들
    public void SelectPanel(GameObject Panel, GameObject Panel2) // 판넬을 켜고 끄기 위해 만든 함수
    {
        Panel.SetActive(false);
        Panel2.SetActive(true);

        if (Panel2.name.Equals("Panel3")) // 판넬3은 랭킹을 보는 곳이므로 랭킹 스크롤 초기화 함수를 호출해준다
            RankSettings();
    }
    public void ShowPopup(GameObject Popup) // 팝업창을 띄어주기 위해 만든 함수
    {
        Popup.SetActive(true);
    }
    public void ClosePopup(GameObject Popup) // 팝업창을 꺼주기 위해 만든 함수
    {
        Popup.SetActive(false);
    }
    #endregion

    #region 랭킹관련 함수들
    void LoadDatabase() // DB 호출
    {
        string url = "server=localhost;user=root;database=project;port=3306;password=201744068;sslmode=none";
        MySqlConnection mConnection = new MySqlConnection(url); // DB접속
        MySqlCommand mCommand = new MySqlCommand(); // 쿼리문 생성
        MySqlDataReader mDataReader;    // 실행문
        mCommand.Connection = mConnection;  // DB에 연결
        mCommand.CommandText = "SELECT * FROM rank order by score desc";    // 쿼리문 작성
        mConnection.Open(); // DB오픈
        mDataReader = mCommand.ExecuteReader(); // 쿼리문 실행

        while (mDataReader.Read())
        {
            string tempNickname = mDataReader["Nickname"].ToString();
            string tempScore = mDataReader["Score"].ToString();
            Dictionary<string, string> Score = new Dictionary<string, string>();    // 닉네임이랑 점수 한 번에 저장
            // 각 개체에 저장
            Score.Add("score", tempScore);
            Score.Add("Nickname", tempNickname);
            // 그 개체 리스트에 저장
            list.Add(Score);
        }
        mConnection.Close();
    }

    void RankSettings() // 랭킹 스크롤 초기화 함수
    {
        RankPage = 0; // 랭킹 페이지 0으로 초기화
        PageNext.SetActive(true); // 랭킹 페이지 넘겨주는 오브젝트 초기화
        PageBack.SetActive(false); // 랭킹 페이지 넘겨주는 오브젝트 초기화
        GetRankList(); // 랭킹셋팅함수 호출
    }

    void GetRankList() // 랭킹 리스트 불러오는 함수
    {
        if (RankPage != 0) // 페이지에 따라 앞에 붙는 숫자변경해주는 작업
        {
            for (int i = 0; i < PageGrid.transform.childCount; i++)
                PageGrid.transform.GetChild(i).Find("Rank").GetComponent<UILabel>().text = ((i + 1) + (RankPage * 10)).ToString() + "th";
        }
        else
        {
            PageGrid.transform.GetChild(0).Find("Rank").GetComponent<UILabel>().text = "1st";
            PageGrid.transform.GetChild(1).Find("Rank").GetComponent<UILabel>().text = "2nd";
            PageGrid.transform.GetChild(2).Find("Rank").GetComponent<UILabel>().text = "3rd";

            for (int i = 3; i < PageGrid.transform.childCount; i++)
                PageGrid.transform.GetChild(i).Find("Rank").GetComponent<UILabel>().text = (i + 1).ToString() + "th";
        } // 페이지에 따라 앞에 붙는 숫자변경해주는 작업 끝

        for (int i = 0; i < list.Count; i++) // 점수를 텍스트로 옮겨주는 작업
        {
            string score = "";  // 점수
            string nickname = "";   // 닉네임
            list[i].TryGetValue("Nickname", out nickname);
            list[i].TryGetValue("score", out score);
            PageGrid.transform.GetChild(i).Find("Score").GetComponent<UILabel>().text = nickname + " : " + score;
        }
    }

    public void RankPageFunc(GameObject Page) // 랭킹 페이지 함수
    {
        GetRankList(); // 랭킹셋팅함수 호출

        if (Page.name.Equals("Rank_Next")) // 다음 페이지로 넘기는 버튼설정
        {
            PageBack.SetActive(true);

            if (RankPage < 4)
                RankPage++;

            if (RankPage == 4)
                Page.SetActive(false);
        }
        else if (Page.name.Equals("Rank_Back")) // 이전 페이지로 넘기는 버튼설정
        {
            PageNext.SetActive(true);

            if (RankPage > 0)
                RankPage--;

            if (RankPage == 0)
                Page.SetActive(false);
        }
        GetRankList();
    }
    #endregion

    void OnTriggerEnter2D(Collider2D collision) // 이 스크립트가 들어간 오브젝트와 2D Collider의 Trigger 체크가 되어 있는 오브젝트가 충돌했을 경우 들어오는 함수 (인자값으로 충돌한 오브젝트가 들어온다)
    {
        if (collision.tag.Equals("MAP")) // 충돌한 오브젝트의 tag가 MAP일 경우 들어오게 설정 (스테이지 오브젝트의 tag가 MAP이다)
        {
            StageLabel.color = collision.transform.Find("Background").GetComponent<UISprite>().color; // Select Stage 라벨의 색상을 맵 백그라운드 색상과 일치하게 설정
            collision.transform.localScale = Vector3.one * 1.3f; // 중앙에 있는 스테이지 크기 확대
            MNG_INFO.H.StageName = collision.transform.Find("Label").GetComponent<UILabel>().text; // 나중에 게임 안에서 스테이지를 구분하기 위해 스테이지 이름을 MNG_INFO의 StageName에 넣어준다
        }
    }

    void OnTriggerExit2D(Collider2D collision) // 위의 함수랑은 반대로 충돌했다가 떨어저 나갔을 경우에 들어오는 함수 (인자값으로 빠져나간 오브젝트가 들어온다)
    {
        if (collision.tag.Equals("MAP")) // 충돌한 오브젝트의 tag가 MAP일 경우 들어오게 설정 (스테이지 오브젝트의 tag가 MAP이다)
            collision.transform.localScale = Vector3.one; // 가운데에서 떨어지면 크기를 원래대로 돌려준다
    }
}
