using UnityEngine;
using UnityEngine.InputSystem;

public class PickupObject : MonoBehaviour
{
    [SerializeField] private Transform handPoint; //punto de donde la recojo
    [SerializeField] private Camera playerCamera; // la camara del jugador para lanzar el Raycast
    [SerializeField] private float pickupDistance = 3f; // que tan lejos puede llegar el jugador para recoger algo

    private GameObject pickedObject = null; // que tenemos en la mano

    void Update()
    {
        if (pickedObject == null)
        {
            //puedes recoger cosas. tienes las manos vacias
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryPickUp();
        }

        }

        else
        {
            //ya tienes algo en la mano

            Drop();
        }
    }

    private void TryPickUp()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position,playerCamera.transform.forward,out hit,pickupDistance))
        {
            if (hit.collider.CompareTag("Ingrediente"))
            {
                PickUp(hit.collider.gameObject);
            }
            Debug.Log(hit.collider.gameObject.name); 
            
        }
    }

    private void PickUp(GameObject objectToPickUp)
    {
        objectToPickUp.transform.SetParent(handPoint);
        objectToPickUp.transform.localPosition = Vector3.zero;

        pickedObject = objectToPickUp;
    }

   private void Drop()
    {
        pickedObject.transform.SetParent(null);
        pickedObject = null;
    }
}
