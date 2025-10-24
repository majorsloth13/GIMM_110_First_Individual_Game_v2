using UnityEngine;
using System.Collections.Generic;

public class CoinGameController : MonoBehaviour
{
    private List<Coin> tempBag = new List<Coin>(); //Temporary holding bag
    private List<Star> tempoBag = new List<Star>(); //Temporary holding bag
    private CoinCollecter collector = new CoinCollecter();

    // Update is called once per frame
    void Update()
    {
        //Add a coin with keys
        if (Input.GetKeyDown(KeyCode.C))
        {
            tempBag.Add(new CopperCoin());
            Debug.Log("You picked up a copper coin");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            tempBag.Add(new SilverCoin());
            Debug.Log("You picked up a silver coin");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            tempBag.Add(new GoldCoin());
            Debug.Log("You picked up a gold coin");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            tempoBag.Add(new SilverStar());
            Debug.Log("You picked up a silver star");
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            tempoBag.Add(new GoldStar());
            Debug.Log("You picked up a gold star");
        }

        //Press E to send coins to collector
        if (Input.GetKeyDown(KeyCode.E))
        {
            collector.AddCoins(tempBag);
            collector.AddStars(tempoBag);
            tempBag.Clear(); //empty the temporary bag
            tempoBag.Clear(); //empty the temporary bag
            Debug.Log("You sent your coins to the main collection");
            Debug.Log("You sent your stars to the main collection");
        }

        //Press I to check totals
        if(Input.GetKeyDown(KeyCode.I))
        {
            collector.DisplayTotals();
        }
    }
}
