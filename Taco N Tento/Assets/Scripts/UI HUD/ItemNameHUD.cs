using TMPro;
using UnityEngine;

public class ItemNameHUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ItemNameTxt;

    public void Start()
    {
    ItemNameTxt.text = "Test";
        
    }
    
    public void DisplayItemName(string itemName)
    {

    }
   
}
