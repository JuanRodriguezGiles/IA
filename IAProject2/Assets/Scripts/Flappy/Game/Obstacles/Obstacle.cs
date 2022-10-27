using System;

using UnityEngine;

[Serializable]
public enum OBSTACLE_TYPE
{
    HORIZONTAL,
    VERTICAL,
    ANY
}

public class Obstacle : MonoBehaviour
{
    public System.Action<Obstacle> OnDestroy;
    public OBSTACLE_TYPE type;
    
    public void CheckToDestroy()
    {
        if (this.transform.position.x - Camera.main.transform.position.x < -7.5f)
        {
            if (OnDestroy != null)
                OnDestroy.Invoke(this);

            Destroy(this.gameObject);
        }

    }
}