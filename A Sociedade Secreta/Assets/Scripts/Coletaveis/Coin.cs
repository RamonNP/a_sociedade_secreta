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
    public void coletar() {
        gameControler.gold += valor;
        Destroy(this.gameObject);
    }
}
