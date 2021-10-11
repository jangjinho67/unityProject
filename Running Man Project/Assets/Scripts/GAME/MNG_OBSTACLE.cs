using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MNG_OBSTACLE : SingleTon<MNG_OBSTACLE>
{
	void Start ()
    {
        if(transform.CompareTag("OBJECT_FLOOR")) // 방해 오브젝트의 tag가 OBJECT_FLOOR인 경우 좌표설정
            transform.localPosition = new Vector3(950, 0, 0);
        else if (transform.CompareTag("OBJECT_SKY")) // 방해 오브젝트의 tag가 OBJECT_SKY인 경우 좌표설정
            transform.localPosition = new Vector3(950, 125, 0);
    }

    void Update()
    {
        if (!MNG_OBJECT.H.isPause) // 현재 일시정지 중이 아닌경우
        {
            // 오브젝트 속도만큼 x좌표를 움직인다
            transform.localPosition = new Vector3(transform.localPosition.x - ((MNG_OBJECT.H.Speed + 400) * Time.deltaTime), transform.localPosition.y, 0);

            if (transform.localPosition.x < -950) // 오브젝트가 화면에서 벗어난 경우 초기화
            {
                if (transform.CompareTag("OBJECT_FLOOR")) // 방해 오브젝트의 tag가 OBJECT_FLOOR인 경우 좌표 초기화
                    transform.localPosition = new Vector3(950, 0, 0);
                else if (transform.CompareTag("OBJECT_SKY")) // 방해 오브젝트의 tag가 OBJECT_SKY인 경우 좌표 초기화
                    transform.localPosition = new Vector3(950, 125, 0);

                gameObject.SetActive(false); // active를 false로 바꿔주어 메모리 낭비를 줄여준다
            }
        }
    }
}
