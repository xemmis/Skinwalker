using UnityEngine;

public class RawMeatBall : Interactable
{
    [SerializeField] private GameObject _cookedMeatBall;
    [SerializeField] private Plate _plate;

    [SerializeField] private float _cookTime;
    private bool _isCooking;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Plate>(out Plate component))
        {
            if (!_isCooking)
            {
                _isCooking = true;
                _plate = component;
                component.OnMealCooked += CookedLogic;
                component.CookMeal(_cookTime);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Plate>(out Plate component))
        {
            if (_isCooking)
            {
                _isCooking = false;
                component.OnMealCooked -= CookedLogic;

            }
        }
    }

    private void CookedLogic()
    {
        Instantiate(_cookedMeatBall, new Vector3(transform.position.x,transform.position.y + .5f,transform.position.z), Quaternion.EulerRotation(90,0,0));
        _plate.OnMealCooked -= CookedLogic;
        Destroy(gameObject);
    }
}
