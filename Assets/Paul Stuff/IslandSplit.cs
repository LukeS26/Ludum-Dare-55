using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSplit : MonoBehaviour
{
    private Vector3 fleePosition, fleeDirection;
    Transform origin;
    [HideInInspector] public Vector3 startPosition;
    private static float splitDistance;
    public bool split;
    private List<IslandProp> props = new List<IslandProp>();
    // Start is called before the first frame update
    void Start()
    {
        origin = transform.Find("Origin");
        startPosition = transform.position;
        fleePosition = Vector3.zero;
        fleeDirection = (origin.position - fleePosition).normalized;
        fleeDirection = new Vector3(fleeDirection.x, 0f, fleeDirection.z);
    }

    private void Update()
    {
        if (split) Split(Time.deltaTime);
    }

    public static void Split(float distance)
    {
        splitDistance += distance;
        foreach (IslandSplit s in FindObjectsOfType<IslandSplit>(false))
        {
            s.transform.position = s.startPosition + s.GetTotalDisplacement();
        }
    }

    public void AddProp(IslandProp prop)
    {
        props.Add(prop);
    }

    public Vector3 GetTotalDisplacement()
    {
        return fleeDirection * splitDistance;
    }
}
