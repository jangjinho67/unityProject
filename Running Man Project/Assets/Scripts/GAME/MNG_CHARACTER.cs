using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MNG_CHARACTER : SingleTon<MNG_CHARACTER>
{
    public enum STATUS { C_STOP, C_MOVE, C_JUMP, C_SLIDE };
    [HideInInspector]
    public STATUS C_STATUS;

    private Rigidbody2D rig;

    private bool isJump = false;
    private bool isSlide = false;

    void Start()
    {
        rig = transform.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        switch (C_STATUS)
        {
            case STATUS.C_MOVE:
                transform.GetComponent<UISpriteAnimation>().namePrefix = "PLR_ANIM_MOVE_";
                transform.localPosition = new Vector3(-650, -270, 0);
                transform.localEulerAngles = Vector3.zero;
                break;
            case STATUS.C_JUMP:
                if (!isJump)
                {
                    transform.GetComponent<UISpriteAnimation>().namePrefix = "PLR_ANIM_JUMP";
                    transform.GetComponent<UISprite>().spriteName = "PLR_ANIM_JUMP";
                    rig.velocity = new Vector2(rig.velocity.x, 180 * Time.fixedDeltaTime);
                    isJump = true;
                }
                break;
            case STATUS.C_SLIDE:
                transform.GetComponent<UISpriteAnimation>().namePrefix = "PLR_ANIM_SLIDE";
                transform.localPosition = new Vector3(-650, -320, 0);
                transform.localEulerAngles = new Vector3(0, 0, 90);
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(!isJump)
                C_STATUS = STATUS.C_SLIDE;
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            C_STATUS = STATUS.C_MOVE;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            C_STATUS = STATUS.C_JUMP;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GROUND"))
        {
            C_STATUS = STATUS.C_MOVE;
            isJump = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("OBJECT_"))
        {
            TweenFill.Begin(MNG_OBJECT.H.HpBar.transform.GetChild(0).gameObject, 0.15f, (MNG_OBJECT.H.HpBar.transform.GetChild(0).GetComponent<UISprite>().fillAmount - 0.1f));
            if (collision.CompareTag("OBJECT_FLOOR"))
                collision.transform.localPosition = new Vector3(950, 0, 0);
            else if (collision.CompareTag("OBJECT_SKY"))
                collision.transform.localPosition = new Vector3(950, 125, 0);
            collision.gameObject.SetActive(false);
        }
    }
}