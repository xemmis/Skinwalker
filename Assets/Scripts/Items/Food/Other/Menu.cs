using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public static Menu MenuInstance { get; private set; }
    [field: SerializeField] public List<GameObject> MenuList { get; private set; }


    private void Awake()
    {
        if (MenuInstance == null)
        {
            MenuInstance = this;
        }
        else Destroy(gameObject);
    }


}
