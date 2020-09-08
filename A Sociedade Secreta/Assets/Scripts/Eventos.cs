using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eventos : MonoBehaviour
{
    private AudioController audioController;
    private AnimacaoScript animacaoScript;
    // Start is called before the first frame update
    void Start()
    {
        animacaoScript = FindObjectOfType(typeof(AnimacaoScript)) as AnimacaoScript;
        audioController = FindObjectOfType(typeof(AudioController)) as AudioController;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void chamarEvento(int obj) {
        if(obj == 1) {
            audioController.tocarFx(audioController.fxMagicEarth, 1);
        }
        if(obj == 0) {
            audioController.tocarFx(audioController.fxShield, 1);
        }
        animacaoScript.eventoAnimacao(obj);
    }
}
