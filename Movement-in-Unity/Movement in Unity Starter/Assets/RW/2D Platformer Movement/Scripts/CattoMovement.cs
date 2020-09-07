/*
 * Copyright (c) 2020 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using UnityEngine;

public class CattoMovement : MonoBehaviour
{
    // Variables for components
    // 1

    private Rigidbody2D rb;
    private Animator animator;

    // Bools to check state
    // 2

    private bool isCatFacingRight = true;
    private bool isCatJumping = false;
    private bool isCatGrounded = false;

    // Most common variables
    // 3

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float moveInput;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        isCatGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, ground);
        moveInput = Input.GetAxis("Horizontal");

        SetAnimatorValues();
    }

    private void SetAnimatorValues()
    {
        if (isCatGrounded)
            animator.SetFloat("Velocity", Mathf.Abs(moveInput));

        if (isCatGrounded && Input.GetButtonDown("Jump"))
        {
            isCatJumping = true;
            animator.SetTrigger("Jump");
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && isCatGrounded)
        {
            animator.SetBool("Crouch", true);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
            animator.SetBool("Crouch", false);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if ((isCatFacingRight && moveInput < 0) || (!isCatFacingRight && moveInput > 0))
            FlipCat();

        if(isCatJumping)
        {
            rb.AddForce(jumpForce * Vector2.up);
            isCatJumping = false;
        }
    }

    private void FlipCat()
    {
        isCatFacingRight = !isCatFacingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
