using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beacon : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Vector3 disp = (Camera.main.transform.position - transform.position);
        disp = new Vector3(disp.x, 0f, disp.z).normalized;
        transform.rotation = Quaternion.LookRotation(-disp, Vector3.up);
    }
}
