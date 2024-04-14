using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archangel : MonoBehaviour
{
    PlayerController player;

    float time = 0;
    public float maxTime = 5;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectsOfType<PlayerController>()[0];

        player.isInvincible = true;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time > maxTime) {
            player.isInvincible = false;
            Destroy(transform.parent.gameObject);
        }
    }
}
