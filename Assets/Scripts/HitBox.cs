using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    private Player player;
    [SerializeField] private eHitType m_hitType;

    public enum eHitType
    {
        WallCheck, Hit,
    }
    public enum eHitBoxState
    {
        Enter, Stay, Exit,
    }
    private void Start()
    {
        player = GetComponentInParent<Player>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.OnTrigger(eHitBoxState.Enter, m_hitType, collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        player.OnTrigger(eHitBoxState.Exit, m_hitType, collision);
    }


}
