using System.Collections;
using UnityEngine;

public class Knockout : MonoBehaviour
{
    [Header("Drag the eyelids here")]
    public RectTransform topLid;
    public RectTransform bottomLid;

    public float closeTime = 0.08f;   // how fast the lids shut
    public float holdTime = 0.15f;    // how long they stay shut
    public float openTime = 0.3f;     // how fast they reopen

    float topX;
    float bottomX;
    bool knockedOut = false;

    void Awake()
    {
        // remember the horizontal placement (should be 0), then start open
        topX = topLid.anchoredPosition.x;
        bottomX = bottomLid.anchoredPosition.x;
        SetOpen(1f);
    }

    // call this when the player takes damage
    public void Blink()
    {
        if (!knockedOut)
            StartCoroutine(BlinkCo());
    }

    // lids close and stay closed until WakeUp is called
    public void PassOut()
    {
        StopAllCoroutines();
        knockedOut = true;
        StartCoroutine(SlideTo(0f, closeTime));
    }

    public void WakeUp()
    {
        if (knockedOut)
        {
            knockedOut = false;
            StartCoroutine(SlideTo(1f, openTime));
        }
    }

    IEnumerator BlinkCo()
    {
        yield return SlideTo(0f, closeTime);
        yield return new WaitForSeconds(holdTime);
        yield return SlideTo(1f, openTime);
    }

    IEnumerator SlideTo(float targetOpen, float time)
    {
        float startOpen = CurrentOpenness();
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / time;
            SetOpen(Mathf.Lerp(startOpen, targetOpen, Mathf.Clamp01(t)));
            yield return null;
        }
        SetOpen(targetOpen);
    }

    // 1 = fully open (lids offscreen), 0 = fully shut (screen covered)
    void SetOpen(float openness)
    {
        topLid.anchoredPosition = new Vector2(topX, topLid.rect.height * openness);
        bottomLid.anchoredPosition = new Vector2(bottomX, -bottomLid.rect.height * openness);
    }

    float CurrentOpenness()
    {
        return Mathf.Clamp01(topLid.anchoredPosition.y / topLid.rect.height);
    }
}
