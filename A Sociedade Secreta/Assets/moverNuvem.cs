using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moverNuvem : MonoBehaviour
{

    public float velocidade;
    // Update is called once per frame
    void Update()
    {
        if(this.transform.position.x < -25){
            this.transform.position = new Vector3(50,this.transform.position.y,this.transform.position.z);
        } else {
            this.transform.position = new Vector3(this.transform.position.x+velocidade,this.transform.position.y,this.transform.position.z);
        }
    }
}
