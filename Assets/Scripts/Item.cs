using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    void Update() {
        transform.localRotation = Camera.main.transform.rotation;
    }
}