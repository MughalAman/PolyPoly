using TMPro;
using UnityEngine;

public class WeaponOffline : MonoBehaviour
{
    //Vars
    public Transform rayOrigin;
    public int Damage = 10;
    public ParticleSystem GunSmoke;
    public ParticleSystem BulletShell;
    public int Ammo = 30;
    public int MagazineSize = 30;
    public float fireRate = 15f;
    [SerializeField]
    private TMP_Text AmmoText = null;
    private float nextTimeToFire = 0f;
    //Update Ammo UI element on start
    void Start()
    {
        AmmoText.text = "Ammo: " + Ammo.ToString();
    }


    void Update()
    {
            TakeInput();    
    }

    private void TakeInput()
    {
        //If player left clicks lets check if player has ammo if player has shoot using photons RPC to execute shooting on all clients then reduce ammo and update player UI
        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            if (Ammo >= 1)
            {
                Shoot();
                FindObjectOfType<SoundSystem>().Play("Shoot");
                Ammo -= 1;
                AmmoText.text = "Ammo: " + Ammo.ToString();
            }
            else
            {
                FindObjectOfType<SoundSystem>().Play("OutOfAmmo");
            }
        }

        //If player presses R then change ammo to magazines size after that update player UI
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Ammo != 30)
            {
                FindObjectOfType<SoundSystem>().Play("Reload");
                Ammo = MagazineSize;
                AmmoText.text = "Ammo: " + Ammo.ToString();
            }
        }
    }

    //Shooting
    void Shoot()
    {
        //Play bullet and smoke particles
        BulletShell.Play();
        GunSmoke.Play();

        RaycastHit hit;
        //Lets check if our raycast hit a player if it did then deal damage to that player
        if (Physics.Raycast(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward), out hit, 1000))
        {
            Debug.DrawRay(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward) * hit.distance, Color.magenta);
            Debug.Log("Did Hit");
            if (hit.transform.tag == "Enemy")
            {
                //hit.transform.gameObject.GetComponent<Enemy>().Health -= Damage;
            }
        }
        else
        {
            Debug.Log("Did not hit");
        }
    }
}
