using Photon.Pun;
using UnityEngine;
using TMPro;

public class Movement : MonoBehaviourPun
{
    //Vars
    public int Health = 100;
    [SerializeField]
    private TMP_Text HealthText = null;

    [SerializeField] private float speed = 12f;
    [SerializeField] private float gravity = -18f;
    [SerializeField] private float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public CharacterController controller;

    public ParticleSystem BloodSplatter;

    Vector3 velocity;
    bool isGrounded;

    //Update player health UI element on start
    void Start()
    {
        HealthText.text = "Health: " + Health.ToString();
    }

    void Update()
    {
        //if photonview is ours take control of the input
        if(photonView.IsMine && GameObject.Find("SpawnSystem").GetComponent<Spawner>().Debug == false)
        {
            TakeInput();
        }
        else
        {
            TakeInput();
        }
    }

    private void TakeInput()
    {
        //Movement code

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }


        //Get inputs from keyboard
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        //normalize movement
        Vector3 move = Vector3.Normalize(transform.right * x + transform.forward * z);

        //move player
        controller.Move(move * speed * Time.deltaTime);

        //jump if player is grounded 
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            FindObjectOfType<SoundSystem>().Play("Jump");
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        //Check if player has fallen out of map
        if (transform.position.y <= -20)
        {
            transform.position = new Vector3(transform.position.x, 10, transform.position.z);
        }
    }

    public void CheckHealth()
    {
        //Check if players health is 0 and then respawn player and change health back to 100 then update player UI
        if (Health <= 0)
        {
            transform.position = new Vector3(Random.Range(-181, 88), 20, Random.Range(-51, 160.8f));
            Health = 100;
            HealthText.text = "Health: " + Health.ToString();
        }
    }

    public void TakeDamage()
    {
        //Update player UI, play hurt sound and play blood splatter effect
        HealthText.text = "Health: " + Health.ToString();
        FindObjectOfType<SoundSystem>().Play("Hurt");
            BloodSplatter.Play();
    }

}
