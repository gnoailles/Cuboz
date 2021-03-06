﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public  float                   inputCooldown       = 0.1f;
    [SerializeField] 
    private ushort m_dashSize          = 2;

    [SerializeField] 
    private bool                    m_restartAtSpawn        = false;
    [SerializeField] 
    private Material                m_defaultMaterial       = null;
    [SerializeField] 
    private Material                m_transparentMaterial   = null;

    private Vector3                 m_lastSafePosition;
    private Vector3                 m_positionBuffer;
    private Animator                m_animator          = null;

    private Vector3                 m_translation;
    private bool                    m_moveNextFrame     = false;
    private bool                    m_isOnFinish        = false;
    private bool                    m_isOnSpikes        = false;
    private bool                    m_isFalling        = false;

    void Start()
    {
        m_lastSafePosition = m_positionBuffer = transform.position;
        m_animator = GetComponent<Animator>();
        if(m_animator == null)
        {
            throw new UnassignedReferenceException("Missing animator!");
        }

        Renderer renderer = GetComponentInChildren<Renderer>();
        renderer.material = m_defaultMaterial;

        //Color color = renderer.material.color;
        //color.a = 1;
        //renderer.material.color = color;

        m_animator.Play("Spawn");
        inputCooldown = m_animator.GetCurrentAnimatorStateInfo(0).length;
    }

    void Update()
    {
        if (transform.position.y <= -5.0f)
            Respawn();

        if(m_moveNextFrame)
        {
            transform.position += m_translation;
            m_moveNextFrame = false;
            if (!m_isFalling && !m_isOnSpikes)
                inputCooldown = 0;
        }
    }

    public void MoveForward()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
        TryMove(Vector3.forward);
    }

    public void MoveBackward()
    {
        transform.eulerAngles = new Vector3(0, 180, 0);
        TryMove(Vector3.back);
    }

    public void MoveRight()
    {
        transform.eulerAngles = new Vector3(0, 90, 0);
        TryMove(Vector3.right);
    }

    public void MoveLeft()
    {
        transform.eulerAngles = new Vector3(0, -90, 0);
        TryMove(Vector3.left);
    }

    public void DashForward()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
        TryMove(Vector3.forward * m_dashSize);
    }

    public void DashBackward()
    {
        transform.eulerAngles = new Vector3(0, 180, 0);
        TryMove(Vector3.back * m_dashSize);
    }

    public void DashRight()
    {
        transform.eulerAngles = new Vector3(0, 90, 0);
        TryMove(Vector3.right * m_dashSize);
    }

    public void DashLeft()
    {
        transform.eulerAngles = new Vector3(0, -90, 0);
        TryMove(Vector3.left * m_dashSize);
    }

    public void Jump()
    {
        m_animator.Play("Jump");
        inputCooldown = m_animator.GetCurrentAnimatorStateInfo(0).length;
    }

    public void Respawn()
    {
        StartCoroutine("FadingDeath");
    }

    private void TryMove(Vector3 p_direction)
    {
        if(m_isFalling || m_isOnSpikes)
            return;
        RaycastHit[] hits = Physics.RaycastAll(transform.position + new Vector3(0,0.5f,0), p_direction, p_direction.magnitude);
        if (hits.Length > 0)
        {
            if(hits.Length > 1 && hits[0].distance > hits[1].distance)
            {
                RaycastHit farHit = hits[0];
                hits[0] = hits[1];
                hits[1] = farHit;
            }
            bool blocked = false;
            foreach(RaycastHit hit in hits)
            {
                switch (hit.collider.tag)
                {
                    case "Wall":
                        if (p_direction.magnitude > 1 && hit.distance > 1)
                        {
                            m_animator.Play("DashB");
                            inputCooldown = m_animator.GetCurrentAnimatorStateInfo(0).length;
                            m_translation = p_direction.normalized;
                        }
                        else
                        {
                            blocked = true;
                        }
                        break;
                    case "Spike":
                        if (p_direction.magnitude > 1 && hit.distance < 1)
                        {
                            m_animator.Play("DashB");
                            inputCooldown = m_animator.GetCurrentAnimatorStateInfo(0).length;
                            m_translation = p_direction.normalized;
                            blocked = true;
                        }
                        else
                        {
                            if (p_direction.magnitude > 1)
                            {
                                m_animator.Play("DashA");
                                inputCooldown = m_animator.GetCurrentAnimatorStateInfo(0).length;
                                m_translation = p_direction;
                            }
                            else
                            {
                                m_animator.Play("Move");
                                inputCooldown = m_animator.GetCurrentAnimatorStateInfo(0).length;
                                m_translation = p_direction.normalized;
                            }
                        }
                        inputCooldown = 1000.0f;
                        m_isOnSpikes = true;
                        break;

                    case "Finish":
                        if(p_direction.magnitude > 1 && hit.distance < 1)
                        {
                            if(hits.Length > 1 && hits[1].collider.tag == "Wall")
                            {
                                m_animator.Play("DashB");
                                inputCooldown = m_animator.GetCurrentAnimatorStateInfo(0).length;
                                m_translation = p_direction.normalized;
                                m_isOnFinish = true;
                                blocked = true;
                            }
                            else
                            {
                                m_animator.Play("DashA");
                                inputCooldown = m_animator.GetCurrentAnimatorStateInfo(0).length;
                                m_translation = p_direction;
                            }
                        } 
                        else
                        { 
                            
                            if (p_direction.magnitude > 1)
                            {
                                m_animator.Play("DashA");
                                inputCooldown = m_animator.GetCurrentAnimatorStateInfo(0).length;
                                m_translation = p_direction;
                            }
                            else
                            {
                                m_animator.Play("Move");
                                inputCooldown = m_animator.GetCurrentAnimatorStateInfo(0).length;
                                m_translation = p_direction.normalized;
                            }
                            m_isOnFinish = true;
                            blocked = true;
                        }
                        break;
                    case "Hole":
                        if (p_direction.magnitude > 1 && hit.distance < 1)
                        {
                            if (hits.Length > 1 && hits[1].collider.tag == "Wall")
                            {
                                m_animator.Play("DashBFall");
                                inputCooldown = m_animator.GetCurrentAnimatorStateInfo(0).length;
                                m_isFalling = true;
                                blocked = true;
                            }
                            else
                            {
                                if (p_direction.magnitude > 1)
                                {
                                    m_animator.Play("DashA");
                                    inputCooldown = m_animator.GetCurrentAnimatorStateInfo(0).length;
                                    m_translation = p_direction;
                                }
                            }
                        }
                        else
                        {

                            if (p_direction.magnitude > 1)
                            {
                                m_animator.Play("DashAFall");
                                inputCooldown = m_animator.GetCurrentAnimatorStateInfo(0).length;
                                m_isFalling = true;
                            }
                            else
                            {
                                m_animator.Play("MoveFall");
                                inputCooldown = m_animator.GetCurrentAnimatorStateInfo(0).length;
                                m_isFalling = true;
                            }
                            blocked = true;
                        }
                        break;
                    default:
                        if(hits.Length == 1 || hit.Equals(hits[1]))
                        {
                            if (p_direction.magnitude > 1)
                            {
                                m_animator.Play("DashA");
                                inputCooldown = m_animator.GetCurrentAnimatorStateInfo(0).length;
                                m_translation = p_direction;
                            }
                            else
                            {
                                m_animator.Play("Move");
                                inputCooldown = m_animator.GetCurrentAnimatorStateInfo(0).length;
                                m_translation = p_direction.normalized;
                            }
                        }
                        break;
                }
                if(blocked) 
                    break;
            }
        }
        else
        {
            if (p_direction.magnitude > 1)
            {
                m_animator.Play("DashA");
                inputCooldown = m_animator.GetCurrentAnimatorStateInfo(0).length;
                m_translation = p_direction;
            }
            else
            {
                m_animator.Play("Move");
                inputCooldown = m_animator.GetCurrentAnimatorStateInfo(0).length;
                m_translation = p_direction.normalized;
            }
        }

        if (!m_restartAtSpawn && !m_isFalling)
        {
            m_lastSafePosition = m_positionBuffer;
            m_positionBuffer = transform.position + m_translation;
        }
    }

    public void AnimationEnded()
    {
        m_moveNextFrame = true;
        if(m_isOnFinish)
        {
            if(LevelManager.Instance.LevelComplete())
            {
                m_animator.Play("Despawn");
                inputCooldown += m_animator.GetCurrentAnimatorStateInfo(0).length;
            }
            m_isOnFinish = false;
        }
    }

    public void DeathAnimationEnded()
    {
        StopAllCoroutines();
        Renderer renderer = GetComponentInChildren<Renderer>();
        renderer.material = m_defaultMaterial;
        //renderer.enabled = true;
        //Color color = renderer.material.color;
        //color.a = 1;
        //renderer.material.color = color;
        if (m_restartAtSpawn)
        {
            m_lastSafePosition = LevelManager.Instance.GetSpawnPos();
            LevelManager.Instance.ResetValidatedElements();
        }
        m_animator.Play("Respawn");
        m_positionBuffer = transform.position = m_lastSafePosition;
        inputCooldown += m_animator.GetCurrentAnimatorStateInfo(0).length;
    }

    public void RespawnAnimationEnded()
    {
        inputCooldown = 0.0f; 
        if(m_isFalling)
            m_isFalling = false;
        if(m_isOnSpikes)
            m_isOnSpikes = false;
    }


    public void DespawnAnimationEnded()
    {
        inputCooldown = 0.0f;
        LevelManager.Instance.LoadNextScene();
    }

    public void FallAnimationEnded()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        renderer.enabled = false;
        StartCoroutine("DelayedRespawn");
    }

    IEnumerator FadingDeath()
    {
        m_animator.Play("Die");
        inputCooldown = m_animator.GetCurrentAnimatorStateInfo(0).length;
        float elapsedTime = 0;
        Renderer renderer = GetComponentInChildren<Renderer>();
        renderer.material = m_transparentMaterial;
        Color color = renderer.material.color;
        color.a = 1;
        while (elapsedTime < inputCooldown)
        {
            color.a = Mathf.Lerp(color.a, 0, (elapsedTime / inputCooldown));
            renderer.material.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        renderer.enabled = false;
    }

    IEnumerator DelayedRespawn()
    {
        inputCooldown += 0.5f;
        yield return new WaitForSeconds(0.5f);
        if (m_restartAtSpawn)
        {
            m_lastSafePosition = LevelManager.Instance.GetSpawnPos();
            LevelManager.Instance.ResetValidatedElements();
        }
        m_animator.Play("Respawn");
        inputCooldown += m_animator.GetCurrentAnimatorStateInfo(0).length;
        Renderer renderer = GetComponentInChildren<Renderer>();
        renderer.material = m_defaultMaterial;
        renderer.enabled = true;
        yield return null;
    }
}