using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 이 스크립트를 추가 할 경우 아래의 스크립트도 같이 추가됩니다
[RequireComponent(typeof(DontDestroy))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioListener))]
#endregion

public class MNG_SOUND : SingleTon<MNG_SOUND>
{
    public List<AudioClip> SOUND_LIST = new List<AudioClip>(); // 음악을 넣어둘 리스트

    void Start()
    {
    }

    public void MusicSet(int _music) // 재생할 음악을 설정 (순번으로 찾기)
    {
        gameObject.GetComponent<AudioSource>().clip = SOUND_LIST[_music];
    }

    public void MusicSet(string _music) // 재생할 음악을 설정 (이름으로 찾기)
    {
        for (int i = 0; i < SOUND_LIST.Count; i++)
        {
            if (SOUND_LIST[i] == null) // 음악리스트에 없을경우 Resource.Load를 이용해 Audio폴더 내에 찾고자 하는 음악을 찾아 리스트에 넣어줍니다
            {
                SOUND_LIST[i] = Resources.Load("Audio/" + _music) as AudioClip;
                gameObject.GetComponent<AudioSource>().clip = SOUND_LIST[i];
                break;
            }
            else if (SOUND_LIST[i].name.Equals(_music)) // 음악 리스트에 해당 음악이 있을 경우 재생할 음악을 해당 음악으로 바꿔줍니다
            {
                gameObject.GetComponent<AudioSource>().clip = SOUND_LIST[i];
                break;
            }
        }
    }

    public void MusicPlay() // 음악재생
    {
        if (Application.loadedLevelName.Equals("1_MENU")) // 게임에서 메뉴로 돌아올때 가끔 음악재생시간 문제로 오류가나서 음악 시간을 0으로 초기화 하였습니다
            gameObject.GetComponent<AudioSource>().time = 0;

        gameObject.GetComponent<AudioSource>().Play();
    }

    public void MusicPause() // 현재 재생중인 음악을 일시정지
    {
        gameObject.GetComponent<AudioSource>().Pause();
    }

    public void MusicUnPause() // 일시정지 중인 음악을 다시 재생
    {
        gameObject.GetComponent<AudioSource>().UnPause();
    }

    public bool MusicIsPlay() // 음악이 현재 재생중인지 구분
    {
        return gameObject.GetComponent<AudioSource>().isPlaying;
    }

    public void MusicSetLoop(bool _loop) // 음악 반복재생 설정
    {
        gameObject.GetComponent<AudioSource>().loop = _loop;
    }
}