using UnityEngine;
using UnityEngine.InputSystem;

public class HeadbobSystem : MonoBehaviour
{
    //Revisar como declarar usando nuevo input system


    [Range(0.001f, 0.1f)]
    public float Amount = 0.002f;

    [Range(1f, 30f)]
    public float Frequency = 10.0f;

    [Range(10f, 100f)]
    public float Smooth = 10.0f;

    private Vector3 StartPos;


    void Start()
    {
        StartPos = transform.localPosition;
    }


    void Update()
    {
        CheckForHeadbobTrigger();
        StopHeadBob();
    }

    private void CheckForHeadbobTrigger()
    {
        if (Keyboard.current == null) return;

        Vector2 movementInput = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) movementInput.y += 1;
        if (Keyboard.current.sKey.isPressed) movementInput.y -= 1;
        if (Keyboard.current.aKey.isPressed) movementInput.x -= 1;
        if (Keyboard.current.dKey.isPressed) movementInput.x += 1;

        if (movementInput.magnitude > 0)
        {
            StartHeadBob();
        }
    }

    private void StartHeadBob()
    {
        Vector3 pos = Vector3.zero;

        pos.y += Mathf.Sin(Time.time * Frequency) * Amount * 1.4f;
        pos.x += Mathf.Cos(Time.time * Frequency / 2f) * Amount * 1.6f;
        transform.localPosition = Vector3.Lerp(transform.localPosition, pos, Smooth * Time.deltaTime);



    }

    private void StopHeadBob()
    {
        if (transform.localPosition == StartPos) return;

        transform.localPosition = Vector3.Lerp(transform.localPosition, StartPos, Time.deltaTime);



    }


}
