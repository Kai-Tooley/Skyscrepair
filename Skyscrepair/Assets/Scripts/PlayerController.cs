using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public InputActionAsset inputSet;
    public Vector2 m_Look;
    public Vector2 m_Move;

    public float weight;

    float moveSpeed = 5f;
    float rotateSpeed = 60f;

    PlayerInput input;

    private void Awake()
    {

        input = GetComponent<PlayerInput>();
        if (input.actions != inputSet)
        {
            input.actions = inputSet;
        }
    }



    void Update()
    {
        Move(m_Move);
        Look(m_Look);
    }


    private void Move(Vector2 direction)
    {

        if (direction.sqrMagnitude < 0.01)
        {
            return;
        }

        float scaledMoveSpeed = moveSpeed * Time.deltaTime;
        Vector3 playerPos = transform.position;

        if (direction.x > 0.1f)
        {
            playerPos.x += (Vector2.right * scaledMoveSpeed).magnitude;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (direction.x < 0.1f)
        {
            playerPos.x -= (Vector2.right * scaledMoveSpeed).magnitude;
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        transform.position = playerPos;
    }

    private void Look(Vector2 rotate)
    {
        //if (rotate.sqrMagnitude < 0.01)
        //{
        //    return;
        //}
        //float scaledRotateSpeed = rotateSpeed * Time.deltaTime;
        //if (rotate.x > 0.1f)
        //{
        //    transform.Rotate(Vector3.back, 1 * scaledRotateSpeed);
        //}
        //else if (rotate.x < 0.1f)
        //{
        //    transform.Rotate(Vector3.forward, 1 * scaledRotateSpeed);
        //}


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
