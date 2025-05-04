using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float speed = 10.0f;
    int curPathIndex = 0;

    List<Vector3> path;

    private void Update()
    {
        HandleMovement();

        if (Input.GetMouseButtonDown(0)) 
        {
            SetTargetPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    public void SetTargetPosition(Vector3 targetPosition) 
    {
        curPathIndex = 0;
        path = BuildingGrid.instance.FindPath(transform.position, targetPosition);

        if (path != null && path.Count > 1) 
        { 
            path.RemoveAt(0);
        }
    }

    private void HandleMovement() 
    {
        if (path != null)
        {
            Vector3 targetPosition = path[curPathIndex];
            if (Vector3.Distance(transform.position, targetPosition) > 0.05f)
            {
                Vector3 moveDir = (targetPosition - transform.position).normalized;
                transform.position = transform.position + moveDir * speed * Time.deltaTime;
            }
            else
            {
                curPathIndex++;
                if (curPathIndex >= path.Count)
                {
                    StopMoving();
                }
            }
        }
    }

    private void StopMoving() 
    {
        path = null;
    }
}
