using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class laser2 : MonoBehaviour,IBullet
{
    public GameObject targetZom;
    public int damage { get; set; }
    public bool damageUpActive { get; set; }

    [SerializeField] GameObject lightTrailParticle;
    [SerializeField] ParticleSystem eyeSmokeParticle;
    public void follow(GameObject targetZombie, GameObject weapon)
    {
        eyeSmokeParticle.Play();
        transform.localScale = new Vector3(4.5f, 4.5f, 4.5f);
        targetZom = targetZombie;
        gameObject.AddComponent<Rigidbody>();
        //GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)) * 10000);
        //gameObject.AddComponent<BoxCollider>();
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<BoxCollider>().isTrigger = true;
        lightTrailParticle.SetActive(true);
        StartCoroutine(targetFollow());
    }
    IEnumerator targetFollow()
    {
        Vector3 direction = ((targetZom.transform.position + new Vector3(0, 6f, 0)) - transform.position).normalized;
        GetComponent<Rigidbody>().AddForce((direction) * 5000);
        transform.LookAt(targetZom.transform.position + new Vector3(0, 4f, 0));
        yield return new WaitForSeconds(0.6f);
        GetComponent<Rigidbody>().drag = 3;
        yield return new WaitForSeconds(0.2f);
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
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<Zombie>() != null)
        {
            Vector3 forceDirection = (other.transform.position - transform.position).normalized;

            //other.GetComponent<Zombie>().deadRagdoll(damage, true, forceDirection);
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
