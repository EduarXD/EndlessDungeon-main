using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class RandomLight : MonoBehaviour
{
    public Light2D Light;
    public float speed;
    public Vector2 rangeIntensity;
    public Gradient gradient;
    public Vector2 changeRate;
    float targetIntensity;
    Color targetColor;
    float nextChange;

    void Update()
    {
        if (nextChange < Time.time)
        {
            nextChange = Time.time + Random.Range(changeRate.x, changeRate.y);
            targetIntensity = Random.Range(rangeIntensity.x, rangeIntensity.y);
            targetColor = gradient.Evaluate(Random.Range(0f, 1f));
        }
        Light.color = Color.Lerp(Light.color, targetColor, speed * Time.deltaTime);
        Light.intensity = Mathf.Lerp(Light.intensity, targetIntensity, speed * Time.deltaTime);
    }
}
