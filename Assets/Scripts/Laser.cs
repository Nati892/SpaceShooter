using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float Speed = 5;

    [SerializeField]
    private float LifeSpan = 9;

    [SerializeField]
    public int YPos = 1;
    private float StartingPoint = 0;
    private float DeathBoundry = 0;
    // Start is called before the first frame update
    void Start()
    {
        DeathBoundry = StartingPoint + Mathf.Abs(LifeSpan);

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, YPos * Speed * Time.deltaTime, 0));
        if (this.transform.position.y > DeathBoundry)
        {
            if (this.transform.parent != null && this.transform.parent.tag == "TripleShot")
            {
                Destroy(this.transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

}
