using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float animSpeed = 1.5f;              
    public float lookSmoother = 3.0f;           

    public float rotateSpeed = 0.01f;
    
    private Rigidbody rb;

    private Animator anim;                          
    private AnimatorStateInfo currentBaseState;
    
    public int linha = 0;
    public int coluna = 0;
    float v = 0;
    float h = 0;

    public float rotatedAngle = 0;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        anim.SetFloat("Speed", 0);
    }

    private void FixedUpdate()
    {
        anim.SetFloat("Speed", v);
        //anim.SetFloat("Direction", h);

        rotatedAngle += Mathf.Abs(h * rotateSpeed);
        transform.Rotate(0, h * rotateSpeed, 0);
    }


    public void CorrigirMovimento()
    {
        transform.Rotate(0, (rotatedAngle - 90) * Mathf.Abs(h) / h * -1, 0);
    }

    public void moverParaFrente()
    {
        v = 0.2f;
    }

    public void moverParaTras()
    {
        v = -0.2f;
    }

    public void virarEsquerda()
    {
        h = -0.2f;
    }

    public void virarDireita()
    {
        h = 0.2f;
    }

    public void parar()
    {
        rotatedAngle = 0;
        v = 0;
        h = 0;
    }
}
