using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boosterIcon : MonoBehaviour
{
    // Start is called before the first frame update
    int aliveTime;
    public void time(int time)
    {
        aliveTime = time;
        StartCoroutine(panelScaleDown(transform.GetComponent<RectTransform>()));
        StartCoroutine(panelScaleUp(transform.GetComponent<RectTransform>()));
        Destroy(gameObject, time);
    }
    IEnumerator panelScaleUp(RectTransform image)
    {
        float counter = 0;
        while (counter < 1f)
        {
            counter += 5 * Time.deltaTime;
            image.localScale = new Vector3(counter, counter, counter);
            yield return null;
        }
        image.localScale = new Vector3(1, 1, 1);
        counter = 0f;
        float scale = 0;
        while (counter < Mathf.PI)
        {
            counter += 5 * Time.deltaTime;
            scale = Mathf.Sin(counter);
            scale *= 0.3f;
            image.localScale = new Vector3(1 + scale, 1 + scale, 1 + scale);
            yield return null;
        }
        image.localScale = new Vector3(1, 1, 1);
    }
    IEnumerator panelScaleDown(RectTransform image)
    {
        yield return new WaitForSeconds(aliveTime - 2);
        float counter = 0f;
        float scale = 0;
        while (counter < Mathf.PI * 2)
        {
            counter += 5 * Time.deltaTime;
            scale = Mathf.Abs(Mathf.Sin(counter));
            scale *= 0.3f;
            image.localScale = new Vector3(1 - scale, 1 - scale, 1 - scale);
            yield return null;
        }
        image.localScale = new Vector3(1, 1, 1);
        counter = 1f;

        while (counter > 0)
        {
            counter -= 1.5f * Time.deltaTime;
            image.localScale = new Vector3(counter, counter, counter);
            yield return null;
        }
        image.localScale = new Vector3(0, 0, 0);
    }
}
