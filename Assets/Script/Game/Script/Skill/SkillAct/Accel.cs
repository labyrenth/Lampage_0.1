using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accel : SkillBase {

    // Use this for initialization
    public override void Awake()
    {
        base.Awake();
	}

    protected override void Start()
    {
        base.Start();
        
    }

    protected override IEnumerator ActivityDuringDurationTime()
    {
        Owner.GetPlayerState().SetEffectedList(this.skillEffectList);
        SkillSoundEffect("SkillEffect_Run", durationTime,1f);
        return base.ActivityDuringDurationTime();
    }

}
