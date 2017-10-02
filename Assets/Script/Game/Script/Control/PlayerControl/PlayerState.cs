using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerControlThree))]

public class PlayerState : MonoBehaviour
{
    private struct EffectedStruct
    {
        public SkillEffectBase targetEffect { get; private set; }
        public float inputTime { get; private set; }

        public EffectedStruct(SkillEffectBase targetEffect, float inputTime)
        {
            this.targetEffect = targetEffect;
            this.inputTime = inputTime;
        }
    }

    public bool InHQ { get; set; }

    private float multiplyValue;
    private bool isFreeze;
    private bool isKnockBack;
    private bool isImmune;
    private PlayerControlThree Owner;

    public float MultiplyValue { get { return multiplyValue; } private set { multiplyValue = value; } }
    public bool IsFreeze { get { return isFreeze; } private set { isFreeze = value; } }
    public bool IsKnockBack { get { return isKnockBack; } private set { isKnockBack = value; } }
    public bool IsImmune { get { return isImmune; } private set { isImmune = value; } }

    private List<EffectedStruct> SpeedMultiplyEffectList;
    private List<EffectedStruct> FreezeEffectList;
    private List<EffectedStruct> KnockBackEffectList;
    private List<EffectedStruct> ImmuneEffectList;

    private void Awake()
    {
        InHQ = true;
        multiplyValue = 1f;
        isFreeze = false;
        isKnockBack = false;
        isImmune = false;
        SpeedMultiplyEffectList = new List<EffectedStruct>();
        FreezeEffectList = new List<EffectedStruct>();
        KnockBackEffectList = new List<EffectedStruct>();
        ImmuneEffectList = new List<EffectedStruct>();
        Owner = GetComponent<PlayerControlThree>();
    }

    public void SetEffectedList(List<SkillEffectBase> targetList)
    {
        foreach (SkillEffectBase target in targetList)
        {
            if (target.GetType().Equals(typeof(SpeedMultiple)))
                SetupEffectList(target as SpeedMultiple);
            else if (target.GetType().Equals(typeof(Freeze)))
                SetupEffectList(target as Freeze);
            else if (target.GetType().Equals(typeof(KnockBack)))
                SetupEffectList(target as KnockBack);
            else if (target.GetType().Equals(typeof(Immune)))
                SetupEffectList(target as Immune);
        }
    }

    private void SetupEffectList(SpeedMultiple targetEffect)
    {
        bool IsInputAble = true;
        //SpeedMultiplyEffectList를 돌아본뒤에 중복되는 이름이 없을경우 List에 추가.
        //먼저 중복되는 이름이 있는지부터 검사.
        for (int i = SpeedMultiplyEffectList.Count - 1; i >= 0; i--)
        {
            EffectedStruct es = SpeedMultiplyEffectList[i];
            if (es.targetEffect.effectName.Equals(targetEffect.effectName))
            {
                IsInputAble = false;
            }
        }
        //중복되는 이름이 없을경우 새로이 추가한다.
        if (IsInputAble && !isImmune)
        {
            SpeedMultiplyEffectList.Add(new EffectedStruct(targetEffect, ManagerHandler.Instance.GameTime().GetTime()));
        }
    }

    private void SetupEffectList(Freeze targetEffect)
    {
        bool IsInputAble = true;
        //SpeedMultiplyEffectList를 돌아본뒤에 중복되는 이름이 없을경우 List에 추가.
        //먼저 중복되는 이름이 있는지부터 검사.
        for (int i = FreezeEffectList.Count - 1; i >= 0; i--)
        {
            EffectedStruct es = FreezeEffectList[i];
            if (es.targetEffect.effectName.Equals(targetEffect.effectName))
            {
                IsInputAble = false;
            }
        }
        //중복되는 이름이 없을경우 새로이 추가한다.
        if (IsInputAble && !isImmune)
        {
            FreezeEffectList.Add(new EffectedStruct(targetEffect, ManagerHandler.Instance.GameTime().GetTime()));
        }
    }

    private void SetupEffectList(KnockBack targetEffect)
    {
        bool IsInputAble = true;
        if (KnockBackEffectList.Count > 1)
        {
            //KnockBack은 한번에 한개만.
            IsInputAble = false;
        }

        if (IsInputAble && !isImmune)
        {
            this.KnockBackEffectList.Add(new EffectedStruct(targetEffect, ManagerHandler.Instance.GameTime().GetTime()));
        }

    }

    private void SetupEffectList(Immune targetEffect)
    {
        bool IsInputAble = true;
        //SpeedMultiplyEffectList를 돌아본뒤에 중복되는 이름이 없을경우 List에 추가.
        //먼저 중복되는 이름이 있는지부터 검사.
        for (int i = ImmuneEffectList.Count - 1; i >= 0; i--)
        {
            EffectedStruct es = ImmuneEffectList[i];
            if (es.targetEffect.effectName.Equals(targetEffect.effectName))
            {
                IsInputAble = false;
            }
        }
        //중복되는 이름이 없을경우 새로이 추가한다.
        if (IsInputAble && !isImmune)
        {
            ImmuneEffectList.Add(new EffectedStruct(targetEffect, ManagerHandler.Instance.GameTime().GetTime()));
        }
    }

    private bool CheckEffectTimeisOver(EffectedStruct target)
    {
        float passedTime = target.inputTime + target.targetEffect.effectDuration;
        if (ManagerHandler.Instance.GameTime().GetTime() > passedTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void CheckSpeedMultipleEffectList()
    {
        float tempValue = 1;
        if (SpeedMultiplyEffectList != null)
        {
            for (int i = SpeedMultiplyEffectList.Count - 1; i >= 0; i--)
            {
                EffectedStruct effect = SpeedMultiplyEffectList[i];
                tempValue *= (effect.targetEffect as SpeedMultiple).GetMultipleValue();
                if (CheckEffectTimeisOver(effect))
                {
                    SpeedMultiplyEffectList.Remove(effect);
                }
            }
        }
        multiplyValue = tempValue;
    }

    private void CheckFreezeEffectList()
    {
        bool isFreezeTemp = false;

        if (FreezeEffectList != null && FreezeEffectList.Count > 0)
        {
            isFreezeTemp = true;
            for (int i = FreezeEffectList.Count - 1; i >= 0; i--)
            {
                EffectedStruct effect = FreezeEffectList[i];
                if (CheckEffectTimeisOver(effect))
                {
                    FreezeEffectList.Remove(effect);
                }
            }
        }
        isFreeze = isFreezeTemp;
    }

    private void CheckKnockBack()
    {
        bool tempisKnockBack = false;
        if (!KnockBackEffectList.Equals(null) && KnockBackEffectList.Count > 0)
        {
            tempisKnockBack = true;
            KnockBack effect = KnockBackEffectList[0].targetEffect as KnockBack;
            if (effect.GetKnockBackEffectState().Equals(KnockBack.KnockBackEffectState.ReadyToActivate))
            {
                StartCoroutine(effect.KnockBackActivate(this.Owner.GetPMI().GetPlayerParent().transform));
            }
            else if (effect.GetKnockBackEffectState().Equals(KnockBack.KnockBackEffectState.ActivateFinish))
            {
                KnockBackEffectList.RemoveAt(0);
            }
        }
        isKnockBack = tempisKnockBack;
    }

    private void CheckImmune()
    {
        bool tempisImmune = false;

        if (!ImmuneEffectList.Equals(null) && ImmuneEffectList.Count > 0)
        {
            tempisImmune = true;
            for (int i = ImmuneEffectList.Count - 1; i >= 0; i--)
            {
                EffectedStruct effect = ImmuneEffectList[i];
                if (CheckEffectTimeisOver(effect))
                {
                    ImmuneEffectList.Remove(effect);
                }
            }
        }

        isImmune = tempisImmune;
    }

    public bool IsPlayerCanMove()
    {
        if (isKnockBack || isFreeze)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void FixedUpdate()
    {
        if (GameTime.IsTimerStart())
        {
            CheckSpeedMultipleEffectList();
            CheckFreezeEffectList();
            CheckKnockBack();
            CheckImmune();
        }
    }
}
