using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseMovement : CharacterBase
{
    [Header("Camera Variables")]
    [SerializeField] private Camera _playerCam;
    [SerializeField] private float _zRayDepth = 10f;

    [Space(12)]
    [Header("Movement Parameters")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotateSpeed = 2f;
    [SerializeField] private bool moving = false;

    [Space(12)]
    [Header("Layermasks")]
    private LayerMask _plyLayer;
    private LayerMask _walkable;
    private LayerMask _obstructed;

    private Vector3 _wishMovePoint;
    private Vector3 _finalMovePoint; // The resultant Vector3 after all checks and calculations are done
    private Vector3 _lookTarget;

    private void OnEnable()
    {
        _wishMovePoint = transform.position;
        _finalMovePoint = transform.position;
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Mouse clicked!");
            _wishMovePoint = GetWorldMoveTarget();
            _lookTarget = GetTargetDirection(_wishMovePoint);
            _finalMovePoint = CalculateMoveWish(_wishMovePoint);
        }
        
        if (transform.position != _finalMovePoint)
        {
            Quaternion direction = Quaternion.LookRotation(_lookTarget, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, direction, rotateSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, _finalMovePoint, moveSpeed * Time.deltaTime);
        }
    }

    private Vector3 GetWorldMoveTarget()
    {
        Debug.Log("");
        Debug.Log("Getting world move target...");
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = _zRayDepth;
        Ray findPointRay = _playerCam.ScreenPointToRay(mousePos);

        Debug.Log("Casting ray...");
        if (Physics.Raycast(findPointRay, out RaycastHit hit))//, ~_plyLayer))
        {
            Debug.Log("Hit!");
            Vector3 offsetPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);

            return offsetPos;
        }
        else
        {
            Debug.Log("No hit!");
            return Vector3.zero;
        }
    }

    private Vector3 CalculateMoveWish(Vector3 p_MoveTarget)
    {
        p_MoveTarget.y = transform.position.y;
        Debug.Log("");
        Debug.Log("Calculating move wish");

        Ray r = new Ray(transform.position, p_MoveTarget);

        if (Physics.Raycast(r, out RaycastHit hit, 100f, ~_plyLayer))
        {
            Debug.Log("Hit something between current pos and wish move pos");

            Vector3 newMovePos = hit.point;
            newMovePos = hit.point;
            newMovePos.y = transform.position.y;

            newMovePos = r.GetPoint(hit.distance - 1f);

            return newMovePos;
        }
        else
        {
            Debug.Log("Didn't hit anything");
            return p_MoveTarget;
        }
    }

    private void CalculateObstructedPath(Vector3 p_MoveTarget)
    {

    }

    private Vector3 GetTargetDirection(Vector3 target)
    {
        return target - transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, _finalMovePoint);
        Gizmos.DrawWireSphere(_finalMovePoint, 0.25f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(_finalMovePoint, _wishMovePoint);
        Gizmos.DrawWireSphere(_wishMovePoint, 0.25f);
    }
}