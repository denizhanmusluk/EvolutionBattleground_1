using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Cinemachine;
public class PlayerController : MonoBehaviour
{
    [SerializeField] public float damp;
    public bool gameActive;
    public bool speedUp;
    bool _hit = true;
    Vector3 stickDirection;
    Vector3 stickSpeed;

    Animator animator;
    Rigidbody playerRigidbody;
    [HideInInspector] public AudioSource audioSource;
    [HideInInspector] public float walkSpeed = 1;
    [Range(1, 20)] [SerializeField] public float rotationSpeed;
    [Range(1, 50)] [SerializeField] public float moveSpeed;

    int heart = 2;
    //[SerializeField]
    //CinemachineVirtualCamera cam1;

    public int playerHealth = 0;
    [SerializeField] GameObject health;
    [SerializeField] GameObject weaponPos;
    [SerializeField] public FieldOfView eye;

    public GameObject finish;
    private void Awake()
    {
        animator = GetComponent<Animator>();

    }
    void Start()
    {
        Globals.isGameFinished = false;
        Globals.playerControlActive = true;
        speedUp = true;
        audioSource = GetComponent<AudioSource>();
        playerRigidbody = GetComponent<Rigidbody>();
        walkSpeed = 1;
    }
    private void OnCollisionEnter(Collision collision)
    {
        //if(collision.transform.tag == "zombie")
        //{
        //    heart--;

        //    if (heart == 0)
        //    {

        //    }
        //    StartCoroutine(healthRegen());
        //}
        //if (collision.transform.tag == "pistol")
        //{
        //    if (weaponPos.transform.childCount == 0)
        //    {
               
        //        var gun = Instantiate(collision.gameObject, weaponPos.transform.position, Quaternion.identity);
        //        gun.transform.parent = weaponPos.transform;
        //        gun.transform.localRotation = Quaternion.Euler(0, 0, 0);
        //        gun.AddComponent<PlayerAttack>();

        //        eye.GetComponent<FieldOfView>().weapon = gun.gameObject;
        //        eye.GetComponent<FieldOfView>().gunActive();
        //    }
        //}
    }
  
    IEnumerator healthRegen()
    {
        yield return new WaitForSeconds(2);
    }
    public void healthUp()
    {
        if (playerHealth < 3)
        {
            playerHealth += 1;
            for(int i = 0; i < 3; i++)
            {
                health.transform.GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < playerHealth; i++)
            {
                health.transform.GetChild(i).gameObject.SetActive(true);
            }
            //health.transform.GetChild(playerHealth)
        }
    }
    public void healthDown()
    {
      
            playerHealth -= 1;
            if(playerHealth < 0)
            {
                //finish.transform.GetComponent<Finish>().playerGameOver();

            }
            for (int i = 0; i < 3; i++)
            {
                health.transform.GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < playerHealth; i++)
            {
                health.transform.GetChild(i).gameObject.SetActive(true);
            }
            //health.transform.GetChild(playerHealth)
        
    }
    //[SerializeField]
    //GameObject cam1Object;
    public void playerMovingDirection()
    {
        //animator.SetBool("walk", true);

        if (transform.localEulerAngles.y >= 330 || transform.localEulerAngles.y < 30 )
        {
            //Debug.Log("on");
            animator.SetBool("forward", true);
        }
        else
        {
            animator.SetBool("forward", false);
        }

        if (transform.localEulerAngles.y >= 30 && transform.localEulerAngles.y < 90)
        {
            //Debug.Log("on sag");
            animator.SetBool("forwardleft", true);
        }
        else
        {
            animator.SetBool("forwardleft", false);
        }
        if (transform.localEulerAngles.y >= 90 && transform.localEulerAngles.y < 150)
        {
            //Debug.Log("arka sag");
            animator.SetBool("backwardleft", true);
            animator.SetFloat("Speed", -1.0f);
        }
        else
        {
            animator.SetBool("backwardleft", false);
        }
        if (transform.localEulerAngles.y >= 150 && transform.localEulerAngles.y < 210)
        {
            //Debug.Log("arka");
            animator.SetBool("backward", true);
            animator.SetFloat("Speed", -1.0f);

        }
        else
        {
            animator.SetBool("backward", false);
        }
        if (transform.localEulerAngles.y >= 210 && transform.localEulerAngles.y < 270)
        {
            //Debug.Log(" arka sol");
            animator.SetBool("backwardright", true);
        }
        else
        {
            animator.SetBool("backwardright", false);
        }
        if (transform.localEulerAngles.y >= 270 && transform.localEulerAngles.y < 330)
        {
            //Debug.Log(" on sol");
            animator.SetBool("forwardright", true);
        }
        else
        {
            animator.SetBool("forwardright", false);
        }

    }
    public void playerStop()
    {
        //animator.SetBool("walk", false);

        animator.SetBool("forward", false);
        animator.SetBool("forwardleft", false);
        animator.SetBool("backwardleft", false);
        animator.SetBool("backward", false);
        animator.SetBool("backwardright", false);
        animator.SetBool("forwardright", false);
    }

    private void Update()
    {
        gameActive = Globals.playerControlActive;
        RaycastHit hit;

        if ((Physics.Raycast(transform.position + new Vector3(0, 2f, 0), transform.TransformDirection(Vector3.forward), out hit, 2f) || Physics.Raycast(transform.position + new Vector3(-1f, 2f, 0), transform.TransformDirection(Vector3.forward), out hit, 2f) || Physics.Raycast(transform.position + new Vector3(1f, 2f, 0), transform.TransformDirection(Vector3.forward), out hit, 2f)) && Globals.playerControlActive)
        {
            if (hit.transform.tag == "AIPlayer" && _hit)
            {
                hit.transform.gameObject.GetComponent<Collider>().enabled = false;
                StartCoroutine(hitting(hit.transform.gameObject));
                _hit = false;
                Debug.Log("vurdu");
                //Globals.aiControlActive = false;
                //hit.transform.gameObject.GetComponent<AIBehaviour>().aicntrlAct = false;
                hit.transform.GetComponent<Animator>().SetTrigger("hit");
            }

        }

        if (eye.visibleTargets.Count > 0)
        {
            if(eye.visibleTargets[0] != null)
            lookTarget(eye.visibleTargets[0].transform);
        }
        else
        {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(transform.localEulerAngles.x, 0, transform.localEulerAngles.z), 50 * Time.deltaTime);
        }

  

    }
    void lookTarget(Transform target)
    {

        Vector3 relativeVector = transform.InverseTransformPoint(target.position);
        relativeVector /= relativeVector.magnitude;
        float newSteer = (relativeVector.x / relativeVector.magnitude) * 50;
        transform.Rotate(0, newSteer * Time.deltaTime * 20, 0);
    }
    public void finishDirection(float angle)
    {
        stickDirection = new Vector3(stickDirection.x, angle, stickDirection.y);
    }
    IEnumerator hitting(GameObject AI)
    {
        AI.GetComponent<Rigidbody>().AddForce(new Vector3(0, 2, 0) * 2000);
        AI.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * 20000);

        yield return new WaitForSeconds(1.5f);
        AI.GetComponent<Collider>().enabled = true;
        _hit = true;
        AI.transform.GetComponent<Animator>().SetTrigger("standup");
    }
 

    public void stand_Up()
    {
        Globals.playerControlActive = true;
    }
    public void shotEnd()
    {
        weaponPos.transform.GetChild(0).GetComponent<PlayerAttack>().shotEnd();
    }
}
