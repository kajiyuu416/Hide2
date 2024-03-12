using UnityEngine;

public class objMove : MonoBehaviour
{
    [SerializeField] float rotatex = 0;
    [SerializeField] float rotatey = 0;
    [SerializeField] float rotatez = 0;
    private Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float posY = startPos.y + Mathf.Sin(Time.time);
        transform.position = new Vector3(transform.position.x, posY, transform.position.z);
        gameObject.transform.Rotate(new Vector3(rotatex, rotatey, rotatez) * Time.deltaTime);
    }
}
