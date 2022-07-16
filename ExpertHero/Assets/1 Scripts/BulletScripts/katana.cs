using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class katana : MonoBehaviour,IBullet
{
    public GameObject targetZom;
    public int damage { get; set; }
    public bool damageUpActive { get; set; }

    public void follow(GameObject targetZombie, GameObject weapon)
    {
        transform.localScale *= 1.5f;

        targetZom = targetZombie;
        gameObject.AddComponent<Rigidbody>();
        //GetComponent<Rigidbody>().AddTorque(transform.right * 50000);

        StartCoroutine(targetFollow());
    }
    IEnumerator targetFollow()
    {
        //yield return new WaitForSeconds(0.1f);
        //transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        Vector2 currentDir = new Vector3(targetZom.transform.position.x - transform.position.x, targetZom.transform.position.z - transform.position.z);
        currentDir.Normalize();


        Vector3 direction = new Vector3(currentDir.x, 0f, currentDir.y);

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, targetAngle, 0);

        while (targetZom != null)
        {
            Vector3 dir = (targetZom.transform.position + new Vector3(0, 1, 0) - transform.position).normalized;
            if (Vector3.Distance(transform.position, targetZom.transform.position + new Vector3(0, 1, 0)) > 50f)
            {
                Debug.Log("katana");
                transform.GetComponent<Rigidbody>().velocity = transform.forward * 20;
                //bulletFollowing(targetZom.transform.position + new Vector3(0, 1, 0));
                yield return null;
            }
            else
            {
                //var point = Instantiate(transform.GetChild(0).gameObject, targetZom.transform.position, Quaternion.identity);
                //point.SetActive(true);
                //point.AddComponent<Point>();
                if (Vector3.Distance(transform.position, targetZom.transform.position + new Vector3(0, 1, 0)) > 1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetZom.transform.position + new Vector3(0, 1, 0), 50 * Time.deltaTime);
                    //transform.LookAt(targetZom.transform.position );

                
                    transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(90, transform.localEulerAngles.y, transform.localEulerAngles.z) , 1000 * Time.deltaTime);
                    yield return null;
                }
                else
                {
                    //targetZom.GetComponent<Zombie>().anim.SetTrigger("fall");
                    if (!targetZom.GetComponent<Zombie>().deadActive)
                    {
                        damageText(targetZom.transform.position + new Vector3(0, 10, 0));

                        targetZom.GetComponent<Zombie>().deadRagdoll(damage,true, new Vector3(direction.x, -3f, direction.z));
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
        //transform.Rotate(0, newSteer * Time.deltaTime * 500, 0);
        transform.Rotate(new Vector3(-transform.right.x * Time.deltaTime * 1000, newSteer * Time.deltaTime * 500, -transform.right.z * Time.deltaTime * 1000));
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
