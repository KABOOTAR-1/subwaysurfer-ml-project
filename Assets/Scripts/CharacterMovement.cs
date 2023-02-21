using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum SIDE { Left,Mid,Right}
public class CharacterMovement : MonoBehaviour
{
    public SIDE curr_side = SIDE.Mid;
    public bool onLeft, onRight,onUp,onDown;
    float NewPos = 0;
    public float Xval = 0;
    [SerializeField] CharacterController m_char;
    public float SpeedDodge;
    public float JumpPower = 7f;
    float x,y;
    public Animator anime;
    public bool InJump,inRoll;
    public float FwdSpeed;
    void Start()
    {
        m_char = GetComponent<CharacterController>();
        transform.position = Vector3.zero;
        anime = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        onLeft = Input.GetKeyDown(KeyCode.A)|| Input.GetKeyDown(KeyCode.LeftArrow);
        onRight = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
        onUp = Input.GetKeyDown(KeyCode.Space);
        onDown = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);
        if (onLeft)
        {
            if (curr_side == SIDE.Mid)
            {
                NewPos = -Xval;
                curr_side = SIDE.Left;
            }else if (curr_side == SIDE.Right)
            {
                NewPos = 0;
                curr_side = SIDE.Mid;
            }
        }
        else if(onRight)
        {
            if (curr_side == SIDE.Mid)
            {
                NewPos = Xval;
                curr_side = SIDE.Right;
            }
            else if (curr_side == SIDE.Left)
            {
                NewPos = 0;
                curr_side = SIDE.Mid;
            }
        }
        Vector3 moveVector = new Vector3(x - transform.position.x, y*Time.deltaTime, FwdSpeed*Time.deltaTime);
        x = Mathf.Lerp(x, NewPos, Time.deltaTime * SpeedDodge);
        m_char.Move(moveVector);
        Jump();
        Roll();
    }

    public void Jump()
    {
        if (m_char.isGrounded)
        {
            if (onUp)
            {
                y = JumpPower;
                anime.CrossFadeInFixedTime("Jumping", 0.1f);
                InJump = true;
            }
        }
        else
        {
            y -= JumpPower * 2 * Time.deltaTime;
            InJump = false;
        }
    }
    internal float RollCounter;
    public void Roll()
    {
        RollCounter -= Time.deltaTime;
        if (RollCounter <= 0f)
        {
            RollCounter = 0f;
            inRoll = false;
        }
        if (onDown)
        {
            RollCounter = 1.167f;
            y -= 10f;
            anime.CrossFadeInFixedTime("Rolling", 0.1f);
            inRoll = true;
            InJump = false;
        }
    }
}
