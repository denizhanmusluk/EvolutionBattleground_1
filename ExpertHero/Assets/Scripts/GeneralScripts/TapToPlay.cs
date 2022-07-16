using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TapToPlay : MonoBehaviour
{
    void Start()
    {
        //transform.DOScale(Vector3.one * 1.1f, 0.3f).SetLoops(-1, LoopType.Yoyo);

        StartCoroutine(swipeMove());
    }
    IEnumerator swipeMove()
    {
        float counter = 0f;
        float value = 0;
        while (true)
        {
            counter += 5 * Time.deltaTime;
            value = Mathf.Abs(Mathf.Sin(counter));
            value *= 0.15f;
            transform.localScale = new Vector3(1 + value, 1 + value, 1 + value);

            yield return null;
        }
    }
}