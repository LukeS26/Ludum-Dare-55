using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandProp : MonoBehaviour
{
    public IslandSplit island;
    private Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        //IslandSplit.Split(Time.deltaTime);
    }

    public void OnTriggerStay(Collider other)
    {
        if (island == null)
        {
            //Debug.Log(name + " -- Found Island");
            bool tryR = other.transform.TryGetComponent(out island);
            if (!tryR) return;
            island.AddProp(this);
            transform.parent = island.transform;
        }
    }

    public Vector3 GetStartPosition()
    {
        return startPosition;
    }
}
