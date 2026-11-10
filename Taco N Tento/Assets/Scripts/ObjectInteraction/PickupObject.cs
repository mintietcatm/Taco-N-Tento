using UnityEngine;
using UnityEngine.InputSystem;

public class PickupObject : MonoBehaviour
{
    [SerializeField] private Transform handPoint; //punto de donde la recojo
    [SerializeField] private LayerMask placeableLayer;

    [SerializeField] private Material validMaterial;
    [SerializeField] private Material invalidMaterial;
    private Renderer pickedObjectRenderer;
    private Material originalMaterial = null;  //el material original

    private InteractableData datosInteractuables = null;
    private TacoAssembly tacoDetectado = null;
    float alturaFinal;

    private GameObject pickedObject = null; // que tenemos en la mano
    private Rigidbody pickedObjectRb = null;
    private BoxCollider pickedObjectCollider = null; // El collider del objeto que estamos agarrando
    PlacementState currentState = PlacementState.EmptyHanded; //pa que el jugador empieze con las manos vacias

    [SerializeField] private RaycastDetector raycastDetector; //referenciamos el raycast de RaycastDetector para no tener que hacer otro aqui
    [SerializeField] private float rotationVal = 15f;




    private void Awake()
    {
        placeableLayer = LayerMask.GetMask("PlaceableSurface");
    }


    void Update()
    {
        switch (currentState)
        {
            case PlacementState.EmptyHanded:

                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    TryPickUp();
                }

                break;
            case PlacementState.Holding:
                Debug.Log("Holding");
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    Drop();
                }
                if (Mouse.current.rightButton.wasPressedThisFrame)
                {
                    StartPlacing();
                }
                break;

            case PlacementState.Placing:
                Debug.Log("Placing");
                if (Mouse.current.rightButton.wasPressedThisFrame)
                {
                    CancelPlacing();
                    break;
                }
                RotateObject();
                RaycastHit hit;

                if (raycastDetector.DetectSurface(placeableLayer, out hit))
                {
                    pickedObject.transform.position = hit.point + Vector3.up * pickedObjectCollider.bounds.extents.y;

                    bool canPlace = CheckCollisions(hit); // llamamos al metodo que revisa los colliders

                    if (tacoDetectado != null)
                    {
                        Collider tortillaCollider = tacoDetectado.GetComponent<Collider>(); //saca el collider de la tortilla
                        float alturaTortilla = tortillaCollider.bounds.max.y; //para que no se clipee

                        float diferenciaAltura = alturaTortilla - pickedObjectCollider.bounds.min.y; //ok aqui basicamente esa funcion toma el punto de contacto del collider en lugar del pivote. Para que la carne no quede ni clipeada a la tortilla ni flotando
                        Vector3 nuevaPosicion = pickedObject.transform.position;

                        nuevaPosicion.y += diferenciaAltura; //le añadimos la diferencia a la nueva posicion
                        pickedObject.transform.position = nuevaPosicion;
                       

                    }

                    UpdateMaterial(canPlace); // llamamos al metodo que tiene los materiales

                    if (Mouse.current.leftButton.wasPressedThisFrame && canPlace == true)
                    {
                        ConfirmPlacement();
                    }
                    Debug.Log("colocar " + canPlace);
                    //Debug.Log(hit.point);

                }

                break;

            default:
                return;

        }
    }

    #region PlaceObject, sistema de colocar objetos, poner material bool true y material falso. 

    private void RotateObject() //para rotar con el mouse
    {
        float scroll = Mouse.current.scroll.ReadValue().y;

        if (scroll != 0)
        {
            pickedObject.transform.Rotate(0, scroll * rotationVal, 0, Space.World); // el mouse tiene 3 valores. Solamente lo vamos a rotar en Y por eso x esta en 0 igual que z
            // ( 0 - es el valor de x, scroll es el valor del mousewheel * rptationVal - esto es el valor de la y donde va a rotar el item, 0 - es el valor de z, 
            //Space.World le aplica la transformacion a los ejes del mundo no del game object)
        }

        Debug.Log("Scroll: " + scroll);
    }

    private void TryPickUp()
    {

        if (raycastDetector.Hit())
        {
            RaycastHit hit = raycastDetector.GetHit(); // usando el raycast de RaycastDectector
            if (hit.collider.CompareTag("Interactuable") && hit.collider.GetComponent<Pickupable>() != null)
            {

                PickUp(hit.collider.gameObject);
            }
            Debug.Log(hit.collider.gameObject.name);

        }
    }

    private void PickUp(GameObject objectToPickUp)
    {
        pickedObject = objectToPickUp;
        Pickupable porcion = pickedObject.GetComponent<Pickupable>();

        if (porcion != null)
        {
            porcion.QuitarDelTaco();
        } 
            

        datosInteractuables = pickedObject.GetComponent<InteractableData>();

        if (datosInteractuables != null)
        {
            if (datosInteractuables.ObtenerDatos() is IngredientData ingrediente)
            {
                Debug.Log(ingrediente.ObtenerNombre());
            }
        }


        pickedObjectRb = pickedObject.GetComponent<Rigidbody>();
        pickedObjectCollider = pickedObject.GetComponent<BoxCollider>();
        pickedObjectRenderer = pickedObject.GetComponent<Renderer>();
        originalMaterial = pickedObjectRenderer.material; //Aqui se guarda el material original del objeto, para no tener que asignarlo manualmente en el inspector



        if (pickedObjectRb != null)
        {
            pickedObjectRb.linearVelocity = Vector3.zero; //representa el cambio de la posicion, la velocidad en xyz ahora es 0
            pickedObjectRb.angularVelocity = Vector3.zero;

            if (!pickedObjectRb.isKinematic)
            {

                pickedObjectRb.isKinematic = true; //Controla si la fisica afecta al rb del objeto

                pickedObjectRb.detectCollisions = false;// Para que no mande a volar las otras cosas. Ya. ya me canse de que eso pase. 
            }

        }

        pickedObject.transform.SetParent(handPoint);
        pickedObject.transform.localRotation = Quaternion.identity;
        currentState = PlacementState.Holding;

        pickedObject.transform.localPosition = Vector3.zero;
        //pickedObject.transform.localRotation = Quaternion.identity;
    }

    private void Drop()
    {
        pickedObject.transform.SetParent(null); //deja de ser hijo de la mano. sueltas la tortilla

        if (pickedObjectRb != null) //comprueba si tiene un rigidbody
        {
            pickedObjectRb.isKinematic = false; //reactivamos las fisicas del objeto para que se caiga
            pickedObjectRb.detectCollisions = true;
        }
        pickedObject = null;
        pickedObjectRb = null; //Para que el codigo no se quede con la informacion
        pickedObjectCollider = null;
        currentState = PlacementState.EmptyHanded;
        Debug.Log("Estado actual: " + currentState);
    }


    public bool IsPlacing() // para referenciar el estado placing en otros codigos como el del UI
    {
        return currentState == PlacementState.Placing;
    }


    private bool CheckCollisions(RaycastHit hit)
    {
        tacoDetectado = null; //limpiamos referencia
        Vector3 overlapCenter = pickedObject.transform.TransformPoint(pickedObjectCollider.center); //Donde esta el centro en coordenadas del mundo (por eso Vector3) 
        Vector3 halfExtents = Vector3.Scale(pickedObjectCollider.size, pickedObject.transform.lossyScale) / 2; //lossyScale considera la escala aproximada final en el mundo, no de los padres

        Collider[] overlaps = Physics.OverlapBox(overlapCenter, halfExtents, pickedObject.transform.rotation);

        foreach (Collider collider in overlaps) // FOR EACH por cada COLLIDER elemento de tipo Collider durante esta vuelta COLLIDER le voy a llamar collider IN  dentro del array
        {
            if (collider == pickedObjectCollider)
            {
                continue; //continue literalmente le dice al codigo que siga avanzando
            }

            if (collider == hit.collider)
            {
                continue;
            }

            TacoAssembly taco = collider.GetComponent<TacoAssembly>();

            if (taco != null && datosInteractuables != null && datosInteractuables.ObtenerDatos() is IngredientData)
            {
                Debug.Log("Hola soy una tortilla");
                tacoDetectado = taco;
                continue;
            }
            return false;

        }
        return true;

    }

    private void ConfirmPlacement()
    {
        if (tacoDetectado != null && datosInteractuables != null)
        {
            if (datosInteractuables.ObtenerDatos() is IngredientData ingrediente)
            {
                tacoDetectado.AgregarIngrediente(ingrediente,pickedObject);
                Pickupable porcion = pickedObject.GetComponent<Pickupable>();
                if (porcion != null)
                {
                    porcion.AsignarTaco(tacoDetectado);
                }
            }
        }
        currentState = PlacementState.EmptyHanded;
        if (pickedObject != null)
        {
            pickedObjectRb.isKinematic = true;

            pickedObjectRb.detectCollisions = true;
        }
        pickedObjectRenderer.material = originalMaterial;
        pickedObject = null;
        pickedObjectRb = null;
        pickedObjectCollider = null;
        pickedObjectRenderer = null;
    }

    private void UpdateMaterial(bool canPlace) //Solo pasa SI se puede poner.
    {
        if (canPlace)
        {
            pickedObjectRenderer.material = validMaterial;
        }
        else
        {
            pickedObjectRenderer.material = invalidMaterial;
        }
    }
    private void StartPlacing()
    {
        pickedObject.transform.SetParent(null);
        currentState = PlacementState.Placing;
    }

    private void CancelPlacing()
    {
        pickedObject.transform.SetParent(handPoint);
        pickedObject.transform.localPosition = Vector3.zero;
        pickedObjectRenderer.material = originalMaterial;
        currentState = PlacementState.Holding;
    }

    #endregion

    #region Armado de Tacos

    private TacoAssembly DetectarTaco()
    {
        Vector3 origen = pickedObject.transform.position;
        RaycastHit hit;

        if (Physics.Raycast(origen, Vector3.down, out hit, 0.5f))
        {
            Debug.Log(hit.collider.gameObject.name);
        }

        return null;
    }

    #endregion

}


