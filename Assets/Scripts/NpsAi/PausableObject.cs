using UnityEngine;

public class PausableObject : MonoBehaviour
{
    public static bool OnPause;

    protected virtual void Update()
    {
        if (OnPause) return;
    }
}   