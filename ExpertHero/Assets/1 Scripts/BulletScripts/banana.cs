using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class banana : MonoBehaviour, IBullet
{
    public GameObject targetZom;
   public int damage { get; set; }
    public bool damageUpActive { get; set; }

    public void follow(GameObject targetZombie, GameObject weapon)
    {
        targetZom = targetZombie;
        gameObject.AddComponent<Rigidbody>();
        GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)) * 10000);

        StartCoroutine(targetFollow());
    }
    IEnumerator targetFollow()
    {
        while (targetZom != null)
        {
            if (Vector3.Distance(transform.position, new Vector3(targetZom.transform.position.x, transform.position.y, targetZom.transform.position.z)) > 1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetZom.transform.position.x, transform.position.y, targetZom.transform.position.z), 50 * Time.deltaTime);
                bulletFollowing(targetZom.transform.position);
                yield return null;
            }
            else
            {
                damageText(targetZom.transform.position + new Vector3(0, 10, 0));
                //var point = Instantiate(transform.GetChild(0).gameObject, targetZom.transform.position +new Vector3(0,10,0), Quaternion.identity);
                //point.SetActive(true);
                //point.AddComponent<Point>();
                //point.transform.localScale *= 6;
                //point.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = damage.ToString();

             
                targetZom.GetComponent<Zombie>().dead(damage);

                bulletForce();
                break;
            }
        }
        yield return new WaitForSeconds(0.15f);
        GetComponent<Rigidbody>().AddForce(new Vector3(0, -1, 0) * 1000);

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
        Vector3 forceDirection = (transform.position - targetZom.transform.position).normalized;
        GetComponent<Rigidbody>().AddForce(new Vector3(forceDirection.x * 5, 1, forceDirection.z * 5) * 500);
        GetComponent<Rigidbody>().AddTorque(new Vector3(forceDirection.z, 0, forceDirection.x) * 10000);
        gameObject.AddComponent<BoxCollider>();
        Destroy(gameObject, 1f);

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
