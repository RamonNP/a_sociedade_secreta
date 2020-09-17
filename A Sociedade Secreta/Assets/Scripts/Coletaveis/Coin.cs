using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private GameControler gameControler;
    public int valor;

    void Start()
    {
        gameControler = GameControler.getInstance();
    }
    public void coletar(int id) {

        //Debug.Log("CHEGOU - "+id);
        if(id == 1) { //moeda
            gameControler.gold += valor;
            Destroy(this.gameObject);
        }
        if(id == 2) { //flexa
            gameControler.quantidadeFlechas += valor;
            Destroy(this.gameObject);
        }
    }
}
