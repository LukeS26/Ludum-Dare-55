using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    Vector3 initialPoint;

    public int id;

    void Update() {
        transform.rotation = Camera.main.transform.rotation;
    }

    public void PickedUp() {
        if(transform.parent == null) { return; }

        for(int i = 0; i < 4; i++) {
            if(transform.parent.parent.GetChild(i).childCount > 0) {
                if(transform.parent.parent.GetChild(i).GetChild(0) == transform) {
                    transform.parent.parent.parent.GetComponent<SummoningCircle>().RemoveChild(i);
                    return;
                }
            }

        }
    }

    public void DropPickup() {
        GetComponent<Rigidbody>().isKinematic = false;
    }
}