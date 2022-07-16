using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Rocket : MonoBehaviour,IBullet
{
    public GameObject targetZom;
    public GameObject boomParticle;
    public GameObject trailParticle;
    public int damage { get; set; }
    public bool damageUpActive { get; set; }

    public void follow(GameObject targetZombie, GameObject weapon)
    {
        targetZom = targetZombie;
        gameObject.AddComponent<Rigidbody>();
        //GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)) * 10000);
        transform.rotation = Quaternion.Euler(-60, transform.eulerAngles.y, transform.eulerAngles.z);
        transform.localScale = new Vector3(4, 4, 4);
        StartCoroutine(targetFollow());
        gameObject.AddComponent<BoxCollider>();
        trailParticle.SetActive(true);
    }
    IEnumerator targetFollow()
    {
        while (targetZom != null)
        {
            if (Vector3.Distance(transform.position, new Vector3(targetZom.transform.position.x, transform.position.y, targetZom.transform.position.z)) > 5f)
            {
                //transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetZom.transform.position.x, transform.position.y, targetZom.transform.position.z), 50 * Time.deltaTime);
                transform.GetComponent<Rigidbody>().velocity = transform.forward * 30;
                bulletFollowing(targetZom.transform.position);
                yield return null;
            }
            else
            {
                //var point = Instantiate(transform.GetChild(0).gameObject, targetZom.transform.position, Quaternion.identity);
                //point.SetActive(true);
                //point.AddComponent<Point>();

                //targetZom.GetComponent<Zombie>().anim.SetTrigger("fall");
                //targetZom.GetComponent<Zombie>().currentBehaviour = Zombie.States.dead;
                //targetZom.GetComponent<Zombie>().gameObject.layer = LayerMask.GetMask("Default");
                //targetZom.GetComponent<Zombie>().dead();

                //bulletForce();
                break;
            }
        }
        //Destroy(gameObject);
    }
    private void bulletFollowing(Vector3 target)
    {

        Vector3 relativeVector = transform.InverseTransformPoint(target);
        relativeVector /= relativeVector.magnitude;
        float newSteer = (relativeVector.x / relativeVector.magnitude);
        transform.Rotate(0, newSteer * Time.deltaTime * 200, 0);
    }
    void bulletForce()
    {
        //Vector3 forceDirection = (transform.position - targetZom.transform.position).normalized;
        //GetComponent<Rigidbody>().AddForce(new Vector3(forceDirection.x * 5, 1, forceDirection.z * 5) * 500);
        //GetComponent<Rigidbody>().AddTorque(new Vector3(forceDirection.z, 0, forceDirection.x) * 10000);
        //gameObject.AddComponent<BoxCollider>();
        //Destroy(gameObject, 5f);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "ground" || collision.transform.tag == "zombie")
        {
            trailParticle.SetActive(false);

            var boom = Instantiate(boomParticle, transform.position, Quaternion.Euler(90, 0, 0));
            boom.SetActive(true);
            boom.transform.localScale = new Vector3(3, 3, 3);
            GetComponent<SphereCollider>().enabled = true;
            GetComponent<BoxCollider>().enabled = false;
            Destroy(gameObject, 0.1f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "zombie")
        {
            //other.transform.GetComponent<Zombie>().agent.enabled = false;
            //other.transform.GetComponent<Zombie>().anim.SetTrigger("fall");
            //other.transform.GetComponent<Zombie>().currentBehaviour = Zombie.States.dead;
            //other.transform.GetComponent<Zombie>().gameObject.layer = LayerMask.GetMask("Default");
            //other.transform.GetComponent<Zombie>().dead();
            Vector3 forceDirection= (other.transform.position - transform.position).normalized;

            if (!other.GetComponent<Zombie>().deadActive)
            {
                other.GetComponent<Zombie>().deadRagdoll(damage,true, forceDirection);
                bulletForce();
                damageText(other.transform.position + new Vector3(0, 10, 0));
            }
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
