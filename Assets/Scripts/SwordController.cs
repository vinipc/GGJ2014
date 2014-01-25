using UnityEngine;
using System.Collections;

public abstract class SwordController : MonoBehaviour {

	public float maxSpeed = 10f;
	public float groundRadius = 0.04f;
	public float jumpForce = 550f;
	public float maxAirTime = 0.3f;
	public float jumpHoldFactor = 15f;

	protected bool facingLeft;

	public Transform groundCheck;
	public LayerMask whatIsGround;
	protected bool grounded;

	//Animator anim;

	void Start()
	{
		//anim = GetComponent<Animator>();
	}
	
	protected abstract float HorizontalInputMethod ();

	protected virtual void FixedUpdate () 
	{
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

		float move = HorizontalInputMethod();

		//anim.SetFloat("Speed", Mathf.Abs(move));

		rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);

		if(move > 0 && facingLeft)
			Flip();
		else if (move < 0 && !facingLeft)
			Flip();

	}

	protected abstract bool JumpInputMethod ();

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
		while (JumpInputMethod() && airTime < maxAirTime);

		rigidbody2D.AddForce(new Vector2(0, -jumpForce/2));
	}
	
	void Flip()
	{
		facingLeft = !facingLeft;
		Vector3 temp = transform.localScale;
		temp.x = - temp.x;
		transform.localScale = temp;
	}
}