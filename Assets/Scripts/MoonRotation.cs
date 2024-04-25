using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonRotation : MonoBehaviour
{
    public static MoonRotation instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    [SerializeField] float moonSpeed = 3f;

    // Update is called once per frame
    void Update()
    {
        RotateMoon();
    }


    void RotateMoon()
    {
        transform.Rotate(Vector3.forward * moonSpeed * Time.deltaTime);
    }
}
