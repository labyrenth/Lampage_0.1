using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : SkillBase
{
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
        SkillSoundEffect("SkillEffect_Shield", durationTime, 1f);
        return base.ActivityDuringDurationTime();
    }

    private void Update()
    {
        this.SkillParent.transform.rotation = Owner.transform.rotation;
    }
}
