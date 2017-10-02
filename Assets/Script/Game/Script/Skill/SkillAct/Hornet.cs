﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hornet : SkillBase {

    public float speed = 20;
    public float maxDegree = 10;
    public float freezeTime = 3f;

    // Use this for initialization
    public override void Awake()
    {
        base.Awake();
        SS = SkillState.ACTIVATED;
	}

    protected override void Start()
    {
        base.Start();
    }

    private void HornetAction(GameObject Target)
    {
        float slowSpeed = speed / 10;
        float goSpeed;

        if (Target != null && !Target.GetComponent<PlayerControlThree>().GetPlayerState().InHQ )
        {
            goSpeed = speed;
        }
        else
        {
            goSpeed = slowSpeed;
        }
        
        if (SS == SkillState.LAUNCHED)
        {
            this.SkillParent.transform.rotation = TurnToTarget();
            this.SkillParent.transform.rotation *= GoStraight(goSpeed);
        }
    }

    private Quaternion TurnToTarget()
    {
        float angle;
        Vector3 PO = this.gameObject.transform.position;
        Vector3 TO = TG.transform.position;
        Vector3 PTVector = TO - PO;
        angle = Vector3.Dot(this.gameObject.transform.right, PTVector);
        Quaternion AA = Quaternion.AngleAxis(angle, SkillParent.transform.up) * this.SkillParent.transform.rotation;
        return Quaternion.Slerp(this.SkillParent.transform.rotation, AA, maxDegree * GameTime.FrameRate_60_Time);
    }

    private Quaternion GoStraight(float SP)
    {
        return Quaternion.Euler(new Vector3(SP * GameTime.FrameRate_60_Time, 0, 0));
    }

    protected override IEnumerator ActivityDuringWaitTime()
    {
        SkillSoundEffect("SkillEffect_Bee", waitTime, 0.25f);
        return base.ActivityDuringWaitTime();
    }

    protected override IEnumerator ActivityDuringDurationTime()
    {
        ChangeSkillStateLaunched();
        SkillSoundEffect("SkillEffect_Bee", durationTime, 1f);
        return base.ActivityDuringDurationTime();
    }

    public override void CollideSkillAction(Collider other)
    {
        if (other.gameObject.tag == "Head" && other.gameObject == TG)
        {
            PlayerControlThree otherPCT = other.GetComponent<PlayerControlThree>();
            if (!otherPCT.GetPlayerState().InHQ)
            {
                otherPCT.GetPlayerState().SetEffectedList(this.skillEffectList);
            }
            SkillParent.SetActive(false);
        }
    }

    public override void SetPivot(Transform pivot, Transform pivotRotation, float angle, Vector3 skillVector)
    {
        base.SetPivot(pivot, pivotRotation, angle, skillVector);
    }

    public override float ShowPreCooltime()
    {
        return base.ShowPreCooltime();
    }

    public override bool GetIsSkillNeedGuideLine()
    {
        return base.GetIsSkillNeedGuideLine();
    }

    private void Update()
    {
        HornetAction(TG);
    }
}
