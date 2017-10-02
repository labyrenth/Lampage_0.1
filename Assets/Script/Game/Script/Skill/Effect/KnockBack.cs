using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : SkillEffectBase {

    public enum KnockBackEffectState
    {
        Default,
        ReadyToActivate,
        BeingActivate,
        ActivateFinish
    }
    [Range(1,20)]
    public float knockBackPower;
    private Quaternion targetQuaternion;
    private KnockBackEffectState knockbackState;
    private void Awake()
    {
        this.skillEffectType = SkillEffectType.KnockBack;
        knockbackState = KnockBackEffectState.Default;
    }

    /*
    private void KnockBack(Vector3 targetV)
    {
        this.gameObject.transform.localPosition = new Vector3(0, 26, 0);
        this.gameObject.transform.localRotation = Quaternion.identity;
        Vector3 knockBackVector = this.gameObject.transform.position - targetV;
        Quaternion Q = Quaternion.FromToRotation(this.gameObject.transform.position, knockBackVector) * PMI.GetPlayerParent().rotation;
        Quaternion QQ = Quaternion.Euler(Q.eulerAngles);
        float flowtime = Time.fixedTime - PMI.Time;
        PMI.GetPlayerParent().rotation = Quaternion.Slerp(PMI.GetPlayerParent().rotation, QQ, (1f - (Mathf.Sqrt(flowtime))) * GameTime.FrameRate_60_Time);
    }*/

    public void SetKnockBackQuaternion(Transform To_Affect, Transform Be_Affected)
    {
        Vector3 knockBackVector = (Be_Affected.position - To_Affect.position).normalized;
        this.targetQuaternion = Quaternion.FromToRotation(Be_Affected.position, knockBackVector);
        knockbackState = KnockBackEffectState.ReadyToActivate;
    }

    public IEnumerator KnockBackActivate(Transform targetTransform)
    {
        knockbackState = KnockBackEffectState.BeingActivate;
        WaitForSeconds waitForFrame = new WaitForSeconds(GameTime.FrameRate_60_Time);
        Quaternion initialQuaternion = targetTransform.rotation;
        Quaternion FinalQuaternion = Quaternion.Euler((targetQuaternion * targetTransform.rotation).eulerAngles);
        for (float i = 0; i < effectDuration; i += GameTime.FrameRate_60_Time)
        {
            targetTransform.rotation = Quaternion.Slerp(initialQuaternion, FinalQuaternion, (Mathf.Sqrt(i*knockBackPower/20)));
            yield return waitForFrame;
        }

        this.gameObject.transform.localPosition = new Vector3(0, 26, 0);
        this.gameObject.transform.localRotation = Quaternion.identity;
        knockbackState = KnockBackEffectState.ActivateFinish;
    }

    public KnockBackEffectState GetKnockBackEffectState()
    {
        return this.knockbackState;
    }
}
