using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : EntityState
{
    protected PlayerStateHandler player;
    
    protected float airTime;


    //protected int currentStateNum;        ���� ������Ʈ ��
    //protected float stateTimer;
    //protected float startTime;                
    //protected bool endMotionChange = true; ������ �ִϸ��̼� �ٲ��Ʈ����
    //protected bool isAbleFly = false; �̰� ���� 
    //protected bool isAbleAttack = true;

    public PlayerState(PlayerStateHandler _player, int _currentStateNum)
    {
        player = _player;
        currentStateNum = _currentStateNum;
    }
    public override void Enter()
    {
        base.Enter();
        startTime = Time.time;
        player.SetInt("State", currentStateNum);
        if (currentStateNum != 0) { player.animationTrigger = true; }
    }
    public override void Update()
    {
        base.Update();
        stateTimer = Time.time;

        //����
        if(Input.GetKeyDown(KeyCode.X) && isAbleAttack) 
        {
            player.StateChange(player.attackState);
            return;
        }
        if (!isAbleFly)
        {
            //ü�� �Ұ��� �����϶�
            if (!player.IsGround())
            {
                if(airTime == 0f)
                {
                    airTime = Time.time;
                }
                if(Time.time - airTime > 0.25f)
                {
                    player.StateChange(player.fallState);
                    return;
                }
            }
            else
            {
                airTime = 0f;
            }
        }
        if (Input.GetKeyDown(KeyCode.C) && isAbleDodge)
        {
            player.StateChange(player.attackState);
            return;
        }
        BaseState();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }
    

    
    protected void BaseState()
    {
        
        if (!player.animationTrigger && endMotionChange)
        {
            if (player.IsGround()) 
            {
                player.StateChange(player.moveState);
            }
            else
            {
                player.StateChange(player.fallState);
            }
        }
    }


}
