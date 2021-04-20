using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    enum jumpDirections{ Forward, Right, Left, Backward};
    jumpDirections jumpDirection = jumpDirections.Forward;
    public float jumpPower;
    public float distanceToDetectMove;
    bool isFingerDown = false;
    Vector2 touchStartPos;
    public Animator animator;
    bool isOnRaft = false;
    bool isOnRiver = false;
    [HideInInspector]
    public bool isOnEndZone = false;
    InScreenClamp inScreenClamp;
    [HideInInspector]
    public HiddingObjects hiddingRaftUnderPlayer;
    public GameMaster gameMaster;
    public AudioSource jumpSound;
    private void Start()
    {
        inScreenClamp = gameObject.GetComponent<InScreenClamp>();
    }

    void Movement()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began && !isFingerDown)
            {
                isFingerDown = true;
                touchStartPos = touch.position;
            }

            if (isFingerDown)
            {

                if (Vector2.Distance(touchStartPos, touch.position) >= distanceToDetectMove)
                {
                    Vector2 deltaPos = touch.position - touchStartPos;

                    if (Mathf.Abs(deltaPos.x) > Mathf.Abs(deltaPos.y))
                    {
                        if (deltaPos.x > 0)
                        {
                            Quaternion rotation = Quaternion.Euler(0, 0, -90);
                            transform.rotation = rotation;
                            jumpDirection = jumpDirections.Right;
                        }
                        else
                        {
                            Quaternion rotation = Quaternion.Euler(0, 0, -270);
                            transform.rotation = rotation;
                            jumpDirection = jumpDirections.Left;
                        }
                    }
                    else
                    {
                        if (deltaPos.y > 0)
                        {
                            Quaternion rotation = Quaternion.Euler(0, 0, 0);
                            transform.rotation = rotation;
                            jumpDirection = jumpDirections.Forward;
                        }
                        else
                        {
                            Quaternion rotation = Quaternion.Euler(0, 0, -180);
                            transform.rotation = rotation;
                            jumpDirection = jumpDirections.Backward;
                        }
                    }

                    if (isOnRaft)
                    {
                        isOnRaft = false;
                        transform.SetParent(null);
                        if (hiddingRaftUnderPlayer != null)
                        {
                            hiddingRaftUnderPlayer = null;
                        }
                    }

                    if (isOnRiver)
                    {
                        isOnRiver = false;
                        inScreenClamp.isPlayerOnRiver = false;
                    }


                    if (jumpDirection == jumpDirections.Forward)
                    {
                        transform.position += Vector3.up * jumpPower;
                    }
                    else if (jumpDirection == jumpDirections.Backward)
                    {
                        transform.position += Vector3.down * jumpPower;
                    }
                    else if (jumpDirection == jumpDirections.Right)
                    {
                        transform.position += Vector3.right * jumpPower;
                    }
                    else
                    {
                        transform.position += Vector3.left * jumpPower;
                    }

                    animator.SetTrigger("Jump");
                    jumpSound.Play();

                    isFingerDown = false;

                }
            }
        }
    }
    void Update()
    {
        if(isOnRiver && !isOnRaft)
        {
            Debug.Log("Death");
            gameMaster.Death();
            isOnRiver = false;
        }

        if(hiddingRaftUnderPlayer != null)
        {
            if(hiddingRaftUnderPlayer.isInvisible)
            {
                isOnRaft = false;
                transform.SetParent(null);
                hiddingRaftUnderPlayer = null;
            }
        }

        Movement();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.gameObject.CompareTag("Car"))
        {
            gameMaster.Death();
        }
        else if(collision.gameObject.CompareTag("StandardLine"))
        {
            Debug.Log("StandardLine");
            gameMaster.LineReached(collision.gameObject);
        }
        else if(collision.gameObject.CompareTag("WaterLine"))
        {
            Debug.Log("WaterLine");

            isOnRiver = true;
            inScreenClamp.isPlayerOnRiver = true;
            gameMaster.LineReached(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("EndZone"))
        {
            Debug.Log("EndZone");
            Debug.Log("Win");
            gameMaster.EndZoneReached(collision.gameObject);
            isOnEndZone = true;
        }
        else if(collision.gameObject.CompareTag("NoGoZone"))
        {
            Debug.Log("NoGoZone");
            if (!isOnEndZone)
            {
                Debug.Log("Death");
                gameMaster.Death();
            }
        }
        else if (collision.gameObject.CompareTag("Raft"))
        {
            Debug.Log("Raft");
            isOnRaft = true;
            transform.SetParent(collision.gameObject.transform);
            transform.localPosition = Vector3.zero;
        }
        else if (collision.gameObject.CompareTag("HiddingRaft"))
        {
            Debug.Log("HiddingRaft");
            isOnRaft = true;
            transform.SetParent(collision.gameObject.transform);
            transform.localPosition = Vector3.zero;
            hiddingRaftUnderPlayer = collision.gameObject.GetComponent<HiddingObjects>();
        }
    }
}
