using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPatterns : MonoBehaviour
{

    [System.Serializable]
    public struct AttackSequence
    {
        public float wait_min;
        public float wait_max;
        [System.Serializable]
        public struct Attack
        {
            public LifeTaker taker;
            public float duration_min;
            public float duration_max;
            public float duration_enable_start;
            public float duration_enable_end;
            public Collider2D[] enableColliders;
            public float wait_min;
            public float wait_max;
        }
    }
    public bool canAttack;
    public AttackSequence[] attacks;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        while (true)
        {
            while (canAttack && attacks.Length > 0)
            {
                int currentAttackGroup = Random.Range(0, attacks.Length);
                
            }
            yield return null;
        }
    }
}
