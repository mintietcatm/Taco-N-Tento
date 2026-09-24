using UnityEngine;
using UnityEngine.InputSystem;

public class PickupObject : MonoBehaviour
{
    [SerializeField] private Transform handPoint; //punto de donde la recojo
    [SerializeField] private Camera playerCamera; // la camara del jugador para lanzar el Raycast
    [SerializeField] private float pickupDistance = 3f; // que tan lejos puede llegar el jugador para recoger algo

    private GameObject pickedObject = null; // que tenemos en la mano
    private Rigidbody pickedObjectRb = null;

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
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
              Drop();
            }
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
        pickedObject = objectToPickUp;
        pickedObjectRb = pickedObject.GetComponent<Rigidbody>();

        if (pickedObjectRb != null)
        {
            pickedObjectRb.isKinematic = true; //Controla si la fisica afecta al rb del objeto
            pickedObjectRb.linearVelocity = Vector3.zero; //representa el cambio de la posicion, aqui ando convirtiendolo al vector3 osea x,y,z
            pickedObjectRb.angularVelocity = Vector3.zero;
        }

        pickedObject.transform.SetParent(handPoint);

        pickedObject.transform.localPosition = Vector3.zero;
        //pickedObject.transform.localRotation = Quaternion.identity;
    }

   private void Drop()
    {
        pickedObject.transform.SetParent(null); //deja de ser hijo de la mano

        if (pickedObjectRb != null)
        {
            pickedObjectRb.isKinematic=false; //reactivamos las fisicas del objeto para que se caiga

        }
        pickedObject = null;
    }
}
