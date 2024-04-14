using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coolerHitBox : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider col){
        PlayerController player = col.transform.gameObject.GetComponent<PlayerController>();
        if (col){
            Debug.Log("Damage, but coolS");
        }
    }

}
