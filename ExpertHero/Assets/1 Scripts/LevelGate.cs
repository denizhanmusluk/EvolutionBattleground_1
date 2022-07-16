using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LevelGate : MonoBehaviour
{
    public enum States1 { Month, Year }
    public States1 selectTime;
    public enum States2 { AddGate, SubGate }
    public States2 selectSign;

    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] int level;
    int year;
    int _sign;
    string signText;
    string time;
    GateBreak gateBreak;

    void Start()
    {
        gateBreak = GetComponent<GateBreak>();

        switch (selectTime)
        {
            case States1.Month:
                {
                    year = level / 12;
                    time = "Month";
                }
                break;
            case States1.Year:
                {
                    year = level;
                    time = "Year";

                }
                break;
        }
        switch (selectSign)
        { 
            case States2.AddGate:
                {
                    _sign = 1;
                    signText = "+";
                }
                break;
            case States2.SubGate:
                {
                    _sign = -1;
                    signText = "-";
                }
                break;
        }
        year *= _sign;
        levelText.text = signText + level.ToString() + "   " + time;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerEvolution>() != null)
        {
            other.GetComponent<PlayerEvolution>().levelGate(year);
            GetComponent<Collider>().enabled = false;
            gateBreak.glassHit(other.gameObject);
            GateSpawner.Instance.gateAll.Remove(gameObject);

        }
    }
}
