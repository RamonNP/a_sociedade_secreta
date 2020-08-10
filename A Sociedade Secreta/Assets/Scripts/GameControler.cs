using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControler : MonoBehaviour
{
    private Fade fade;
    public int gold;
    public string[] tiposDano;

    public GameObject[] fxDano;
    public GameObject fxMorte;
    
    // Start is called before the first frame update
    void Start()
    {
        fade = FindObjectOfType(typeof(Fade)) as Fade;
        fade.fadeOut();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
