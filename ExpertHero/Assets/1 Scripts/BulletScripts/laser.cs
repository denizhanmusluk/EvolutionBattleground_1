using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class laser : MonoBehaviour,IBullet
{
    public GameObject targetZom;
    public int damage { get; set; }
    public bool damageUpActive { get; set; }

    public void follow(GameObject targetZombie, GameObject weapon)
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().detectCollisions = true;
        targetZom = targetZombie;
        //GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)) * 10000);
        StartCoroutine(targetFollow());
    }
    IEnumerator targetFollow()
    {
        yield return new WaitForSeconds(0.2f);

        GetComponent<BoxCollider>().enabled = true;

      
            Vector2 direction = new Vector3(targetZom.transform.position.x - transform.position.x, targetZom.transform.position.z - transform.position.z);
            direction.Normalize();


            Vector3 dir = new Vector3(direction.x, 0f, direction.y);

            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);

            yield return new WaitForSeconds(0.1f);
        GetComponent<BoxCollider>().enabled = false;
        //transform.rotation = Quaternion.Euler(0, 0, 0);

        //Vector3 direction = (targetZom.transform.position - transform.position).normalized;


        //GetComponent<Rigidbody>().AddForce(direction * 4000);
        //transform.LookAt(targetZom.transform.position + new Vector3(0, 1, 0));
        //yield return new WaitForSeconds(0.3f);
        //GetComponent<Rigidbody>().drag = 10;
        //yield return new WaitForSeconds(1f);
        //Destroy(gameObject);



        //float counter = 0f;
        //while (counter < 1f)
        //{
        //    //(targetZom.transform.position - transform.position).normalized

        //    counter += Time.deltaTime;
        //    RaycastHit hit;
        //    if (Physics.Raycast(transform.position, transform.TransformDirection(new Vector3(  direction.x , transform.position.y, direction.z )), out hit, 30f))    //right
        //    {
        //        Debug.Log(hit.transform.name + "          " + direction);
        //        if (hit.transform.tag == "zombie")
        //        {
        //            Debug.Log(hit.point.y);
        //            hit.transform.GetComponent<Zombie>().anim.SetTrigger("fall");
        //            hit.transform.GetComponent<Zombie>().currentBehaviour = Zombie.States.dead;
        //            hit.transform.GetComponent<Zombie>().gameObject.layer = 2;
        //            hit.transform.GetComponent<Zombie>().dead();
        //            damageText(hit.transform.position + new Vector3(0, 10, 0));

        //        }

        //    }
        //    yield return null;
        //}



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

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Zombie>() != null)
        {
       
            other.GetComponent<Zombie>().dead(damage);
            damageText(other.transform.position + new Vector3(0, 10, 0));
        
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
