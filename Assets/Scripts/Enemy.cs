using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float Speed = 4;

    [SerializeField]
    private float BottomBoundry = -10.0f;

    [SerializeField]
    private float LeftBoundry = -10.0f;

    [SerializeField]
    private float RightBoundry = 10.0f;

    [SerializeField]
    private bool Die_on_range_end = true;

    private Vector3 StartingPoint;
    // Start is called before the first frame update
    void Start()
    {
        StartingPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        transform.Translate(Vector3.down * Speed * Time.deltaTime);
        HandleBounds();
    }

    private void HandleBounds()
    {
        if (transform.position.y < BottomBoundry)
        {
            if (!Die_on_range_end)
            {
                var randomX = Random.Range(LeftBoundry, RightBoundry);
                transform.position = new Vector3(randomX, StartingPoint.y, StartingPoint.z); ;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        if (other.tag == "Player")
        {
            Player p = other.transform.GetComponent<Player>();
            if (p != null)
            {
                p.RecieveDamage();
                Destroy(this.gameObject);
            }
        }
    }



}
