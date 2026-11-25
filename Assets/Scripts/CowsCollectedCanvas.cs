using System;
using System.IO;
using TMPro;
using UnityEngine;

public class CowsCollectedCanvas : ISubscriber
{
    [SerializeField]
    AnimalRecoveryTracker animalRecoveryTracker;
    [SerializeField]
    private TextMeshProUGUI cowCount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animalRecoveryTracker.AddSubscriber(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Notify(string info)
    {
        if (info == "recovered")
        {
            Debug.Log("Recieved");
            // Used AI to understand the existing code, specifically how the count of recovered animals works 
            int recovered = animalRecoveryTracker.RecoveredCount;
            cowCount.text = $"{recovered} Cows";
        }
    }
}
