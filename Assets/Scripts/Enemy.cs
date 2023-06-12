using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float m_movingSpeed = 3.0f;
    [SerializeField] private LayerMask m_checkMask;
    [SerializeField] private LayerMask m_checkWall;

    private Rigidbody2D m_rigid;
    private BoxCollider2D m_Box2D;
    
    // Start is called before the first frame update
    void Start()
    {
        m_rigid = GetComponent<Rigidbody2D>();
        m_Box2D = GetComponent<BoxCollider2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        checkFlip();
        m_rigid.velocity = new Vector2(m_movingSpeed, m_rigid.velocity.y);
    }

    private void checkFlip()
    {
        if (m_Box2D.IsTouchingLayers(m_checkMask) == false || m_Box2D.IsTouchingLayers(m_checkWall) == true)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            m_movingSpeed *= -1;
        }
    }




}
