using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerEvolution : MonoBehaviour,IEvolution
{
    // Start is called before the first frame update
    [SerializeField] public GameObject magnetParticle, moveSpeedParticle, attackSpeedParticle, damageParticle;
    public PlayerAttack _playerAttack;
    bool firstMoveActive;
   public int maxHelath;
  public  PlayerControl _playerControl;
    [SerializeField] Material deadMaterial;
    [SerializeField] SkinnedMeshRenderer characterSkin;
    bool deadActive = false;
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.GetComponent<coin>() != null)
    //    {
    //        other.GetComponent<coin>().collect(_playerControl.transform);
    //    }
    //}
    void Start()
    {
        //maxHelath = 5;
        _playerControl = transform.parent.GetComponent<PlayerControl>();
        StartCoroutine(startPosSet());
        StartCoroutine(spawnDelay());
    }
    IEnumerator spawnDelay()
    {
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(2f);
        GetComponent<Collider>().enabled = true;
    }
    public void firstMove()
    {
        StartCoroutine(startPosSet());
    }
    IEnumerator startPosSet()
    {
        firstMoveActive = false;
        yield return new WaitForSeconds(0.1f);
        firstMoveActive = true;

        float counter = 0f;
        while(counter < 2f && firstMoveActive && !deadActive)
        {
            counter += Time.deltaTime;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, 0.3f * Vector3.Distance(transform.localPosition,Vector3.zero) * Time.deltaTime);
            yield return null;
        }
    }
    #region Duplicate Funcs
    public void duplicate(int cloneCount)
    {
        //for(int i = 0; i < cloneCount; i++)
        //{
        //    GameObject clonePlayer = Instantiate(gameObject,transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)), Quaternion.identity, transform.parent);
        //    _playerControl.players.Add(clonePlayer);
        //}
        StartCoroutine(scaleCalling(cloneCount));
        _playerControl.cameraSetUp(cloneCount);
        // // // // //
    }
    IEnumerator scaleCalling(int cloneCount)
    {
        for (int i = 0; i < cloneCount; i++)
        {
            GameObject clonePlayer = Instantiate(_playerControl.players[0], _playerControl.transform.position, Quaternion.identity, _playerControl.transform);
            clonePlayer.transform.localPosition = new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f));
            _playerControl.players.Add(clonePlayer);
            StartCoroutine(throughlyScaling(clonePlayer.transform));
       
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator throughlyScaling(Transform hmn)
    {

        float counter = 0f;
        float firstSize = 1f;
        float sizeDelta;

        while (counter < firstSize)
        {
            counter += 15 * Time.deltaTime;
            hmn.localScale = new Vector3(counter, counter, counter);
            yield return null;
        }

        counter = 0f;
        while (counter < Mathf.PI)
        {
            counter += 15 * Time.deltaTime;
            sizeDelta = 1f - Mathf.Abs(Mathf.Cos(counter));
            sizeDelta /= 4f;
            hmn.localScale = new Vector3(firstSize + sizeDelta, firstSize + sizeDelta, firstSize + sizeDelta);

            yield return null;
        }
        hmn.localScale = new Vector3(firstSize, firstSize, firstSize);
        for (int t = 0; t < _playerControl.players.Count; t++)
        {
            _playerControl.players[t].GetComponent<PlayerEvolution>().firstMove();
        }
    }
    #endregion
    void damageDead()
    {
        deadActive = true;
        GetComponent<NavMeshAgent>().enabled = false;
        characterSkin.material = deadMaterial;
        GetComponent<Collider>().enabled = false;
        transform.GetChild(0).GetComponent<Ragdoll>().RagdollActivate(true);
        transform.GetChild(0).GetComponent<PlayerController>().eye.viewAngle = 0;



        _playerControl.players.Remove(gameObject);
        _playerControl.cameraSetUp(-1);
        for (int t = 0; t < _playerControl.players.Count; t++)
        {
            _playerControl.players[t].GetComponent<PlayerEvolution>().firstMove();
        }

        if (_playerControl.players.Count == 0)
        {
            transform.parent.GetComponent<PlayerParent>().yearCanvas.SetActive(false);
            GameManager.Instance.Notify_LoseObservers();
        }

        transform.parent = null;
        Destroy(gameObject, 3);
    }
    public void playerDead(int damage)
    {
        if (_playerControl.players.Count > 1)
        {
            maxHelath -= damage;
            if (maxHelath <= 0)
            {
                damageDead();
            }
            //GetComponent<NavMeshAgent>().enabled = false;
            //characterSkin.material = deadMaterial;
            //GetComponent<Collider>().enabled = false;
            //transform.GetChild(0).GetComponent<Ragdoll>().RagdollActivate(true);
            //Destroy(transform.GetChild(0).GetComponent<PlayerController>().eye);



            //_playerControl.players.Remove(gameObject);
            //_playerControl.cameraSetUp(-1);


            //if (_playerControl.players.Count == 0)
            //{
            //    transform.parent.GetComponent<PlayerParent>().yearCanvas.SetActive(false);
            //    GameManager.Instance.Notify_LoseObservers();
            //}

            //transform.parent = null;
            //Destroy(gameObject, 3);
        }
        else
        {
            transform.parent.GetComponent<PlayerParent>().playerYearSet(-5);
            if(Globals.currentYear <= 0)
            {
                deadActive = true;

                GetComponent<NavMeshAgent>().enabled = false;
                characterSkin.material = deadMaterial;
                GetComponent<Collider>().enabled = false;
                transform.GetChild(0).GetComponent<Ragdoll>().RagdollActivate(true);
                Destroy(transform.GetChild(0).GetComponent<PlayerController>().eye);



                _playerControl.players.Remove(gameObject);
                _playerControl.cameraSetUp(-1);




                transform.parent.GetComponent<PlayerParent>().yearCanvas.SetActive(false);
                GameManager.Instance.Notify_LoseObservers();
                

                transform.parent = null;
                Destroy(gameObject, 3);
            }
            else
            {
                firstMove();
            }
        }
    }

    public void levelGate(int yearAmount)
    {
        transform.parent.GetComponent<PlayerParent>().playerYearSet(yearAmount);

    }


    public void magneticGate(int affectTime)
    {
        transform.parent.GetComponent<PlayerControl>().magnetOn(affectTime);

    }
   public void speedGate(int affectTime, int speedUp)
    {
        transform.parent.GetComponent<PlayerControl>().speedOn(affectTime, speedUp);
    }

    public void attackSpeedGate(int affectTime, float attackSpeedUp)
    {
        transform.parent.GetComponent<PlayerControl>().attackSpeedOn(affectTime, attackSpeedUp);

    }
    public void damageGate(int affectTime, int damageUp)
    {
        transform.parent.GetComponent<PlayerControl>().damageOn(affectTime, damageUp);

    }
}
