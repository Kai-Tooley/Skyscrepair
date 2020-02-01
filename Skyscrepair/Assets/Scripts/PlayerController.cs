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
        //input.actions = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().inputActions;
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

    //private void Look(Vector2 rotate)
    //{
    //    if (rotate.sqrMagnitude < 0.01)
    //    {
    //        return;
    //    }
    //    float scaledRotateSpeed = rotateSpeed * Time.deltaTime;
    //    if (rotate.x > 0.1f)
    //    {
    //        transform.Rotate(Vector3.up, 1 * scaledRotateSpeed);
    //    }
    //    else if (rotate.x < 0.1f)
    //    {
    //        transform.Rotate(Vector3.up, -1 * scaledRotateSpeed);
    //    }
    //}

    public void OnMove(InputAction.CallbackContext context)
    {
        m_Move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        m_Look = context.ReadValue<Vector2>();
    }
}
