using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class mizrak : MonoBehaviour,IBullet
{
    public GameObject targetZom;
    public int damage { get; set; }
   public bool damageUpActive { get; set; }
    public void follow(GameObject targetZombie, GameObject weapon)
    {
        targetZom = targetZombie;
        gameObject.AddComponent<Rigidbody>();
        //GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)) * 10000);
        transform.localScale = new Vector3(4,4,4);

        StartCoroutine(targetFollow());
    }
    IEnumerator targetFollow()
    {
        Vector2 currentDir = new Vector3(targetZom.transform.position.x - transform.position.x, targetZom.transform.position.z - transform.position.z);
        currentDir.Normalize();


        Vector3 direction = new Vector3(currentDir.x, 0f, currentDir.y);
        while (targetZom != null)
        {
            Vector3 dir = (new Vector3(targetZom.transform.position.x, transform.position.y, targetZom.transform.position.z) - transform.position).normalized;
            if (Vector3.Distance(transform.position, new Vector3(targetZom.transform.position.x, transform.position.y, targetZom.transform.position.z)) > 50f)
            {
                transform.GetComponent<Rigidbody>().velocity = transform.forward * 30;
                bulletFollowing(targetZom.transform.position + new Vector3(0, 1, 0));
                yield return null;
            }
            else
            {

                //var point = Instantiate(transform.GetChild(0).gameObject, targetZom.transform.position, Quaternion.identity);
                //point.SetActive(true);
                //point.AddComponent<Point>();
                if (Vector3.Distance(transform.position, new Vector3(targetZom.transform.position.x, transform.position.y, targetZom.transform.position.z)) > 1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetZom.transform.position.x, transform.position.y, targetZom.transform.position.z), 50 * Time.deltaTime);
                    bulletFollowing(targetZom.transform.position + new Vector3(0, 1, 0));
                    yield return null;
                }
                else
                {
                    //targetZom.GetComponent<Zombie>().anim.SetTrigger("fall");
                    if (!targetZom.GetComponent<Zombie>().deadActive)
                    {
                        damageText(targetZom.transform.position + new Vector3(0, 10, 0));
                        targetZom.GetComponent<Zombie>().deadRagdoll(damage,true, direction);
                        bulletForce();
                    }
                    break;
                }
            }
        }
        //Destroy(gameObject);
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
        transform.parent = targetZom.transform.GetChild(1).GetChild(0);
        Destroy(GetComponent<Rigidbody>());
        transform.localPosition = new Vector3(0, 0.5f, 0);
        //Vector3 forceDirection = (transform.position - targetZom.transform.position).normalized;
        //GetComponent<Rigidbody>().AddForce(new Vector3(forceDirection.x * 5, 1, forceDirection.z * 5) * 500);
        //GetComponent<Rigidbody>().AddTorque(new Vector3(forceDirection.z, 0, forceDirection.x) * 10000);
        //gameObject.AddComponent<BoxCollider>();
        //Destroy(gameObject, 5f);

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
