using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("�������� ���������� ���������")]
    public float speed = 7f;

    [Header("�������� ���� ���������")]
    public float runSpeed = 7f;

    [Header("���� ������")]
    public float junpPower = 300f;  

    [Header("�������� �� ����� �� ��")]
    public bool ground;


    public Rigidbody rb;
    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                transform.localPosition += transform.forward * runSpeed * Time.deltaTime;
            }
            else
            {
                transform.localPosition += transform.forward * speed * Time.deltaTime;
            }    
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.localPosition += -transform.forward * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.localPosition += -transform.right * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.localPosition += transform.right * speed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (ground == true)
            {
                rb.AddForce(transform.up * junpPower);
            }    
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            ground = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
         if (collision.gameObject.tag == "Ground")
         {
             ground = true;
         }
        }
    }
