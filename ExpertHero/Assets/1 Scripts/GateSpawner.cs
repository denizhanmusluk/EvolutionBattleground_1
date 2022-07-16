using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateSpawner : MonoBehaviour, IStartGameObserver
{
    public static GateSpawner Instance;
    public List<GameObject> gateAll;

    public GameObject player;

    [SerializeField] GameObject[] gatePrefab;
    [SerializeField] float spawnPeriod;
    [SerializeField] Transform[] spawnPoints;
    int spawnPointSelect = -1;
    int spawnGateSelect;
    int spawnPointSelectTemp = -2;
    public bool spawnActive = true;
    bool selectAcitve = true;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        GameManager.Instance.Add_StartObserver(this);
    }
    public void StartGame()
    {
        StartCoroutine(spawning());

    }
    IEnumerator spawning()
    {
        while (spawnActive)
        {
            if (Globals.isGameActive && (gateAll.Count < 10))
            {
                spawnPointSelectTemp = Random.Range(0, spawnPoints.Length);
                if (spawnPointSelect != spawnPointSelectTemp)
                {
                    spawnPointSelect = spawnPointSelectTemp;
                    for(int i = 0; i< gateAll.Count; i++)
                    {
                        if (Vector3.Distance( spawnPoints[spawnPointSelect].position, gateAll[i].transform.position) < 50)
                        {
                            selectAcitve = false;
                            break;
                        }
                    }
                    if (selectAcitve)
                    {
                        spawnGateSelect = Random.Range(0, gatePrefab.Length);
                        while(((gatePrefab[spawnGateSelect].name == "x2" || gatePrefab[spawnGateSelect].name == "+5") && player.GetComponent<PlayerControl>().players.Count > 10) || ((gatePrefab[spawnGateSelect].name == "+1" || gatePrefab[spawnGateSelect].name == "+2" || gatePrefab[spawnGateSelect].name == "+3") && player.GetComponent<PlayerControl>().players.Count > 17))
                        {
                            spawnGateSelect = Random.Range(0, gatePrefab.Length);
                        }
                        var gate = Instantiate(gatePrefab[spawnGateSelect], spawnPoints[spawnPointSelect].position, Quaternion.Euler(15, 0, 0));

                        gateAll.Add(gate);
                    }
                    selectAcitve = true;
                }
            }
            yield return new WaitForSeconds(2000f / ((float)Globals.currentBox + 4000f));
        }
    }
    void Update()
    {
        transform.position = player.transform.position;
    }
}
