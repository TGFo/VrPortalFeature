using UnityEngine;

public class Portal : MonoBehaviour
{
    public Renderer renderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        renderer = gameObject.GetComponent<Renderer>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
