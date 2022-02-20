using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowProjectile : MonoBehaviour
{

    public GameObject projectile;
    public float projVelocity = 10f;

    public int Direction;
    public bool isReady = false;


    public void SetRockReady(int direction)
    {
        isReady = true;
        Direction = direction;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Down") && isReady)
        {
            GameObject rock = Instantiate(projectile, transform.position,
                                                      transform.rotation);
            rock.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(0, Direction * projVelocity, 0));
        }
    }
}
