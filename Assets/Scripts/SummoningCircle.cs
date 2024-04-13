using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummoningCircle : MonoBehaviour {
    float drawCompletionPercent = 0;
    float drawSpeed = 0.5f;
    bool isDrawn = false;
    Renderer renderer;

    void Update() {
        if(!isDrawn) {
            if (renderer == null) { renderer = GetComponent<Renderer>(); }
            drawCompletionPercent += Time.deltaTime * (1 / drawSpeed);

            if(drawCompletionPercent >= 1.0f) {
                drawCompletionPercent = 1.0f;
                isDrawn = true;
            }

            renderer.material.SetFloat("_Percent", drawCompletionPercent);
        }
    }

    public void Activate() {
        if (!isDrawn) { return; }
        
        Collider[] possibleColliders = Physics.OverlapBox(transform.position, new Vector3(2.5f, 2.5f, 2.5f), Quaternion.identity, 1 << 8);

        for (int i = 0; i < 5; i++) {
            Transform curNode = transform.GetChild(i);

            float closestDist = Mathf.Infinity;
            int closestCollider = -1;

            for (int j = 0; j < possibleColliders.Length; j++) {
                Collider collider = possibleColliders[j];

                if(collider == null) { continue; }

                float dist = (collider.transform.position - curNode.position).sqrMagnitude;

                if (dist < closestDist) {
                    closestCollider = j;
                    closestDist = dist;
                }
            }

            if(closestCollider != -1) {
                print(closestCollider);

                possibleColliders[closestCollider].GetComponent<Item>().SetPoint(curNode);

                possibleColliders[closestCollider] = null;
            }
        }
    }
}
