using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject FallingItem;
    public GameObject SpawnPoint;
    public static Spawner spawner;
    private int counter = 0;
    private Coroutine coroutine;
    public Stack<GameObject> stack = new Stack<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        coroutine = StartCoroutine(SpawnBall());
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        Spawner.spawner.Push(collision.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Push(GameObject obj)
    {
        obj.SetActive(false);
        stack.Push(obj);
    }
    public GameObject Pop()
    {
        GameObject obj = stack.Pop();
        obj.SetActive(true);
        obj.transform.position = SpawnPoint.transform.position;
        return obj;
    }
    public GameObject Peek()
    {
        return stack.Peek();
    }
    private IEnumerator SpawnBall()
    {
        try
        {
            Debug.Log(Peek().name);
            Pop();
        }
        catch 
        {
            GameObject bullet = Instantiate(FallingItem, SpawnPoint.transform.position, Quaternion.identity);
            bullet.GetComponent<FallingObject>().spawner = this.gameObject; 
        }
        if (counter >= 4)
        {
            StopCoroutine(coroutine);
        }
        counter++;
        yield return new WaitForSeconds(1.5f);
        yield return SpawnBall();

    }
}
