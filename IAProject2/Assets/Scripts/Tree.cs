using UnityEngine;

public abstract class Tree : MonoBehaviour
{
    protected Node root = null;
    // Start is called before the first frame update
    void Start()
    {
        root = Setup();
    }

    // Update is called once per frame
    void Update()
    {
        if (root != null)
        {
            root.Evaluate();
        }
    }

    protected abstract Node Setup();
}
