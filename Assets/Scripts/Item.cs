using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    Transform target;
    Vector3 initialPoint;

    float slerpPercent = 0;
    float slerpTime = 0.5f;

    void Update() {
        transform.rotation = Camera.main.transform.rotation;
        if (target) {
            slerpPercent += Time.deltaTime * (1 / slerpTime);

            transform.position = Vector3.Slerp(initialPoint, target.position, slerpPercent);
        }
    }

    public void DropPickup() {
        GetComponent<Rigidbody>().isKinematic = false;
        target = null;
        slerpPercent = 0;
    }


    public void SetPoint(Transform point) {
        target = point;
        initialPoint = transform.position;
        GetComponent<Rigidbody>().isKinematic = true;
    }
}