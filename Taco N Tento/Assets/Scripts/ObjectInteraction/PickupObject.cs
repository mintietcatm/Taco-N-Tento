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

    private CuttingBoard tablaDetectada = null;
    float alturaFinal;

    private CookingStation comalDetectado = null;

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
                else if (Mouse.current.rightButton.wasPressedThisFrame)
                {
                    TryRefillContainer();
                }

                break;
            case PlacementState.Holding:
                Debug.Log("Holding");
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    if (!TryCut()) //para que revise si vamos a cortar algo antes de droppearlo
                    {
                        Drop();
                    }
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
                    float alturaDesdePivote = (pickedObjectCollider.size.y / 2f - pickedObjectCollider.center.y) * pickedObject.transform.lossyScale.y;

                    pickedObject.transform.position = hit.point + Vector3.up * alturaDesdePivote;
                    tablaDetectada = hit.collider.GetComponent<CuttingBoard>(); //si es una tabla
                    comalDetectado = hit.collider.GetComponent<CookingStation>(); //si es un comal

                    Debug.Log("Detectando " + hit.collider.gameObject.name);

                    bool canPlace = CheckCollisions(hit); // llamamos al metodo que revisa los colliders


                    Debug.Log("Colocar:" + canPlace);

                    ProductInstance producto = pickedObject.GetComponent<ProductInstance>();

                    if (tablaDetectada != null && tablaDetectada.EstaDisponible() == false) //si la existe Y NO esta desocupada
                    {
                        canPlace = false;

                    }

                    if (tablaDetectada != null && producto == null) // si la tabla existe pero el objeto que traemos en la mano no es un Producto
                    {
                        canPlace = false;
                    }

                    ProductData datosProducto = null;

                    if (producto != null)
                    {
                        datosProducto = producto.ObtenerProducto();
                    }

                    if (tablaDetectada != null && (datosProducto == null || !datosProducto.ObtenerSePuedeCortar()))
                    {
                        canPlace = false;
                    }

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
                else
                {
                    tablaDetectada = null;
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
            CuttingBoard tabla = hit.collider.GetComponent<CuttingBoard>();

            if (tabla != null && !tabla.EstaDisponible()) //para que no se pueda recoger la tabla si esta ocupada
            {
                return;
            }

            //dispensador de porciones (empaques basicamente)

            DispensadorDePorciones dispensador = hit.collider.GetComponent<DispensadorDePorciones>(); //buscamos el script en el objeto al que esta tocando el raycast

            if (dispensador != null)
            {
                GameObject porcion = dispensador.ExtraerPorcion(); //intentamos extraer

                if (porcion != null) //si se puede se usa pickup
                {
                    PickUp(porcion);
                }

                return; //se termina TryPickUp para que no se agarre el empaque. 
            }

           
            IngredientContainer recipiente = hit.collider.GetComponent<IngredientContainer>(); //buscamos el script del contenedor para checar si es un contenedor

            if (recipiente != null)
            {
                GameObject porcion = recipiente.ExtraerPorcion();

                if (porcion != null)
                {
                    PickUp(porcion);
                }

                return;
            }

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

        ProductInstance producto = pickedObject.GetComponent<ProductInstance>();

        if (producto != null)
        {
            producto.QuitarDeLaTabla();
        }


        IngredientInstance ingredienteFisico = pickedObject.GetComponent<IngredientInstance>(); //se para la coccion cuando recoges el ingrediente

        if (ingredienteFisico != null)
        {
            ingredienteFisico.DetenerCoccion();
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

            pickedObjectRb.isKinematic = true; //Controla si la fisica afecta al rb del objeto

            pickedObjectRb.detectCollisions = false;// Para que no mande a volar las otras cosas. Ya. ya me canse de que eso pase. 

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

        //FOREACH DEL COLLIDER 

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


            Debug.Log("Colocar se bloquea por " + collider.gameObject.name);
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
                tacoDetectado.AgregarIngrediente(ingrediente, pickedObject);
                Pickupable porcion = pickedObject.GetComponent<Pickupable>();
                if (porcion != null)
                {
                    porcion.AsignarTaco(tacoDetectado);
                }
            }
        }

        if (tablaDetectada != null && pickedObject != null)
        {
            ProductInstance producto = pickedObject.GetComponent<ProductInstance>();

            if (producto != null)
            {
                ProductData datosProducto = producto.ObtenerProducto();

                if (datosProducto != null && datosProducto.ObtenerSePuedeCortar())
                {
                    if (tablaDetectada.ColocarProducto(producto))
                    {
                        producto.AsignarLaTabla(tablaDetectada);
                    }

                }

            }
        }

        currentState = PlacementState.EmptyHanded;
        if (pickedObject != null)
        {
            pickedObjectRb.isKinematic = true;

            pickedObjectRb.detectCollisions = true;
        }

        if (comalDetectado != null && pickedObject != null) //checar si lo estamos poniendo en un comal
        {
            IngredientInstance ingrediente = pickedObject.GetComponent<IngredientInstance>();

            if (ingrediente != null)
            {
                comalDetectado.ColocarIngrediente(ingrediente);
            }
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


    #region Cuchillo, tabla de cortar y relacionado.

    private bool TryCut()
    {
        if (pickedObject.GetComponent<Knife>() == null) //no traemos un cuchillo?????
        {
            return false; //va pues no
        }

        if (!raycastDetector.Hit())//no estamos apuntando a nada???
        {
            return false; // va pues tampoco
        }
        RaycastHit hit = raycastDetector.GetHit();
        CuttingBoard tabla = hit.collider.GetComponent<CuttingBoard>();

        if (tabla == null) //no detectamos una tabla?????
        {
            ProductInstance producto = hit.collider.GetComponent<ProductInstance>(); //pues buscamos un producto

            if (producto != null) //ah pero si detectamos un producto
            {
                tabla = producto.ObtenerTabla(); //ps buscamos la tabla
            }
        }

        if (tabla != null) //si hay tabla?????
        {
            tabla.Cortar(); //va pues cortamos
            return true;
        }

        return false; //no se cumplio ni una de las condiciones. pos no ejecutamos nada
    }

    #endregion

    #region Contenedor e inventario

    private void TryRefillContainer()
    {
        if (!raycastDetector.Hit()) //si no hay nada
        {
            return;
        }

        RaycastHit hit = raycastDetector.GetHit(); //referenciamos el raycastDetector

        IngredientContainer recipiente = hit.collider.GetComponent<IngredientContainer>();

        if (recipiente != null)
        {
            recipiente.RellenarRecipiente();
        }

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


