using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerControl : MonoBehaviour, IStartGameObserver
{
    [SerializeField] Transform enemySpawner;
    [SerializeField]
    GameObject boosterCanvas;
    [SerializeField] GameObject magnetImage, moveSpeedImage, attackSpeedImage, damageImage;
    [SerializeField] public List<GameObject> players;

    private float m_previousY;
    private float dY;
    private float m_previousX;
    private float dX;


    public float acceleration = 15;


    [SerializeField] public CinemachineVirtualCamera Camera;
    public enum States { idle, runner, idleControl , runnerToIdle}
    public States currentBehaviour;
    public int slotNum = 0;

    public Transform moneylabel;
   public PlayerParent playerParent;



    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private Vector2 currentSwipe;
    public bool pressed = false;
    //public Animator anim;
    public float speed;
    //GameObject parent;
    float spd;
    //public bool idleControlActive = true;
    public bool runnerControlActive = false;

    [SerializeField] public GameObject moneyTarget;
    //[SerializeField] GameObject rainParticle;
    float cameraView;
    float cameraBodyOffsetZ;
    float cameraTrackedOffsetY;
   public int currentHealth = 100;
    int currentIconCount = 0;
    private void Awake()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<coin>() != null)
        {
            other.GetComponent<coin>().collect(transform);
            Globals.currentBox++;
            chatacterHealthSet();
        }
    }
    public void chatacterHealthSet()
    {
        if(Globals.currentBox>100 && Globals.currentBox < 200)
        {
            currentHealth = 150;
        }
        if (Globals.currentBox >= 200)
        {
            currentHealth = 225;
        }
    }
    private void Start()
    {
        cameraView = Camera.m_Lens.FieldOfView;
        cameraBodyOffsetZ = Camera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z;
        cameraTrackedOffsetY = Camera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.y;
        StartGame();
        spd = acceleration;
    }

    public void StartGame()
    {
        currentBehaviour = States.idleControl;
    }

    public void cameraSetUp(int cloneAmount)
    {
        if (cloneAmount + players.Count < 13)
        {
            if (cameraView >= 66.9f)
            {
                cameraViewSet(cloneAmount);
                cameraBodyOffset(cloneAmount);
                cameraTrackedOffset(cloneAmount);
            }
            else
            {
                Camera.m_Lens.FieldOfView = 66.9f;
                Camera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z = -32.2f;
                Camera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.y = -10.8f;
            }
        }
        else
        {
            LeanTween.value(cameraView, 94.5f, 0.5f).setOnUpdate((float val) =>
            {
                Camera.m_Lens.FieldOfView = val;
            });
            LeanTween.value(cameraBodyOffsetZ, -25f, 0.5f).setOnUpdate((float val) =>
            {
                Camera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z = val;
            });
            LeanTween.value(cameraTrackedOffsetY, -22.5f, 0.5f).setOnUpdate((float val) =>
            {
                Camera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.y = val;
            });
       
        }
    }
    public void cameraViewSet(int cloneAmount)
    {
        float deltaView = 2.3f * (float)cloneAmount;
        float viewOld = cameraView;
        cameraView = cameraView + deltaView;
        LeanTween.value(viewOld, cameraView, 0.5f).setOnUpdate((float val) =>
        {
            Camera.m_Lens.FieldOfView = val;
        });
    }
    public void cameraBodyOffset(int cloneAmount)
    {
        float deltaZ = 0.6f * (float)cloneAmount;
        float ZOld = cameraBodyOffsetZ;
        cameraBodyOffsetZ = cameraBodyOffsetZ + deltaZ;
        LeanTween.value(ZOld, cameraBodyOffsetZ, 0.5f).setOnUpdate((float val) =>
        {
            Camera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z = val;
        });
    }
    public void cameraTrackedOffset(int cloneAmount)
    {
        float deltaY = -0.9f * (float)cloneAmount;
        float YOld = cameraTrackedOffsetY;
        cameraTrackedOffsetY = cameraTrackedOffsetY + deltaY;
        LeanTween.value(YOld, cameraTrackedOffsetY, 0.5f).setOnUpdate((float val) =>
        {
            Camera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.y = val;
        });
    }
    private void LateUpdate()
    {
        switch (currentBehaviour)
        {
            case States.idle:
                {
                }
                break;
            case States.runner:
                {
                    if (runnerControlActive)
                    {

                    }
                }
                break;
            case States.idleControl:
                {
                    if (Globals.isGameActive)
                    {
                        IdleControl();
                    }
                }
                break;
            case States.runnerToIdle:
                {
                }
                break;
        }
    }

   

    public void IdleControl()
    {

        if (Input.GetMouseButtonDown(0))
        {
            m_previousX = Input.mousePosition.x;
            dX = 0f;
            m_previousY = Input.mousePosition.y;
            dY = 0f;

            firstPressPos = (Vector2)Input.mousePosition;
            pressed = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            secondPressPos = (Vector2)Input.mousePosition;
            currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
            firstPressPos = (Vector2)Input.mousePosition;
            pressed = false;
            dX = 0f;
            dY = 0f;

        }

        if (pressed == true)
        {
            dX = (Input.mousePosition.x - m_previousX);
            dY = (Input.mousePosition.y - m_previousY);

            //foreach (var anim in GetComponentsInChildren<Animator>())
            //{
            //    anim.SetBool("walk", true);
            //}
            secondPressPos = (Vector2)Input.mousePosition;
            currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
            currentSwipe.Normalize();


            Vector3 direction = new Vector3(currentSwipe.x, 0f, currentSwipe.y);

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Quaternion newRot = Quaternion.Euler(0, targetAngle, 0);

            if (direction != Vector3.zero)
            {
                enemySpawner.rotation = Quaternion.RotateTowards(enemySpawner.rotation, newRot, 300 * Time.deltaTime);
                for (int i = 0; i < players.Count; i++)
                {
                    players[i].transform.rotation = Quaternion.RotateTowards(players[i].transform.rotation, newRot, 300 * Time.deltaTime);
                    players[i].transform.GetChild(0).GetComponent<PlayerController>().playerMovingDirection();
                }
            }

            transform.position = transform.position + (direction * speed * Time.deltaTime);
            //if(Vector2.Distance(secondPressPos,firstPressPos) > 500f)
            //{
            //    firstPressPos += new Vector2(dX, dY);

            //}
            m_previousX = Input.mousePosition.x;
            m_previousY = Input.mousePosition.y;
        }
        else
        {
            //foreach (var anim in GetComponentsInChildren<Animator>())
            //{
            //    anim.SetBool("walk", false);
            //}
            for (int i = 0; i < players.Count; i++)
            {
                players[i].transform.GetChild(0).GetComponent<PlayerController>().playerStop();
            }
        }
    }

    //IEnumerator targetMotion(GameObject money)
    //{
    //    while (Vector3.Distance(money.transform.position, moneyTarget.transform.position) > 0.3f)
    //    {
    //        money.transform.position = Vector3.MoveTowards(money.transform.position, moneyTarget.transform.position, (3 / Vector3.Distance(money.transform.position, moneyTarget.transform.position)) * acceleration * Time.deltaTime);
    //        money.transform.localScale = Vector3.Lerp(money.transform.localScale, moneyTarget.transform.localScale, acceleration * 0.3f * Time.deltaTime);
    //        yield return null;
    //    }
    //    //LevelScore.Instance.MoneyUpdate(money.transform.GetComponent<MoneyCollecting>().moneyValue);

    //    money.transform.parent = null;
    //    Destroy(money);
    //}


    /////////// MAGNET GATE  \\\\\\\\\\
    public void magnetOn(int affectTime)
    {
        currentIconCount++;
        Vector2 iconPosition = posSet(currentIconCount);

        var icon = Instantiate(magnetImage, iconPosition, Quaternion.identity, boosterCanvas.transform);
        icon.GetComponent<boosterIcon>().time(affectTime);
        for (int i = 0; i < players.Count; i++)
        {
            players[i].GetComponent<PlayerEvolution>().magnetParticle.SetActive(true);
        }
        GetComponent<CapsuleCollider>().radius = 200;
        StartCoroutine(magnetOnOf(affectTime));
    }
    IEnumerator magnetOnOf(int affectTime)
    {
        yield return new WaitForSeconds(affectTime);
        GetComponent<CapsuleCollider>().radius = 15;
        for (int i = 0; i < players.Count; i++)
        {
            players[i].GetComponent<PlayerEvolution>().magnetParticle.SetActive(false);
        }
        currentIconCount--;
    }
    /////////// MOVESPEED GATE  \\\\\\\\\\
    public void speedOn(int affectTime,int speedUp)
    {
        currentIconCount++;
        Vector2 iconPosition = posSet(currentIconCount);

        var icon = Instantiate(moveSpeedImage, iconPosition, Quaternion.identity, boosterCanvas.transform);
        icon.GetComponent<boosterIcon>().time(affectTime);
        speed = speedUp;
        for (int i = 0; i < players.Count; i++)
        {
            players[i].GetComponent<PlayerEvolution>().moveSpeedParticle.SetActive(true);
        }
        StartCoroutine(speedOnOf(affectTime, speedUp));
    }
    IEnumerator speedOnOf(int affectTime, int speedUp)
    {
        yield return new WaitForSeconds(affectTime);
        speed = 15;
        for (int i = 0; i < players.Count; i++)
        {
            players[i].GetComponent<PlayerEvolution>().moveSpeedParticle.SetActive(false);
        }
        currentIconCount--;
    }
    /////////// ATTACKSPEED GATE  \\\\\\\\\\????????????
    public void attackSpeedOn(int affectTime, float attackSpeedUp)
    {
        currentIconCount++;
        Vector2 iconPosition = posSet(currentIconCount);

        var icon = Instantiate(attackSpeedImage, iconPosition, Quaternion.identity, boosterCanvas.transform);
        icon.GetComponent<boosterIcon>().time(affectTime);
        for (int i = 0; i < players.Count; i++)
        {
            players[i].GetComponent<PlayerEvolution>()._playerAttack.attackSpeed *= attackSpeedUp;
            players[i].GetComponent<PlayerEvolution>().attackSpeedParticle.SetActive(true);

        }
        StartCoroutine(attackSpeedOnOf(affectTime, attackSpeedUp));
    }
    IEnumerator attackSpeedOnOf(int affectTime, float attackSpeedUp)
    {
        yield return new WaitForSeconds(affectTime);
        for (int i = 0; i < players.Count; i++)
        {
            players[i].GetComponent<PlayerEvolution>()._playerAttack.attackSpeed /= attackSpeedUp;
            players[i].GetComponent<PlayerEvolution>().attackSpeedParticle.SetActive(false);

        }
        currentIconCount--;
    }

    /////////// DAMAGE GATE  \\\\\\\\\\????????????
    public void damageOn(int affectTime, int attackSpeedUp)
    {
        currentIconCount++;
        Vector2 iconPosition = posSet(currentIconCount);
        var icon = Instantiate(damageImage, iconPosition, Quaternion.identity, boosterCanvas.transform);
        icon.GetComponent<boosterIcon>().time(affectTime);
        for (int i = 0; i < players.Count; i++)
        {
            players[i].GetComponent<PlayerEvolution>()._playerAttack.damage *= attackSpeedUp;
            players[i].GetComponent<PlayerEvolution>()._playerAttack.damageUpActive = true;
            players[i].GetComponent<PlayerEvolution>().damageParticle.SetActive(true);

        }
        StartCoroutine(damageOnOf(affectTime, attackSpeedUp));
    }
    IEnumerator damageOnOf(int affectTime, int attackSpeedUp)
    {
        yield return new WaitForSeconds(affectTime);
        for (int i = 0; i < players.Count; i++)
        {
            players[i].GetComponent<PlayerEvolution>()._playerAttack.damage /= attackSpeedUp;
            players[i].GetComponent<PlayerEvolution>()._playerAttack.damageUpActive = false;
            players[i].GetComponent<PlayerEvolution>().damageParticle.SetActive(false);

        }
        currentIconCount--;
    }
    Vector2 posSet(int iconAmount)
    {
        if(iconAmount == 1)
        {
            return new Vector2(6 * Screen.width / 12, 6 * Screen.height / 7);
        }
        else if (iconAmount == 2)
        {
            return new Vector2(4 * Screen.width / 12, 6 * Screen.height / 7);

        }
        else if (iconAmount == 3)
        {
            return new Vector2(8 * Screen.width / 12, 6 * Screen.height / 7);

        }
        else
        {
            return new Vector2(10 * Screen.width / 12, 6 * Screen.height / 7);

        }

    }
}
