using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class shootgun : MonoBehaviour, IBullet
{
    public GameObject targetZom;
    public int damage { get; set; }
    public bool damageUpActive { get; set; }
    Vector3 _weapon;
    public void follow(GameObject targetZombie, GameObject weapon)
    {
        _weapon = weapon.transform.position;
        transform.localScale *= 10;
        targetZom = targetZombie;
        gameObject.AddComponent<Rigidbody>();
        //GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)) * 10000);
        gameObject.AddComponent<BoxCollider>();
        StartCoroutine(targetFollow());
    }
    IEnumerator targetFollow()
    {
        Vector3 direction = ((targetZom.transform.position + new Vector3(0, 0.5f, 0)) - transform.position).normalized;
        GetComponent<Rigidbody>().AddForce((direction +new Vector3(Random.Range(-0.4f,0.4f),0, Random.Range(-0.4f, 0.4f))) * 8000);
        transform.LookAt(targetZom.transform.position + new Vector3(0, 0.5f, 0));
        yield return new WaitForSeconds(0.15f);
        GetComponent<Rigidbody>().drag = 15;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        //while (targetZom != null)
        //{
        //    if (Vector3.Distance(transform.position, new Vector3(targetZom.transform.position.x, transform.position.y, targetZom.transform.position.z)) > 1f)
        //    {
        //        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetZom.transform.position.x, transform.position.y, targetZom.transform.position.z), 50 * Time.deltaTime);
        //        bulletFollowing(targetZom.transform.position);
        //        yield return null;
        //    }
        //    else
        //    {
        //        //var point = Instantiate(transform.GetChild(0).gameObject, targetZom.transform.position, Quaternion.identity);
        //        //point.SetActive(true);
        //        //point.AddComponent<Point>();

        //        targetZom.GetComponent<Zombie>().anim.SetTrigger("fall");
        //        targetZom.GetComponent<Zombie>().currentBehaviour = Zombie.States.dead;
        //        targetZom.GetComponent<Zombie>().gameObject.layer = LayerMask.GetMask("Default");
        //        targetZom.GetComponent<Zombie>().dead();

        //        bulletForce();
        //        break;
        //    }
        //}
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<Zombie>() != null)
        {
            Vector3 forceDirection = (collision.transform.position - _weapon).normalized;

            collision.transform.GetComponent<Zombie>().deadRagdoll(damage, true, forceDirection*2);

            damageText(collision.transform.position + new Vector3(0, 10, 0));
            collision.transform.GetComponent<Zombie>().shotgunForce(forceDirection);
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
