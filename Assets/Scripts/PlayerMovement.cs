/*** 
*file: PlayerMovement.cs 
*Members: Juniper Watson, Andrew Sanford, Marty Scott
*class: CS 4700 – Game Development 
*assignment: program 4
*date last modified: 12/1/2022 
* 
*purpose: This scripts allows the player to control the player sprite's movement, 
*and ensures proper animations and sound effects are played while walking. 
* 
**/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header ("Components")]
    private Rigidbody2D rb;
    private BoxCollider2D boxColl;
    public Animator animator;

    [Header ("Movement Details")]
    public bool canMove = true;
    public float speed = 10f;
    Vector2 dir;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxColl = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //if can't move, then set velocity to zero and return
        if (!canMove)
        {
            dir.x = 0;
            dir.y = 0;

            animator.SetFloat("Horizontal", dir.x);
            animator.SetFloat("Vertical", dir.y);
            animator.SetFloat("Speed", dir.sqrMagnitude);

            rb.velocity = Vector2.zero;


            return;
        }
        //else
        //Get value of horiz/vert axes
        dir.x = Input.GetAxis("Horizontal");
        dir.y = Input.GetAxis("Vertical");

        animator.SetFloat("Horizontal", dir.x);
        animator.SetFloat("Vertical", dir.y);
        animator.SetFloat("Speed", dir.sqrMagnitude); //square magnitude is slightly faster than magnitude
           
        //update velocity toward given player input at speed
        rb.velocity = new Vector2(dir.x * speed, dir.y * speed);



        // start audio source(walking FX) if moving
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
            {
            GetComponent<AudioSource>().UnPause();
        }
        else
        {
            //stop audio source(walking FX) if not moving
            GetComponent<AudioSource>().Pause();
        }
    }
}
