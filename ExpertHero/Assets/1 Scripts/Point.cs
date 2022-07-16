using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Point : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(pointUp());
    }
    public void _Start()
    {
        StartCoroutine(colorSet());
    }

    IEnumerator pointUp()
    {

        float counter = 0;
        float speed = 0;
        while (counter < Mathf.PI / 2)
        {
            counter += 1.5f * Time.deltaTime;
            speed = Mathf.Cos(counter);
            speed *= 10;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 1, 0), Time.deltaTime * speed);

            yield return null;
        }
        Destroy(gameObject);
    }
    IEnumerator colorSet()
    {

        float counter = 0;
        float speed = 0;
        while (counter < Mathf.PI / 2)
        {
            counter += 1.5f * Time.deltaTime;
            speed = Mathf.Cos(counter);
            speed *= 10;
            transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1 - (counter / (Mathf.PI / 2)));

            yield return null;
        }
        Destroy(gameObject);
    }
}
