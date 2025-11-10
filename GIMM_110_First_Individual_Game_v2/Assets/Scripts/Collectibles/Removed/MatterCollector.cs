using UnityEngine;
using System.Collections.Generic;

public class MatterCollector : MonoBehaviour
{
    private List<Matter> collectedMatter = new List<Matter>();

    //Add coins from a list to the collector
    public void AddMatter(List<Matter> matterToAdd)
    {
        collectedMatter.AddRange(matterToAdd);
        Debug.Log("Coins added to the main collections");
    }

    //Count how many of each coin type there are
    public void DisplayTotals()
    {
        int darkMatterCount = 0;
 

        foreach (Matter matter in collectedMatter)
        {
            if (matter is DarkMatter) darkMatterCount++;
        }

        Debug.Log("------ Totals ------");
        Debug.Log("Dark Matter: " + darkMatterCount);
        Debug.Log("-------------------------");

    }

}
