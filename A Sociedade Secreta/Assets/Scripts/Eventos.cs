using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eventos : MonoBehaviour
{
    private AnimacaoScript animacaoScript;
    // Start is called before the first frame update
    void Start()
    {
        animacaoScript = FindObjectOfType(typeof(AnimacaoScript)) as AnimacaoScript;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void chamarEvento(int obj) {
        animacaoScript.eventoAnimacao(obj);
    }
}
