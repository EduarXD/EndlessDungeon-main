using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCamera : MonoBehaviour
{
    public Camera cam;
    public Transform target;
    public Vector2 offset = Vector2.up;
    public float aspectRatio = 16f / 9f;
    public enum MODE { FADE, TRANSLATE};
    public MODE mode;
    public CanvasGroup fade;
    public float transitionDuration;
    public AnimationCurve transition = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public bool transitionStop;
    public bool animating { get; private set; }
    public Vector2 size { get; private set; }
    public Vector2 sizeInv { get; private set; }
    float startZ;
    public Vector2 startPos { get; private set; } = Vector2.zero;
    public Vector2 endPos { get; private set; } = Vector2.zero;
    // Start is called before the first frame update
    void Awake()
    {
        Reset();
        GameManager.scripts.Add(GameManager.SCRIPTS.GridCamera, this);
    }
    private void OnValidate()
    {
        Reset();
    }
    private void Reset()
    {
        startPos = transform.position;
        startZ = transform.position.z;
        endPos = transform.position;
        size = new Vector2(aspectRatio, 1) * cam.orthographicSize * 2;
        sizeInv = new Vector2(1 / size.x, 1 / size.y);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEndPos();
        if(startPos != endPos && !animating)
        {
            StartCoroutine(Animate_Coroutine());
        }
    }
    void UpdateEndPos()
    {

        Vector2 targetPos = (Vector2)target.position + offset;
        targetPos = Vector2.Scale(targetPos, sizeInv);
        targetPos.x = Mathf.Round(targetPos.x);
        targetPos.y = Mathf.Round(targetPos.y);
        endPos = Vector2.Scale(targetPos, size);
    }

    IEnumerator Animate_Coroutine()
    {
        animating = true;
        transform.position = new Vector3(startPos.x, startPos.y, startZ);
        float count = 0;
        if (transitionStop)
            Time.timeScale = 0;
        while (count <= 1)
        {
            float amount = transition.Evaluate(count);
            if(mode == MODE.FADE || (startPos.x != endPos.x && startPos.y != endPos.y))
            {
                amount = transition.Evaluate(1 - Mathf.Abs(count * 2 - 1));
                if(count > 0.5)
                {
                    transform.position = new Vector3(endPos.x, endPos.y, startZ);
                }
                fade.alpha = amount;
            }
            else
            {
                transform.position = Vector3.Lerp(new Vector3(startPos.x, startPos.y, startZ), new Vector3(endPos.x, endPos.y, startZ), amount);
            }
            count += Time.unscaledDeltaTime * transitionDuration;
            yield return null;
        }
        Time.timeScale = 1;
        startPos = endPos;
        transform.position = new Vector3(startPos.x, startPos.y, startZ);
        fade.alpha = 0;
        animating = false;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        DebugExtended.GizmosSaveColor();
        if (cam != null)
        {
            UpdateEndPos();
            const float repetition = 10f;
            for (float x = startPos.x - size.x * repetition; x <= startPos.x + size.x * repetition; x+= size.x)
            {
                for (float y = startPos.y - size.y * repetition; y <= startPos.y + size.y * repetition; y+= size.y)
                {
                    Gizmos.DrawWireCube(new Vector2(x, y), size);
                }
            }
        }
        Gizmos.color = Color.red * 0.5f;
        Gizmos.DrawWireSphere(startPos, 1);
        Gizmos.color = Color.green * 0.5f;
        Gizmos.DrawWireSphere(endPos, 1);
        DebugExtended.GizmosRestoreColor();
    }
#endif
}
