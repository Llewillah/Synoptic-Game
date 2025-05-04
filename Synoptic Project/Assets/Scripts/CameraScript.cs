using UnityEngine;

public class CameraScript: MonoBehaviour
{

    [SerializeField] float camSpeed;
    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        

        transform.Translate(new Vector3(x, y, 0.0f) * camSpeed);

        Vector3 newPos = transform.position;

        if (transform.position.x > 90) 
        {
            newPos.x = 90f;
        }

        if (transform.position.x < 10) 
        {
            newPos.x = 10;
        }

        if (transform.position.y > 94) 
        {
            newPos.y = 94;
        }

        if (transform.position.y < 6) 
        {
            newPos.y = 6;
        }

        transform.position = newPos;
    }
}
