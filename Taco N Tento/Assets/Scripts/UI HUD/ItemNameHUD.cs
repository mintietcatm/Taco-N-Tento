using TMPro;
using UnityEngine;

public class ItemNameHUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ItemNameTxt;

    public void Start()
    {
    ItemNameTxt.text = "Hola";
        
    }
    
   
}
