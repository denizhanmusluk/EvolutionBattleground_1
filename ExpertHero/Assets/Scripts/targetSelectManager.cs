using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetSelectManager : MonoBehaviour
{
    public static targetSelectManager Instance;
    private List<ITargetChange> targetChangeObservers;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        targetChangeObservers = new List<ITargetChange>();

    }
    void Start()
    {
        
    }

    public void Add_ChangeObserver(ITargetChange observer)
    {
        targetChangeObservers.Add(observer);
    }

    public void Remove_ChangeObserver(ITargetChange observer)
    {
        targetChangeObservers.Remove(observer);
    }

    public void Notify_ChangeObservers()
    {
        foreach (ITargetChange observer in targetChangeObservers.ToArray())
        {
            if (targetChangeObservers.Contains(observer))
                observer.targetChange();
        }
    }
}
