using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummoningCircle : MonoBehaviour {
    public GameObject[] summons;

    float drawCompletionPercent = 0;
    float drawSpeed = 0.5f;
    bool isDrawn = false;
    bool isStarted = false;
    Renderer circleRenderer;

    public int collectIndex = 0;

    public Item[] items = new Item[4];

    public bool SpotFree(int index) {
        return items[index] == null;
    }

    void Update() {
        if(!isDrawn) {
            if (circleRenderer == null) { circleRenderer = transform.GetChild(0).GetComponent<Renderer>(); }

            drawCompletionPercent += Time.deltaTime * (1 / drawSpeed);

            if(drawCompletionPercent >= 1.0f) {
                drawCompletionPercent = 1.0f;
                isDrawn = true;
                Ready();
            }

            circleRenderer.material.SetFloat("_Percent", drawCompletionPercent);
        } else {
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    void Ready() {
        if (isStarted) { return; }

        Collider[] possibleColliders = Physics.OverlapBox(transform.position, new Vector3(3f, 3f, 3f), Quaternion.identity, 1 << 8);

        for (int i = 0; i < possibleColliders.Length; i++) {
            possibleColliders[i].GetComponent<Rigidbody>().AddForce((possibleColliders[i].transform.position - transform.position).normalized * 3, ForceMode.Impulse);
        }

        isStarted = true;
    }

    public void FillSpot(int index, Item item) {
        items[index] = item;
    }

    public void Activate() {
        int index = 0;
        Instantiate(summons[index], transform);
    }
}
