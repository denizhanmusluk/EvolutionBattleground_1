using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
public class PlayerParent : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText, levelYearText;
    [SerializeField] Slider levelBar;
    [SerializeField] public List<GameObject> humans;
    public int currentYear;
    [SerializeField] TextMeshProUGUI yearText;
    [SerializeField] public GameObject yearCanvas;
    //public NavMeshAgent agent;
    public int level;
    PlayerControl playerControl;
    int currentPlayerCount;
    public int[] levelYears;

    [SerializeField] GameObject yearUpParticle;
    bool yearParticleActive = false;
    private void Start()
    {
        playerControl = GetComponent<PlayerControl>();
 
        level = 0;
        currentYear = 0;
        Globals.currentYear = 0;
        yearText.text = currentYear.ToString();
        //agent = GetComponent<NavMeshAgent>();
        //agent.enabled = false;

        levelText.text = (level + 1).ToString();
        levelYearText.text = currentYear.ToString("N0") + "/" + levelYears[level].ToString();
        levelBar.value = 0f;
    }
    public void throughlyScale()
    {
    }
    IEnumerator scaleCalling()
    {
        int humanCount = humans.Count;
        for (int i = 0; i < humanCount - 1; i++)
        {
            StartCoroutine(throughlyScaling(humans[humanCount - 1 - i].transform));
            yield return new WaitForSeconds(0.05f);
        }
        yearParticleActive = false;
    }
    IEnumerator throughlyScaling(Transform hmn)
    {
        //////
        if (yearParticleActive)
        {
            GameObject yearUp = Instantiate(yearUpParticle, hmn.transform.position + new Vector3(0, 1, 0), Quaternion.Euler(-90, 0, 0));
            yearUp.transform.localScale = new Vector3(7, 7, 7);
        }
        float counter = 0f;
        float firstSize = 1f;
        float sizeDelta;
        while (counter < Mathf.PI)
        {
            counter += 15 * Time.deltaTime;
            sizeDelta = 1f - Mathf.Abs(Mathf.Cos(counter));
            sizeDelta /= 3f;
            hmn.localScale = new Vector3(firstSize + sizeDelta, firstSize + sizeDelta, firstSize + sizeDelta);

            yield return null;
        }
        hmn.localScale = new Vector3(firstSize, firstSize, firstSize);

    }
    //public void UItargetSelect()
    // {
    //     if (humans.Count > 1)
    //     {
    //         direction.selectTarget(humans[humans.Count - 1].GetComponent<Employee>().jobId, transform);
    //     }
    //     else
    //     {
    //         direction.selectTarget(0, transform);
    //         direction.arrowScaleSet();
    //     }
    // }

    public void playerYearSet(int year)
    {
        currentYear += year;

        if (currentYear < 0)
        {
            currentYear = 0;
            YearUpdate(-Globals.currentYear);
        }
        else
        {
            YearUpdate(year);
        }
        //yearText.text = currentYear.ToString();
        //Globals.currentYear = currentYear;
        //if (currentYear > 40)
        //{

        //}
        if (currentYear> levelYears[level] && levelYears[levelYears.Length - 1] > currentYear)
        {
            yearParticleActive = true;
            level++;
            evolutionSet();
        }
        if (level > 0)
        {
            if (currentYear < levelYears[level - 1])
            {
                level--;
                evolutionSet();
            }
        }
    }
    public void YearUpdate(int miktar)
    {
        levelText.text = (level + 1).ToString();
        int yearOld = Globals.currentYear;
        Globals.currentYear = Globals.currentYear + miktar;
        LeanTween.value(yearOld, Globals.currentYear, 0.2f).setOnUpdate((float val) =>
        {
            yearText.text = val.ToString("N0");
            levelYearText.text = val.ToString("N0") + "/" + levelYears[level].ToString();
            //levelBar.fillAmount = (val) / levelYears[level];
            if (level > 0)
            {
                //levelYearText.text = (val - levelYears[level - 1]).ToString("N0") + "/" + levelYears[level].ToString();
                levelBar.value = (val - levelYears[level - 1]) / (levelYears[level] - levelYears[level - 1]);
            }
            else
            {
                //levelYearText.text = val.ToString("N0") + "/" + levelYears[level].ToString();
                levelBar.value = (val) / levelYears[level];
            }
        });//.setOnComplete(() =>{});
        //PlayerPrefs.SetInt("money", Globals.moneyAmount);

    }
    void evolutionSet()
    {
        currentPlayerCount = playerControl.players.Count;

        StartCoroutine(evolutionSett());
    }
    IEnumerator evolutionSett()
    {
  
        for (int i = 0; i < playerControl.players.Count; i++)
        {
            GameObject player = Instantiate(humans[level], playerControl.players[i].transform.position, Quaternion.identity, this.transform);
            player.transform.localPosition = Vector3.zero;
            player.GetComponent<PlayerEvolution>().maxHelath = GetComponent<PlayerControl>().currentHealth;
            Destroy(playerControl.players[i]);
            playerControl.players[i] = player;
            StartCoroutine(throughlyScaling(player.transform));
            targetSelectManager.Instance.Notify_ChangeObservers();

            yield return new WaitForSeconds(0.1f);

        }
    }
}