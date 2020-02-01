using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{ 

    public Vector2 m_Look;
    public Vector2 m_Move;

    float moveSpeed = 10f;
    float rotateSpeed = 60f;

    PlayerInput input;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
    }



    void Update()
    {
        Move(m_Move);
    }


    private void Move(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01)
        {
            return;
        }

        float scaledMoveSpeed = moveSpeed * Time.deltaTime;

        if (direction.x > 0.1f)
        {
            transform.Translate(Vector2.right * scaledMoveSpeed);
        }
        else if (direction.x < 0.1f)
        {
            transform.Translate(Vector2.left * scaledMoveSpeed);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        m_Move = context.ReadValue<Vector2>();
    }

    //This isn't being used
    public void OnLook(InputAction.CallbackContext context)
    {
        m_Look = context.ReadValue<Vector2>();
    }
}
