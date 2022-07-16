using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] public int damage;
    [SerializeField] FieldOfView view;
    public enum States { attack, none }
    public States currentBehaviour;
    public GameObject targetZombie;
    public GameObject bulletPrefab;
    public bool gun;
    [SerializeField] Animator anim;
    [SerializeField] public float attackSpeed;
    public bool damageUpActive;
    void Start()
    {
        bulletPrefab = transform.GetChild(0).gameObject;
        //bulletPrefab.SetActive(true);
        gun = true;
    }

    void Update()
    {
        switch (currentBehaviour)
        {
            case States.attack:
                {
                    _attack();
                }
                break;

            case States.none:
                {
                    anim.SetBool("shoot", false);
                }
                break;

        }
    }
    void _attack()
    {
        if (gun && targetZombie != null)
        {
            if (targetZombie.GetComponent<Zombie>().deadActive)
            {
                targetZombie = null;
            }
            else
            {
                StartCoroutine(attack());
            }
        }
    }
    IEnumerator attack()
    {
        anim.SetTrigger("shot");

        targetZombie.GetComponent<Zombie>().isSelected = true;
        targetZombie.GetComponent<Zombie>().thisSelected();
        //anim.speed = attackSpeed;
        anim.SetFloat("attackSpeed", attackSpeed);
        gun = false;
        //if (transform.GetChild(0).name != "laser")
        //{
        yield return new WaitForSeconds(0.15f/ attackSpeed);
            if(transform.GetChild(0).GetComponent<shootgun>() != null || transform.GetChild(0).GetComponent<Rocket>() != null)
            {
                transform.parent.GetChild(1).GetComponent<ParticleSystem>().Play();
            }
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject bullet;
            bullet = Instantiate(transform.GetChild(i).gameObject, bulletPrefab.transform.position, bulletPrefab.transform.rotation);
            bullet.GetComponent<IBullet>().damage = damage;
            bullet.GetComponent<IBullet>().damageUpActive = damageUpActive;
            if (view != null)
            {
                bullet.SetActive(true);
                //bullet.transform.LookAt(targetZombie.transform);
                bulletPrefab.SetActive(false);

                bullet.GetComponent<IBullet>().follow(targetZombie, view.transform.parent.parent.gameObject);
            }
            if (bullet.GetComponent<TrailRenderer>() != null)
            {
                bullet.GetComponent<TrailRenderer>().enabled = true;
            }
        }
            bulletPrefab.SetActive(false);
        //targetZombie.layer = LayerMask.GetMask("Default");
        //}
        //else
        //{
        //    transform.GetChild(0).GetComponent<IBullet>().follow(targetZombie, view.transform.parent.parent.gameObject);
        //}
        //targetZombie.GetComponent<Zombie>().isSelected = false;
        targetZombie = null;
        yield return new WaitForSeconds(1f/attackSpeed);
        //anim.SetBool("shoot", false);
        //anim.speed = 1;
        gun = true;
        view.visibleTargets.Clear();
    }
    public void shotEnd()
    {
        bulletPrefab.SetActive(true);

    }
    private void bulletFollowing(Vector3 target, GameObject bullet)
    {

        Vector3 relativeVector = bullet.transform.InverseTransformPoint(target);
        relativeVector /= relativeVector.magnitude;
        float newSteer = (relativeVector.x / relativeVector.magnitude);
        bullet.transform.Rotate(0, newSteer * Time.deltaTime * 500 , 0);
    }
}
