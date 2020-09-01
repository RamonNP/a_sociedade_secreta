using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcaoBTN : MonoBehaviour
{
    public Fade fade;

    public AudioController audioC;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void novaScena() {
        fade = FindObjectOfType(typeof(Fade) ) as Fade;
        fade.fadeIn();
        audioC = FindObjectOfType(typeof(AudioController)) as AudioController;
        audioC.trocarMusica(audioC.musicaFase1, "historiaCena1", true);

    }
}
