using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummoningCircle : MonoBehaviour {
    float drawCompletionPercent = 0;
    float drawSpeed = 0.5f;
    bool isDrawn = false;
    bool isStarted = false;
    Renderer renderer;

    public int collectIndex = 0;

    public Item[] items = new Item[5];

    void Update() {
        if(!isDrawn) {
            if (renderer == null) { renderer = GetComponent<Renderer>(); }
            drawCompletionPercent += Time.deltaTime * (1 / drawSpeed);

            if(drawCompletionPercent >= 1.0f) {
                drawCompletionPercent = 1.0f;
                isDrawn = true;
                Activate();
            }

            renderer.material.SetFloat("_Percent", drawCompletionPercent);
        } else {

        }
    }

    public void Activate() {
        if (isStarted) { return; }

        Collider[] possibleColliders = Physics.OverlapBox(transform.position, new Vector3(3f, 3f, 3f), Quaternion.identity, 1 << 8);

        for (int i = 0; i < possibleColliders.Length; i++) {
            possibleColliders[i].GetComponent<Rigidbody>().AddForce((possibleColliders[i].transform.position - transform.position).normalized * 3, ForceMode.Impulse);
        }

        isStarted = true;
    }
}
