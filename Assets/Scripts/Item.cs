using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    Transform target;
    Vector3 initialPoint;
    float slerpTime = 0;

    void Update() {
        transform.rotation = Camera.main.transform.rotation;
    }

    void FixedUpdate() {
        if(target) {
            slerpTime += Time.deltaTime;
            transform.position = Vector3.Slerp(initialPoint, target.position, slerpTime);
        }
    }

    public void SetPoint(Transform point) {
        GetComponent<Rigidbody>().isKinematic = true;

        initialPoint = transform.position;
        target = point;
    }
}