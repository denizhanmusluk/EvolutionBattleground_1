using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class duplicateGate : MonoBehaviour
{
    public enum States1 { Add, Multiple }
    public States1 selectTime;
    [SerializeField] TextMeshProUGUI countText;
    [SerializeField] int cloneCount;
    GateBreak gateBreak;
    void Start()
    {
        switch (selectTime)
        {
            case States1.Add:
                {
                    countText.text = "+" + cloneCount.ToString() + "   " + "PEOPLE";

                }
                break;
            case States1.Multiple:
                {

                    countText.text = "x" + cloneCount.ToString() + "   " + "PEOPLE";

                }
                break;
        }
        gateBreak = GetComponent<GateBreak>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerEvolution>() != null)
        {
            switch (selectTime)
            {
                case States1.Add:
                    {
                        other.GetComponent<PlayerEvolution>().duplicate(cloneCount);

                    }
                    break;
                case States1.Multiple:
                    {


                        other.GetComponent<PlayerEvolution>().duplicate(other.GetComponent<PlayerEvolution>()._playerControl.players.Count * (cloneCount - 1));

                    }
                    break;
            }
            GetComponent<Collider>().enabled = false;
            gateBreak.glassHit(other.gameObject);
            GateSpawner.Instance.gateAll.Remove(gameObject);

        }
    }
}
