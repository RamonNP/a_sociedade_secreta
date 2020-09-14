using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chave : MonoBehaviour
{
    private Hud hud;
    // Start is called before the first frame update
    void Start()
    {
        hud = FindObjectOfType(typeof(Hud)) as Hud;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void interacao() {
        hud.mostrarChave();
        Destroy(this.gameObject);
    }
}
