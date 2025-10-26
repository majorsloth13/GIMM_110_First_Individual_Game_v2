using System.Collections.Generic;
using UnityEngine;

public class Matter : MonoBehaviour
{
    //Private variable {Encapsulation}
    private int coinValue;

    //Public Getter and Setter
    public int GetValue()
    {
        return coinValue;
    }

    public void SetValue(int value) 
    {
        coinValue = value;
    }

}