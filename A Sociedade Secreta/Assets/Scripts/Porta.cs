using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porta : MonoBehaviour
{
    private Fade fade;
    public Transform tPlayer;
    public Transform destino; 

    public bool escuro;
    public Material luz2D, padrao2D;
    // Start is called before the first frame update
    void Start()
    {
        fade = FindObjectOfType(typeof(Fade)) as Fade;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void interacao(){
        StartCoroutine("acionarPorta");
    }
    IEnumerator acionarPorta() {
        fade.fadeIn();
        yield return new WaitWhile(() => fade.fume.color.a < 0.9f);
        tPlayer.gameObject.SetActive(false);
        if(escuro){
            tPlayer.gameObject.GetComponent<SpriteRenderer>().material = luz2D;
        } else {
            tPlayer.gameObject.GetComponent<SpriteRenderer>().material = padrao2D;
        }

        tPlayer.position = destino.position;
        yield return new WaitForSeconds(0.5f);
        tPlayer.gameObject.SetActive(true);
        fade.fadeOut();
    }
}
