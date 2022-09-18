using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestAStarMove : MonoBehaviour
{
    public Transform target;
    public Camera _playerCam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos.z = 20;
            Ray findPointRay = _playerCam.ScreenPointToRay(mousePos);

            Debug.Log("Casting ray...");
            if (Physics.Raycast(findPointRay, out RaycastHit hit))//, ~_plyLayer))
            {
                target.position = hit.point;
            }
        }
    }
}
