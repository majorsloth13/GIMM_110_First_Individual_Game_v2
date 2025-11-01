using UnityEngine;
using System.Collections.Generic;

public class MatterGameController : MonoBehaviour
{
    private List<Matter> tempBag = new List<Matter>(); //Temporary holding bag
    private MatterCollector collector = new MatterCollector();

    // Update is called once per frame
    void Update()
    {
        //Add a coin with keys
        if (Input.GetKeyDown(KeyCode.C))
        {
            tempBag.Add(new DarkMatter());
            Debug.Log("You picked up some Dark Matter");
        }

        //Press E to send coins to collector
        if (Input.GetKeyDown(KeyCode.E))
        {
            collector.AddMatter(tempBag);
            tempBag.Clear(); //empty the temporary bag
            Debug.Log("You sent your Dark Matter to the ship");
        }

        //Press I to check totals
        if(Input.GetKeyDown(KeyCode.I))
        {
            collector.DisplayTotals();
        }
    }
}
