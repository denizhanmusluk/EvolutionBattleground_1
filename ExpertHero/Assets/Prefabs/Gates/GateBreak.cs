using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateBreak : MonoBehaviour
{
    GameObject glass, glassBroken;
    bool aliveActive = true;
    [SerializeField] GameObject yearUpParticle;
    private void Start()
    {
        glass = transform.GetChild(0).gameObject;
        glassBroken = transform.GetChild(1).gameObject;
        StartCoroutine(selfDestroy());
    }
    public void glassHit(GameObject man)
    {
      GameObject yearUp=  Instantiate(yearUpParticle, transform.position + new Vector3(0,1,0), Quaternion.Euler(-90,0,0));
        yearUp.transform.localScale = new Vector3(7, 7, 7);
        glass.SetActive(false);
        glassBroken.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetComponent<BoxCollider>().enabled = false;

        for (int i = 0; i < glassBroken.transform.childCount; i++)
        {
            //Vector3 obsPos = new Vector3(-1 / glassBroken.transform.GetChild(i).transform.localPosition.y, 1, -1 / (glassBroken.transform.GetChild(i).transform.localPosition.y));
            Vector3 forceDirection = (glassBroken.transform.GetChild(i).transform.position - man.transform.position).normalized + Vector3.up;
            glassBroken.transform.GetChild(i).transform.GetComponent<Rigidbody>().useGravity = true;
            glassBroken.transform.GetChild(i).transform.GetComponent<Rigidbody>().AddForce(forceDirection * 500);
            glassBroken.transform.GetChild(i).transform.GetComponent<Rigidbody>().AddTorque(forceDirection * 500);
        }
        StartCoroutine(down());
        Destroy(glass, 4);
        Destroy(glassBroken, 4);
    }
    IEnumerator down()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < glassBroken.transform.childCount; i++)
        {
            //Vector3 obsPos = new Vector3(-1 / glassBroken.transform.GetChild(i).transform.localPosition.y, 1, -1 / (glassBroken.transform.GetChild(i).transform.localPosition.y));
            glassBroken.transform.GetChild(i).transform.GetComponent<Rigidbody>().AddForce(-transform.up * 200);
        }
        yield return new WaitForSeconds(0.5f);


        float counter = 0f;
        float value = 0;
        while (counter < Mathf.PI)
        {
            counter += 5 * Time.deltaTime;
            value = Mathf.Abs(Mathf.Sin(counter));
            value /= 3f;
            transform.localScale = new Vector3(transform.localScale.x, 1.1f + value, transform.localScale.z);

            yield return null;
        }
        transform.localScale = new Vector3(transform.localScale.x, 1.1f, transform.localScale.z);
        counter = 0;
        value = 0f;
        while(counter < Mathf.PI/2)
        {
            counter += 5 * Time.deltaTime;
            value = 1.1f * Mathf.Sin(counter);
            transform.localScale = new Vector3(transform.localScale.x, 1.1f - value, transform.localScale.z);
            yield return null;
        }
        transform.localScale = new Vector3(transform.localScale.x, 0, transform.localScale.z);
        Destroy(gameObject);
    }
    IEnumerator selfDestroy()
    {
        yield return new WaitForSeconds(15f);
        while (aliveActive)
        {
            if ((Camera.main.WorldToScreenPoint(transform.position).x < Screen.width  && Camera.main.WorldToScreenPoint(transform.position).x > 0) && (Camera.main.WorldToScreenPoint(transform.position).y < Screen.height && Camera.main.WorldToScreenPoint(transform.position).y > 0))
            {
            }
            else
            {
                aliveActive = false;
            }
            yield return null;

        }
        float counter = 0;
        float value = 0f;
        while (counter < Mathf.PI / 2)
        {
            counter += 5 * Time.deltaTime;
            value = 1.1f * Mathf.Sin(counter);
            transform.localScale = new Vector3(transform.localScale.x, 1.1f - value, transform.localScale.z);
            yield return null;
        }
        transform.localScale = new Vector3(transform.localScale.x, 0, transform.localScale.z);
        GateSpawner.Instance.gateAll.Remove(gameObject);
        Destroy(gameObject);
    }
}
