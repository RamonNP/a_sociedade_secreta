using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoControler : MonoBehaviour
{
    //configuração de danos para RPG fogo - agua  - normal 
    public float[] ajusteDano;
    private GameControler gameControler;
    //Knockback
    public GameObject knockbackForcedPrefab;
    public Transform knockbackPosition;
    // Start is called before the first frame update
    void Start()
    {
        gameControler = FindObjectOfType(typeof(GameControler)) as GameControler;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        switch (other.gameObject.tag)
        {
            case "Arma":
                ArmaInfo armaInfo = other.gameObject.GetComponent<ArmaInfo>();
                float danoTomado = armaInfo.dano + (armaInfo.dano * (ajusteDano[armaInfo.tipoDanoTomado])/100);
                
                Debug.Log("Dano Tomado"+ danoTomado+" do Tipo:"+gameControler.tiposDano[armaInfo.tipoDanoTomado]);
                GameObject knockbackTemp = Instantiate(knockbackForcedPrefab, knockbackPosition.position, knockbackPosition.localRotation);
                Destroy(knockbackTemp, 0.03f);
                break;
        }
    }
}
