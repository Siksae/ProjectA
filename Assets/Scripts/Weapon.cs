using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Rigidbody2D m_rigid;
    private BoxCollider2D m_box2D;
    private Vector2 m_force;
    private bool m_right;
    [SerializeField] private LayerMask m_checkGround;
    public void SetForce(Vector2 _force, bool _right)
    {
        m_force = _force;
        m_right = _right;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_rigid = GetComponent<Rigidbody2D>();
        m_rigid.AddForce(m_force,ForceMode2D.Impulse); //Impulse(Æø¹ßÀûÀÎ Èû), force(±×³É ÈûÀ» °¡ÇÔ)

        Destroy(gameObject, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, m_right == true ? -360f : 360f) * Time.deltaTime);

        if (m_box2D.IsTouchingLayers(m_checkGround) == true)
        {
            m_rigid.velocity = new Vector2(m_rigid.velocity.x * -1, m_rigid.velocity.y * -1);
        }
    }
}
