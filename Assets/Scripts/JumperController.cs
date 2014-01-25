using UnityEngine;
using System.Collections;

public abstract class JumperController : MonoBehaviour {

	public float maxSpeed = 10f;
	public float groundRadius = 0.04f;
	public float jumpForce = 650f;
	public float maxAirTime = 0.3f;
	public float jumpHoldFactor = 15f;

	public bool attachCamera = false;

	bool facingLeft;

	public Transform groundCheck;
	public LayerMask whatIsGround;
	protected bool grounded;

	protected virtual void Update()
	{
		if(GetJumpInputDown())
		{
			if(grounded)
				StartCoroutine("Jump");
			else
				Kickdown();
		}
		
		if(attachCamera)
			Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
	}

	void FixedUpdate () 
	{
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

		float move = GetHorizontalInput();

		rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);

		if(move > 0 && facingLeft)
			Flip();
		else if (move < 0 && !facingLeft)
			Flip();

	}

	IEnumerator Jump()
	{
		float airTime = 0f;
		rigidbody2D.AddForce(new Vector2(0, jumpForce));

		do
		{
			rigidbody2D.AddForce(new Vector2(0, Time.deltaTime*50*jumpForce/jumpHoldFactor));
			airTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		while (GetJumpInput() && airTime < maxAirTime);

		rigidbody2D.AddForce(new Vector2(0, -jumpForce/2));
	}

	protected void Kickdown()
	{
		rigidbody2D.AddForce (new Vector2(0, -2f*jumpForce));
	}
	
	void Flip()
	{
		facingLeft = !facingLeft;
		Vector3 temp = transform.localScale;
		temp.x = - temp.x;
		transform.localScale = temp;
	}

	protected abstract bool GetJumpInput();
	protected abstract bool GetJumpInputDown();
	protected abstract float GetHorizontalInput();
}