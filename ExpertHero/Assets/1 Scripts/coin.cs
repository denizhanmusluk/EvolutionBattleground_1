using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour
{
    float motionSpeed = 20;
    Transform target;
    GameObject particle;
    void Start()
    {
        particle = transform.GetChild(0).gameObject;
        particle.SetActive(true);
        GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-2f, 2f), 0.5f, Random.Range(-2f, 2f)) * 200);
        GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), Random.Range(-2f, 2f)) * 1000);
        StartCoroutine(rotateCoin());
    }
    IEnumerator rotateCoin()
    {
        yield return new WaitForSeconds(0.3f);
        GetComponent<Rigidbody>().AddForce(-Vector3.up * 500);
        yield return new WaitForSeconds(0.3f);
        Destroy(GetComponent<Rigidbody>());

        transform.position = new Vector3(transform.position.x, 2, transform.position.z);
        yield return null;
        particle.SetActive(true);
        particle.transform.position -= new Vector3(0, 1, 0);
        particle.transform.parent = null;
        while (true)
        {
            transform.Rotate(50 * Time.deltaTime, 200 * Time.deltaTime, 50 * Time.deltaTime);
            yield return null;
        }
    }
    /* IEnumerator targetMotion()
     {

         GetComponent<SphereCollider>().enabled = false;
         GetComponent<BoxCollider>().enabled = false;
         float counter = 0f;
         float angle = 0f;
         Vector3 dirVect = (transform.position - target.position).normalized;
         GetComponent<Rigidbody>().AddForce((dirVect + new Vector3(0, 0.1f, 0)) * 1000);
         yield return new WaitForSeconds(0.5f);


         while (counter < 0.5f)
         {
             counter += Time.deltaTime;
             GetComponent<Rigidbody>().AddForce((target.position - transform.position).normalized * 20*  Vector3.Distance(transform.position, target.position) * 50 * Time.deltaTime);

             yield return null;
         }
         while (Vector3.Distance(transform.position, target.position) > 5f)
         {
             GetComponent<Rigidbody>().velocity = (target.position - transform.position).normalized * (100 + Vector3.Distance(transform.position, target.position));

             yield return null;
         }
         //while (counter < Mathf.PI)
         //{
         //    counter += 3 * Time.deltaTime;
         //    angle = 5 * Mathf.Cos(counter);

         //    transform.position = Vector3.MoveTowards(transform.position, transform.position + dirVect*angle + new Vector3(0,1,0), counter* motionSpeed * Time.deltaTime);
         //    yield return null;
         //}
         //while (Vector3.Distance(transform.position, target.position) > 1f)
         //{
         //    transform.position = Vector3.MoveTowards(transform.position, target.position, (70 / Vector3.Distance(transform.position, target.position)) * motionSpeed * Time.deltaTime);
         //    yield return null;
         //}
         target.GetComponent<PlayerParent>().playerYearSet(1);
         GameObject money = gameObject;
         money.transform.parent = null;
         Destroy(money);
     }
    */
    IEnumerator targetMotion()
    {
        particle.SetActive(false);

        GetComponent<SphereCollider>().enabled = false;
        float counter = 0f;
        float angle = 0f;
        Vector3 dirVect = (transform.position - target.position).normalized;



        while (counter < Mathf.PI/2)
        {
            counter += 3 * Time.deltaTime;
            angle = 2 * Mathf.Cos(counter);

            transform.position = Vector3.MoveTowards(transform.position, transform.position + dirVect * angle + new Vector3(0, 0.5f, 0), counter * motionSpeed * Time.deltaTime);
            yield return null;
        }

        while (Vector3.Distance(transform.position, target.position) > 1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, (40 / Vector3.Distance(transform.position, target.position)) * motionSpeed * Time.deltaTime);
            yield return null;
        }
        Destroy(particle);

        target.GetComponent<PlayerParent>().playerYearSet(2);
        GameObject money = gameObject;
        money.transform.parent = null;
        Destroy(money);
    }
    public void collect(Transform moneyTarget)
    {
        target = moneyTarget;
        StartCoroutine(targetMotion());
    }
}
