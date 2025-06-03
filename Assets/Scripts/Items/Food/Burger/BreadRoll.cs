using UnityEngine;

public class BreadRoll : Interactable
{
    [SerializeField] private GameObject _nextStage;
    private bool _interacted = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<MeatBall>(out MeatBall meatBall) && !_interacted)
        {
            _interacted = true;
            Instantiate(_nextStage,transform.position,Quaternion.identity);
            Destroy(meatBall.gameObject);
            Destroy(gameObject);
        }
    }

}
