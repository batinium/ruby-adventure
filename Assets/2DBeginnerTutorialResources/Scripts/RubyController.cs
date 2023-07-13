using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A, D, Left, Right
        float vertical = Input.GetAxis("Vertical"); // W, S, Up, Down
        Vector2 position = transform.position;
        position.x = position.x + 3.0f * horizontal *Time.deltaTime;
        position.y = position.y + 3.0f * vertical*Time.deltaTime;
        transform.position = position;
    }
}
