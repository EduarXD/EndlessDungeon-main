using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class LifeController : MonoBehaviour
{
    public int lifes;
    [Min(1)]
    public int maxLifes;
    public float invencibleTime;
    public bool invencible;
    public UnityEvent onDamage;
    public UnityEvent onKillStart;
    public UnityEvent onKill;

    public Rigidbody2D rb;
    public SpriteRenderer sr;

    public Color damageColor;
    public float damageDuration;
    public Color invencibleColor;

    public enum DEATHTYPE { Destroy, CheckPoint, LoadScene }
    public DEATHTYPE deathType;
    public string loadSceneName;
    public float deathDuration;

    public Vector3 checkpoint;

    void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();
        checkpoint = transform.position;
    }
    public void ModLife(int life)
    {
        lifes = Mathf.Clamp(lifes + life, 0, maxLifes);
    }
    public void ModMaxLife(int life)
    {
        maxLifes += life;
        if (maxLifes <= 0) maxLifes = 1;
        if (life > 0)
            lifes = maxLifes;
    }
    public void Damage(int damage, float knockbackMultiply = 0, float knockbackAdd = 0, float pushup = 0, bool ignoreInvencible = false)
    {
        if (invencible && !ignoreInvencible)
            return;
        StartCoroutine(Invencible_Corutine());
        if (rb != null)
        {
            rb.velocity = new Vector2(rb.velocity.x * -knockbackMultiply + knockbackAdd, pushup);
        }
        lifes -= damage;
        if (lifes <= 0)
        {
            lifes = 0;
            Kill();
        }
        onDamage.Invoke();
    }

    public void Kill()
    {
        StartCoroutine(Kill_Coroutine());
    }
    public void SetCheckpoint(Vector3 checkpoint)
    {
        this.checkpoint = checkpoint;
    }
    public void SetCheckpoint(Transform checkpoint)
    {
        this.checkpoint = checkpoint.position;
    }
    IEnumerator Kill_Coroutine()
    {
        onKillStart.Invoke();
        yield return new WaitForSeconds(deathDuration);
        switch (deathType)
        {
            case DEATHTYPE.CheckPoint:
                if (rb != null)
                    rb.velocity = Vector2.zero;
                transform.position = checkpoint;
                lifes = maxLifes;
                break;
            case DEATHTYPE.LoadScene:
                UnityEngine.SceneManagement.SceneManager.LoadScene(loadSceneName);
                lifes = 3;
                break;
            default:
                DestroyImmediate(gameObject);
                break;
        }
        onKill.Invoke();
    }
    IEnumerator Invencible_Corutine()
    {
        invencible = true;
        float count = 0;
        if (sr != null)
            sr.color = damageColor;
        while (count < damageDuration && count < invencibleTime)
        {
            count += Time.deltaTime;
            yield return null;
        }
        if (sr != null)
            sr.color = invencibleColor;
        while (count < invencibleTime)
        {
            count += Time.deltaTime;
            yield return null;
        }
        sr.color = Color.white;
        invencible = false;
    }
}
