using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour {
    public List<int> recipe = new List<int>();

    void Start() {
        
    }

    void Update() {
        transform.GetChild(0).localEulerAngles = new Vector3(
            0, 
            Camera.main.transform.rotation.eulerAngles.y, 
            0
        );
    }

    public bool CheckRecipe(Item[] items) {
        List<int> copy = new List<int>(recipe);

        foreach (Item item in items) {
            if(item == null) { continue; }

            if (!copy.Remove(item.id)) { return false; }
        }

        return copy.Count == 0;
    }
}