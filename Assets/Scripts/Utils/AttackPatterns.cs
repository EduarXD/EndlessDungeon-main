using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackPatterns : MonoBehaviour
{

    [System.Serializable]
    public struct AttackSequence
    {
        public bool cancellable;
        public float wait_min;
        public float wait_max;
        public Attack[] attack;
        [System.Serializable]
        public struct Attack
        {
            public bool cancellable;
            public UnityEvent onStart;
            public UnityEvent onEnd;
            public float duration_min;
            public float duration_max;
            public float duration_enable_start;
            public UnityEvent onEnable;
            public Behaviour enable;
            public float duration_enable_end;
            public UnityEvent onDisable;
            public float wait_min;
            public float wait_max;
        }
    }
    public bool canAttack;
    public AttackSequence[] attacks;
    public int currentAttackSequence
    {
        get;
        protected set;
    } = -1;
    public int currentAttack
    {
        get;
        private set;
    } = -1;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        while (true)
        {
            if(canAttack && attacks.Length > 0 && currentAttackSequence < 0)
            {
                currentAttackSequence = Random.Range(0, attacks.Length);
            }
            if (attacks[currentAttackSequence].attack.Length > 0)
            {
                if(currentAttack < 0)
                {
                    currentAttack = 0;
                } else if(currentAttack >= attacks[currentAttackSequence].attack.Length)
                {
                    currentAttack = -1;
                    currentAttackSequence = -1;
                }
            }
            else
            {
                currentAttack = -1;
            }
            if(currentAttackSequence >= 0 && currentAttackSequence < attacks.Length)
            {
                float time;
                if(currentAttack >= 0 && currentAttack < attacks[currentAttackSequence].attack.Length)
                {
                    attacks[currentAttackSequence].attack[currentAttack].onStart.Invoke();
                    time = Random.Range(attacks[currentAttackSequence].attack[currentAttack].duration_min, attacks[currentAttackSequence].attack[currentAttack].duration_max);
                    if (time > 0 && (canAttack || !attacks[currentAttackSequence].attack[currentAttack].cancellable))
                    {
                        if(time > attacks[currentAttackSequence].attack[currentAttack].duration_enable_start && time < attacks[currentAttackSequence].attack[currentAttack].duration_enable_end && !attacks[currentAttackSequence].attack[currentAttack].enable.enabled)
                        {
                            attacks[currentAttackSequence].attack[currentAttack].enable.enabled = true;
                            attacks[currentAttackSequence].attack[currentAttack].onEnable.Invoke();
                        }
                        else if(time > attacks[currentAttackSequence].attack[currentAttack].duration_enable_end && attacks[currentAttackSequence].attack[currentAttack].enable.enabled)
                        {
                            attacks[currentAttackSequence].attack[currentAttack].enable.enabled = false;
                            attacks[currentAttackSequence].attack[currentAttack].onDisable.Invoke();
                        }
                        time -= Time.deltaTime;
                        yield return null;
                    }
                    time = Random.Range(attacks[currentAttackSequence].attack[currentAttack].wait_min, attacks[currentAttackSequence].attack[currentAttack].wait_max);
                    if (time > 0 && (canAttack || !attacks[currentAttackSequence].attack[currentAttack].cancellable))
                    {
                        time -= Time.deltaTime;
                        yield return null;
                    }
                    attacks[currentAttackSequence].attack[currentAttack].onEnd.Invoke();
                }
                time = Random.Range(attacks[currentAttackSequence].wait_min, attacks[currentAttackSequence].wait_max);
                if (time > 0 && (canAttack || !attacks[currentAttackSequence].cancellable))
                {
                    time -= Time.deltaTime;
                    yield return null;
                }
            }

            yield return null;
        }
    }
}
