using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    void Update() {
        transform.rotation = Camera.main.transform.rotation;
    }

    public void SetPoint(Transform point) {
        transform.parent = point;
        transform.localPosition = Vector3.zero;

        GetComponent<Rigidbody>().isKinematic = true;
    }
}