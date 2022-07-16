using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class crossbow : MonoBehaviour,IBullet
{
    public GameObject targetZom;
    public int damage { get; set; }
    public bool damageUpActive { get; set; }

    public void follow(GameObject targetZombie, GameObject weapon)
    {
        transform.localScale *= 6;
        targetZom = targetZombie;
        gameObject.AddComponent<Rigidbody>();
        GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)) * 10000);

        StartCoroutine(targetFollow());
    }
    IEnumerator targetFollow()
    {
        while (targetZom != null)
        {
            if (Vector3.Distance(transform.position, targetZom.transform.position + new Vector3(0, 1, 0)) > 1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetZom.transform.position + new Vector3(0, 1, 0), 80 * Time.deltaTime);
                transform.LookAt(targetZom.transform.position + new Vector3(0, 1, 0));
                yield return null;
            }
            else
            {
                damageText(targetZom.transform.position + new Vector3(0, 10, 0));

                //var point = Instantiate(transform.GetChild(0).gameObject, targetZom.transform.position, Quaternion.identity);
                //point.SetActive(true);
                //point.AddComponent<Point>();

              
                targetZom.GetComponent<Zombie>().dead(damage);

                bulletForce();
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
