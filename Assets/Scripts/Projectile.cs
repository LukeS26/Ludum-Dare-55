using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float deathTime = 3;
    float timer = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
        timer += Time.deltaTime;
        if (timer > deathTime){Destroy(gameObject);}
            
    }

    void OnTriggerEnter(Collider col) {
        PlayerController player = col.transform.gameObject.GetComponent<PlayerController>();
        if (player)
            Debug.Log("Hit my Briches");
            // Write code for player getting damaged
    }
}
