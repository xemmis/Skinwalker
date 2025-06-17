using UnityEngine;

public class CucumberSliceLogic : MonoBehaviour
{
    [SerializeField] private GameObject _slicePrefab;
    [SerializeField] private int _sliceAmount = 5;
    [SerializeField] private bool _isSliced = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Knife") && !_isSliced)
        {
            _isSliced = true;
            for (int i = 0; i < _sliceAmount; i++)
            {
                Instantiate(_slicePrefab, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}

