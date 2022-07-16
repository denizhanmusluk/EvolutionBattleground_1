using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Boomerang : MonoBehaviour,IBullet
{
    public GameObject targetZom;
    public GameObject playerParent;
    bool hitActive = false;
    public int damage { get; set; }
    public bool damageUpActive { get; set; }

    int totalHitCount = 3;
    int currentHitCount = 0;
    public void follow(GameObject targetZombie,GameObject parent)
    {
        transform.localScale *= 1.5f;

        playerParent = parent;
        targetZom = targetZombie;
        gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        gameObject.AddComponent<BoxCollider>();
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
        GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)) * 10000);

        StartCoroutine(targetFollow());
        StartCoroutine(rotate());
    }
    IEnumerator rotate()
    {
        while (hitActive)
        {
            transform.GetChild(1).Rotate(1250 * Time.deltaTime, 0, 0);
            yield return null;
        }
    }
    IEnumerator targetFollow()
    {
        hitActive = true;
        Vector3 direction = ((targetZom.transform.position + new Vector3(0,1,0) )- playerParent.transform.position).normalized;
        GetComponent<Rigidbody>().AddForce(direction  * 1500);
        yield return new WaitForSeconds(0.4f);
        float counter = 0f;
        if (playerParent != null)
        {
            while (Vector3.Distance(transform.position, (playerParent.transform.position+ new Vector3(0, 1, 0))) > 5f && counter<3f)
            {
                counter += Time.deltaTime;
                GetComponent<Rigidbody>().AddForce(((playerParent.transform.position + new Vector3(0, 1, 0)) - transform.position).normalized * 500 * Time.deltaTime * (500 /( Vector3.Distance(transform.position, (playerParent.transform.position + new Vector3(0, 1, 0))))));
                Debug.Log("following");
                yield return null;
                if (playerParent == null)
                {
                    break;
                }
            }
            if (playerParent != null)
            {

                while (Vector3.Distance(transform.position, (playerParent.transform.position + new Vector3(0, 1, 0))) > 5f)
                {
                    if (playerParent == null)
                    {
                        playerParent = gameObject;
                        GetComponent<Rigidbody>().AddForce(transform.position + new Vector3(0, 1, 0) * 1500);
                        Destroy(gameObject, 2);
                        Debug.Log("player null");
                        break;
                    }
                    transform.position = Vector3.MoveTowards(transform.position, (playerParent.transform.position + new Vector3(0, 1, 0)), 50 * Time.deltaTime);
                    yield return null;
                    if (playerParent == null)
                    {
                        playerParent = gameObject;
                        GetComponent<Rigidbody>().AddForce(transform.position + new Vector3(0, 1, 0) * 1500);
                        Destroy(gameObject, 2);
                        Debug.Log("player null");
                        break;
                    }
                }
            }
            else
            {
                GetComponent<Rigidbody>().AddForce(transform.position + new Vector3(0, 1, 0) * 1500);
                Destroy(gameObject, 2);
                Debug.Log("player null");
            }
        }
        hitActive = false;


        //var point = Instantiate(transform.GetChild(0).gameObject, targetZom.transform.position, Quaternion.identity);
        //point.SetActive(true);
        //point.AddComponent<Point>();

        //targetZom.GetComponent<Zombie>().anim.SetTrigger("fall");
        //targetZom.GetComponent<Zombie>().currentBehaviour = Zombie.States.dead;
        //targetZom.GetComponent<Zombie>().gameObject.layer = LayerMask.GetMask("Default");
        //targetZom.GetComponent<Zombie>().dead();
        if (playerParent != null)
        {
            Destroy(gameObject);
        }
    }
    private void bulletFollowing(Vector3 target)
    {

        Vector3 relativeVector = transform.InverseTransformPoint(target);
        relativeVector /= relativeVector.magnitude;
        float newSteer = (relativeVector.x / relativeVector.magnitude);
        transform.Rotate(0, newSteer * Time.deltaTime * 500, 0);
    }
    void bulletForce()
    {
        Vector3 forceDirection = (transform.position - targetZom.transform.position).normalized;
        GetComponent<Rigidbody>().AddForce(new Vector3(forceDirection.x * 5, 1, forceDirection.z * 5) * 500);
        GetComponent<Rigidbody>().AddTorque(new Vector3(forceDirection.z, 0, forceDirection.x) * 10000);
        gameObject.AddComponent<BoxCollider>();
        Destroy(gameObject, 5f);

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Zombie>() != null && hitActive)
        {
    
            other.GetComponent<Zombie>().dead(damage);
            damageText(other.transform.position + new Vector3(0, 10, 0));
            //currentHitCount++;
            //if (currentHitCount >= totalHitCount)
            //{
            //    GetComponent<Collider>().enabled = false;
            //}
        }
    }
    void damageText(Vector3 pos)
    {
        var point = Instantiate(transform.GetChild(0).gameObject, pos, Quaternion.identity);
        point.SetActive(true);


        point.AddComponent<Point>();
        point.transform.localScale = Vector3.one * 3;
        point.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = damage.ToString();
        if (damageUpActive)
        {
            point.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.red;
            point.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text += "!";
        }
        else
        {
            point.GetComponent<Point>()._Start();
        }
    }
}
