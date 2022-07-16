using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class swipeHand : MonoBehaviour
{
    RectTransform rect;
    void Start()
    {
        rect = GetComponent<RectTransform>();
        //transform.DOMoveX(Screen.width/3f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        StartCoroutine(swipeMove());
    }
    IEnumerator swipeMove()
    {
        float counter = 0f;
        float value = 0;
        while (true)
        {
            counter += 5 * Time.deltaTime;
            value = Mathf.Cos(counter);
            value *= 120;
            rect.localPosition = new Vector3(value, 0, 0);

            yield return null;
        }
    }
}
