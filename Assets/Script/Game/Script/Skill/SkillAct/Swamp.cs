using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swamp : SkillBase {


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
        SkillSoundEffect("SkillEffect_Swamp", durationTime);
        yield return base.ActivityDuringDurationTime();
        this.gameObject.GetComponent<Collider>().enabled = false;
    }


    Quaternion targetRotation(Vector3 target)
    {
        Quaternion Q = Quaternion.FromToRotation(this.transform.position, target) * SkillParent.transform.rotation;
        return Quaternion.Euler(Q.eulerAngles);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Head")
        {
            other.gameObject.GetComponent<PlayerControlThree>().GetPlayerState().SetEffectedList(this.skillEffectList);
        }
    }

    public override void SetPivot(Transform pivot, Transform pivotRotation, float angle, Vector3 skillVector)
    {
        base.SetPivot(pivot, pivotRotation, angle, skillVector);
        SkillParent.transform.rotation = targetRotation(skillVector);
    }

    public override float ShowPreCooltime()
    {
        return base.ShowPreCooltime();
    }

    public override bool GetIsSkillNeedGuideLine()
    {
        return base.GetIsSkillNeedGuideLine();
    }

}
