using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentosAnimation : MonoBehaviour
{
    private HeroiController playScript;

    void Start()
    {
        playScript = FindObjectOfType(typeof(HeroiController)) as HeroiController;
    }
    public void atack(int atk)
    {
//        Debug.Log("Atacando - "+atk);
        playScript.atack(atk);
    }
    public void controleArma(int atk)
    {
//        Debug.Log("Atacando - "+atk);
        playScript.controleArma(atk);
    }
}
