using System.Collections.Generic;
using UnityEngine;

public class NPSMovePoints : MonoBehaviour
{
    public static NPSMovePoints PointsInstance { get; private set; }
    [field: SerializeField] public List<Transform> PointsToMove { get; private set; }
    [field: SerializeField] public Transform PointToDialogue { get; private set; }
    [field: SerializeField] public Transform PointToExit { get; private set; }

    private void Awake()
    {
        if (PointsInstance != null && PointsInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            PointsInstance = this;
        }
    }

    public Transform TakeRandPoint()
    {
        if (PointsToMove.Count > 0) return PointsToMove[Random.Range(0, PointsToMove.Count)];
        if (PointToExit != null) return PointToExit;

        else return null;
    }


}
