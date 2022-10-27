using UnityEngine;
using System.Collections.Generic;

public class ObstacleManager : MonoBehaviour
{
    const float DISTANCE_BETWEEN_OBSTACLES = 6f;
    const float HEIGHT_RANDOM = 3f;
    const int MIN_COUNT = 3;
    public List<GameObject> prefab;
    public OBSTACLE_TYPE type;
    Vector3 pos = new Vector3(DISTANCE_BETWEEN_OBSTACLES, 0, 0);

    List<Obstacle> obstacles = new List<Obstacle>();

    private static ObstacleManager instance = null;

    public static ObstacleManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<ObstacleManager>();

            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public void Reset()
    {
        for (int i = 0; i < obstacles.Count; i++)
            Destroy(obstacles[i].gameObject);

        obstacles.Clear();

        pos.x = 0;

        InstantiateObstacle();
        InstantiateObstacle();
    }

    public Obstacle GetNextObstacle(Vector3 pos)
    {
        for (int i = 0; i < obstacles.Count; i++)
        {
            if (pos.x < obstacles[i].transform.position.x + 2f)
                return obstacles[i];
        }

        return null;
    }

    public bool IsColliding(Vector3 pos)
    {
        Collider2D collider = Physics2D.OverlapBox(pos, new Vector2(0.3f, 0.3f), 0);

        if (collider != null)
            return true;

        return false;
    }

    public void CheckAndInstatiate()
    {
        for (int i = 0; i < obstacles.Count; i++)
        {
            obstacles[i].CheckToDestroy();
        }

        while (obstacles.Count < MIN_COUNT)
            InstantiateObstacle();
    }

    void InstantiateObstacle()
    {
        int index = Random.Range(0, 2);
        if (type == OBSTACLE_TYPE.VERTICAL || (type == OBSTACLE_TYPE.ANY && index == 1)) 
        {
            pos.x += DISTANCE_BETWEEN_OBSTACLES;
            pos.y = Random.Range(-HEIGHT_RANDOM, HEIGHT_RANDOM);
            GameObject go = GameObject.Instantiate(prefab[0], pos, Quaternion.identity);
            go.transform.SetParent(this.transform, false);
            Obstacle obstacle = go.GetComponent<Obstacle>();
            obstacle.type = OBSTACLE_TYPE.VERTICAL;
            obstacle.OnDestroy += OnObstacleDestroy;
            obstacles.Add(obstacle);
        }
        else if (type == OBSTACLE_TYPE.HORIZONTAL || (type == OBSTACLE_TYPE.ANY && index == 2)) 
        {
            pos.x += DISTANCE_BETWEEN_OBSTACLES;
            pos.y = 0;
            GameObject go = GameObject.Instantiate(prefab[1], pos, Quaternion.identity);
            go.transform.SetParent(this.transform, false);
            Obstacle obstacle = go.GetComponent<Obstacle>();
            obstacle.type = OBSTACLE_TYPE.HORIZONTAL;
            obstacle.OnDestroy += OnObstacleDestroy;
            obstacles.Add(obstacle);
        }
    }

    void OnObstacleDestroy(Obstacle obstacle)
    {
        obstacle.OnDestroy -= OnObstacleDestroy;
        obstacles.Remove(obstacle);
    }
}
