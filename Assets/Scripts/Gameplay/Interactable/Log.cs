using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour, IInteractable
{
    public LogTextSO logSO;
    private Tablet tablet;

    public string InteractionText { get; set; } = "Read";

    public void Interact() {
        tablet.OpenLog(logSO);
    }

    // Start is called before the first frame update
    void Start()
    {
        tablet = FindFirstObjectByType<Tablet>(FindObjectsInactive.Include);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
