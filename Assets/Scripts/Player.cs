using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("플레이어 기본 움직임")]
    private Rigidbody2D m_rigid;
    private BoxCollider2D m_box2d;
    private Animator m_anim;


    [SerializeField] private bool m_isGrounded;
    [SerializeField] private float m_moveSpeed;
    private Vector3 m_moveDir;

    private float m_verticalVelocity = 0f;
    private float m_gravity = 9.81f;
    private float m_maxFallingSpeed = -10f;
    [SerializeField]private float m_jumpPower = 10f;

    [SerializeField] private bool m_isJump = false;
    private bool _doJump = false;
    private bool DoJump //1차 보안
    {
        get => _doJump;
        set
        {
            _doJump = value;
            m_anim.SetBool("Jump", value);
        }
    }

    private bool damaged = false;
    private float invincibilityTimer = 0f;
    [SerializeField] private float invincibilityTime = 2f;
    private SpriteRenderer[] m_Sr;
    private bool _invincibility = false;
    private bool Invincibility
    {
        get => _invincibility;
        set
        {
            _invincibility = value;
            if (value == true)
            {
                setAlpha(0.5f);
            }
            else
            {
                setAlpha(1.0f);
            }
        }
    }

    [Header("플레이어 무기투척")]
    [SerializeField] private Transform trsHand; //회전을 적용할 손
    [SerializeField] private Transform trsWeaponOriginal; //원본
    [SerializeField] private Transform trsGameObject; //오브젝트
    [SerializeField] private GameObject m_objWeapon; //프리팹
    private bool right = false; //쳐다보는 방향

    [Header("플레이어 벽 점프")]
    private bool m_wallJump = false;
    private bool m_doWallJump = false;
    private bool m_doWallJumpTimer = false;
    private float m_wallJumpTimer = 0.0f;
    private float m_wallJumpTime = 0.3f;

    [Header("플레이어 대쉬")]
    private bool m_dash = false;
    private float m_dashTimer = 0f;
    private float m_dashTime = 0.2f;
    private TrailRenderer m_tDashEffect;

    void Start()
    {
        m_rigid = GetComponent<Rigidbody2D>();
        m_box2d = GetComponent<BoxCollider2D>();
        m_anim = GetComponent<Animator>();
        m_Sr = GetComponentsInChildren<SpriteRenderer>(); //본인도 포함해서 검색한다.
        m_tDashEffect = GetComponent<TrailRenderer>();

        //A에서 B까지 이동
        //Vector3.SmoothDamp()
        //Vector3.Lerp()
    }

    void Update()
    {
        CheckGrounded();
        moving();
        jumping();
        checkInvincibility();
        checkAnim();
        shoot();
        checkDoWallJumpTimer();
        checkDash();
        checkCamera();
        checkGravity();
    }

    private void checkCamera()
    {
        Camera.main.transform.position = new Vector3(transform.position.x,transform.position.y,-10f);
    }
    private void CheckGrounded() //땅에 붙어있는지 확인하는 함수
    {
        bool beforeGround = m_isGrounded;
        m_isGrounded = false;

        if(m_verticalVelocity > 0) //올라가는 중이면 바로 리턴
        {
            return;
        }
        RaycastHit2D hit = Physics2D.BoxCast(m_box2d.bounds.center,
            m_box2d.bounds.size, 0f, Vector3.down, 0.1f, LayerMask.GetMask("Ground", "Trap")); //Get Mask 장점
        if (hit) //hit의 조건문
        {
            m_isGrounded = true;
            if (beforeGround == false && DoJump == true)
            {
                DoJump = false;
            }

            if (Invincibility == false && hit.transform.gameObject.layer == LayerMask.NameToLayer("Trap"))
            {
                damaged = true;
            }
        }
    }

    private void moving() //움직이는 함수
    {

        if (m_doWallJumpTimer == true || m_dash == true)
        {
            return;
        }

        m_moveDir.x = Input.GetAxisRaw("Horizontal");
        m_anim.SetBool("run", m_moveDir.x != 0f);

        //if (m_moveDir.x == -1 && transform.localScale.x != 1.0f)//왼쪽을 누르고 있음, 왼쪽을 쳐다보게 해야함
        //{
        //    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        //}
        //else if (m_moveDir.x == 1)
        //{
        //    if (transform.localScale.x != -1.0f)
        //    {
        //        transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        //    }
        //}

        m_rigid.velocity = m_moveDir * m_moveSpeed;
    }

    private void jumping()
    {
        if (m_isGrounded == false)
        {
            if(Input.GetKeyDown(KeyCode.Space) && m_wallJump == true && m_moveDir.x != 0)
            {
                m_doWallJump = true;
            }
        }
        else {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_isJump = true;
            }
        }
    }

    private void checkGravity()
    {
        if (m_dash ==true)
        {
            return;
        }

        if (m_doWallJump == true)
        {
            Vector2 jumpDir = m_rigid.velocity;
            jumpDir.x *= -1;
            m_rigid.velocity = jumpDir;
            m_verticalVelocity = m_jumpPower * 0.5f;
            m_doWallJump = false;
            m_doWallJumpTimer = true;
        }
        if (m_isGrounded == false)//공중에 떠있는 상태
        {
            m_verticalVelocity -= m_gravity * Time.deltaTime;
            if (m_verticalVelocity < m_maxFallingSpeed)
            {
                m_verticalVelocity = m_maxFallingSpeed;
            }
        }
        else//땅에 서있는 상태
        {
            if (damaged == true)
            {
                damaged = false;
                Invincibility = true;
                m_verticalVelocity = m_jumpPower * 0.5f; //예전엔 나눗셈보단 곱셈을 선호               
            }
            else if (m_isJump == true)
            {
                m_isJump = false;
                DoJump = true;
                m_verticalVelocity = m_jumpPower;
            }
            else
            {
                m_verticalVelocity += m_gravity * 3 * Time.deltaTime;
                if (m_verticalVelocity > 0)
                {
                    m_verticalVelocity = 0;
                }
            }
        }

        m_rigid.velocity = new Vector2(m_rigid.velocity.x, m_verticalVelocity);
    }

    private void setAlpha(float _alphavalue)
    {
        int count = m_Sr.Length;
        for (int iNum = 0; iNum < count; iNum++)
        {
            SpriteRenderer spriteRenderer = m_Sr[iNum];
            Color color = spriteRenderer.color;
            color.a = _alphavalue;
            spriteRenderer.color = color;
        }
    }

    private void checkInvincibility()
    {
        if (Invincibility == true)
        {
            invincibilityTimer += Time.deltaTime;
            if (invincibilityTimer >= invincibilityTime)
            {
                Invincibility = false;
                invincibilityTimer = 0.0f;
            }
        }
    }

    private void checkAnim()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        //x,y는 mouse position값을 넣고 , z값은 카메라의 z만큼 빼줘야함

        Vector3 newPos = mouseWorldPos - transform.position;

        if (newPos.x > 0 && transform.localScale.x != -1.0f)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            right = true;
        }
        else if (newPos.x < 0 && transform.localScale.x != 1.0f)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            right = false;
        }
        Vector3 direction = right == true ? Vector3.right : Vector3.left;
        float angle = Quaternion.FromToRotation(direction, newPos).eulerAngles.z;

        angle = right == true ? -angle : angle;

        trsHand.localEulerAngles = new Vector3(transform.localEulerAngles.x,transform.localEulerAngles.y,angle);
    }

    private void shoot()
    {
        if (Input.GetMouseButtonDown(0)) //0 마우스 왼쪽 , 1 마우스 오른쪽 , 휠 클릭 2번
        {
            GameObject obj = Instantiate(m_objWeapon, trsWeaponOriginal.position, trsWeaponOriginal.rotation, trsGameObject);
            Weapon weapon = obj.GetComponent<Weapon>();
            Vector2 force = new Vector2(right == true ? 40.0f : -40.0f, 1.0f); //
            weapon.SetForce(trsWeaponOriginal.rotation * force, right); //프로시저가 우선 순위
            //trsWeaponOriginal.rotation * force 각도 * 백터2 (방향) 이해하기.
        }
    }

    public void OnTrigger(HitBox.eHitBoxState _state, HitBox.eHitType _type, Collider2D _collision)
    {
        switch(_state)
        {
            case HitBox.eHitBoxState.Enter:
                {
                    switch (_type)
                    {
                        case HitBox.eHitType.WallCheck:

                            m_wallJump = true;

                            break;
                        case HitBox.eHitType.Hit:
                            if (_collision.gameObject.tag == "Item")
                            {
                                Item item = _collision.GetComponent<Item>();
                                if (item != null)
                                {
                                    item.GetItem();
                                }
                                else
                                {
                                    Debug.Log("Item 스크립트가 없습니다");
                                }
                            }
                            break;
                    }
                }
                break;
            case HitBox.eHitBoxState.Exit:
                {
                    switch (_type)
                    {
                        case HitBox.eHitType.WallCheck:

                            m_wallJump = false;
                            
                            break;

                        case HitBox.eHitType.Hit:


                            break;
                    }
                }
                break;
            case HitBox.eHitBoxState.Stay:
                {
                    switch (_type)
                    {
                        case HitBox.eHitType.WallCheck:


                            break;

                        case HitBox.eHitType.Hit:


                            break;
                    }
                }
                break;
        }
    }

    private void checkDoWallJumpTimer()
    {
        if (m_doWallJumpTimer == true)
        {
            m_wallJumpTimer += Time.deltaTime;
            if (m_wallJumpTimer >= m_wallJumpTime)
            {
                m_wallJumpTimer = 0f;
                m_doWallJumpTimer = false;
            }
        }
    }

    private void checkDash()
    { 
        if (Input.GetKeyDown(KeyCode.LeftShift) && m_dash == false)
        {
            m_dash = true;
            m_verticalVelocity = 0f; 
            m_rigid.velocity = new Vector2(right == true ? 20.0f : -20.0f, 0.0f);
            transform.Rotate(new Vector3(0, 0, right == true ? -45f : 45));
            m_tDashEffect.enabled = true;
        }
        else if (m_dash == true)
        {
            m_dashTimer += Time.deltaTime;
            if (m_dashTimer >= m_dashTime)
            {
                m_dashTimer = 0f;
                m_rigid.rotation = 0f;
                m_dash = false;
                m_tDashEffect.enabled = false;
                m_tDashEffect.Clear();
            }
        }
    }

    
}


//0530 점프기능과 트랩기능을 추가
//0531 무기 추가 및 무기 투척기능 추가 프리팹 수정 시 apply to prefab // 원본을 누르고 Revert를 누르면 프리팹 초기상태로 돌아감
//ViewportPoint 카메라에 보이는 비율로 위치를 파악 (0~1), ScreenPoint: 해상도에 비례하여 위치를 파악 화면 중심이 (0,0)이다 ex) 1920x1080 = -960~960  //WorldPoint : 월드 기준 포지션
