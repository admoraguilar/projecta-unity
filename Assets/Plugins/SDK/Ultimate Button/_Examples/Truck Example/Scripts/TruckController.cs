/* Written by Kaz Crowe */
/* TruckController.cs */
using UnityEngine;
using System.Collections;

public class TruckController : MonoBehaviour
{
	[Header( "Assigned Variables" )]
    public Transform cameraTransform;
	public Rigidbody2D myRigidbody;
	public WheelJoint2D frontWheel;
	public WheelJoint2D rearWheel;
	public Rigidbody2D frontRigidbody;
	public Rigidbody2D backRigidbody;
	public Transform groundCheck;

	[Header( "Speeds and Times" )]
	public int maxSpeed = 1200;
	public int reverseSpeed = 250;
	public float accelerationSpeed = 10.0f;
	public float decelerationSpeed = 5.0f;
	public float resetTimerMax = 2.5f;

	// Misc. Private Variables
	JointMotor2D wheelMotor;
	Vector3 cameraDefaultPosition;
	float resetTimer = 0.0f;
	bool isResetting = false;
	bool isGrounded = false;
	
	
	void Start ()
	{
		// Store the current position of the camera in relation to the truck.
		cameraDefaultPosition = cameraTransform.position - transform.position;

		// Set a max motor torque.
		wheelMotor.maxMotorTorque = 10000;

		// Apply the resetTimerMax to the resetTimer.
		resetTimer = resetTimerMax;
	}

	void Update ()
	{
		// If the button has been pressed down on this frame, and the truck is not moving faster than -250, then set the motor speed to -250.
		if( UltimateButton.GetButtonDown( "Gas" ) && wheelMotor.motorSpeed > -250 )
			wheelMotor.motorSpeed = -250;
	}
	
	void FixedUpdate ()
	{
		// Determine if the truck is grounded or not by using a Line cast.
		isGrounded = Physics2D.Linecast( transform.position, groundCheck.position, 1 << LayerMask.NameToLayer( "Default" ) );

		// If the gas button is down...
		if( UltimateButton.GetButton( "Gas" ) )
		{
			// If the motor speed is less than the max speed, then accelerate.
			if( wheelMotor.motorSpeed > -maxSpeed )
				wheelMotor.motorSpeed -= Time.deltaTime * accelerationSpeed;

			// If the truck is grounded, then add force to help the truck have more power.
			if( isGrounded == true )
				myRigidbody.AddForceAtPosition( new Vector2( 20, 0 ), new Vector2( transform.position.x, transform.position.y - 1 ) );
		}
		else
		{
			// If the reverse button is down and the motor is not going at all...
			if( UltimateButton.GetButton( "Reverse" ) && wheelMotor.motorSpeed >= 0 )
			{
				// Then set the motor speed.
				if( wheelMotor.motorSpeed != reverseSpeed )
					wheelMotor.motorSpeed = reverseSpeed;

				// If the truck is grounded, then add force to help give more power.
				if( isGrounded == true )
					myRigidbody.AddForceAtPosition( new Vector2( -10, 0 ), new Vector2( transform.position.x, transform.position.y - 1 ) );
			}
			else
			{
				// If the motor is running, then decelerate the truck.
				if( wheelMotor.motorSpeed < 0 )
					wheelMotor.motorSpeed += Time.deltaTime * decelerationSpeed;
				// Else if the motor speed is not stopped correctly and the truck is not in reverse, then set the motor speed to 0.
				else if( wheelMotor.motorSpeed > 0 && !UltimateButton.GetButton( "Reverse" ) )
					wheelMotor.motorSpeed = 0;

				// If the truck is grounded, the wheel speed is nearly stopped, and the truck is still moving, then add force to slow the truck down.
				if( isGrounded == true && wheelMotor.motorSpeed > -100 && myRigidbody.velocity.x > 2 )
					myRigidbody.AddForceAtPosition( new Vector2( -30, 0 ), new Vector2( transform.position.x, transform.position.y - 1 ) );
			}
		}

		// Apply the motor force.
		rearWheel.motor = wheelMotor;
		frontWheel.motor = wheelMotor;

		// Make the camera follow the truck.
		cameraTransform.position = transform.position + cameraDefaultPosition;

		// If the truck is not grouned and the truck is not moving, and the truck is not in the middle of resetting...
		if( isGrounded == false && myRigidbody.velocity.magnitude <= 0.5f && isResetting == false )
		{
			// Reduce the reset timer.
			resetTimer -= Time.deltaTime;

			// If the timer is less than zero, then reset the timer and start the ResetTruck coroutine.
			if( resetTimer <= 0 )
			{
				resetTimer = resetTimerMax;
				StartCoroutine( "ResetTruck" );
			}
		}
		// Else if the truck is grounded and the timer isn't at max, then reset the timer to max.
		else if( isGrounded == true && resetTimer != resetTimerMax )
			resetTimer = resetTimerMax;
	}

	IEnumerator ResetTruck ()
	{
		// Set isResetting to true so that other functions know this function is running.
		isResetting = true;

		// Set the rigidbody to isKinematic to not allow physics movements to it.
		myRigidbody.isKinematic = true;

		// Store the positions of where the truck currently is, and where the trucks ending position should be.
		Vector2 originalPos = transform.position;
		Vector2 endPos = new Vector2( transform.position.x, transform.position.y + 3f );

		// Store the original rotation of the truck.
		Quaternion originalRot = transform.rotation;

		for( float t = 0.0f; t < 1.0f; t += Time.deltaTime * 1.5f )
		{
			// Lerp the position from the original to the end position.
			transform.position = Vector2.Lerp( originalPos, endPos, t );

			// Slerp the rotation from the original to a 0 rotation.
			transform.rotation = Quaternion.Slerp( originalRot, Quaternion.identity, t );

			// Wait for the Fixed Update so that all movement is fluid.
			yield return new WaitForFixedUpdate();
		}

		// Set the back and front tire's velocity to 0 to avoid moving the truck at all.
		backRigidbody.velocity = Vector2.zero;
		frontRigidbody.velocity = Vector2.zero;

		// Allow for physics movements again.
		myRigidbody.isKinematic = false;

		// Set isResetting to false so that this script can call this function again.
		isResetting = false;
	}
}