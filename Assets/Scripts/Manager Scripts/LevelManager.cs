using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    /// 
    /// EVENTS
    /// 
    public delegate void LevelManagerEventHandler();
    public event LevelManagerEventHandler SpawnEvent;
    public event LevelManagerEventHandler LevelFinnishedEvent;

    public GameObject[] objects; //will be needed
    public Collider2D[] triggers; //maybe

    private GameManager eventMasterScript;
    private int offset;

    private void CallSpawnEvent()
    {
        if (SpawnEvent != null)
        {
            SpawnEvent();
        }
    }

    public void CallLevelFinnishedEvent()
    {
        if (LevelFinnishedEvent != null)
        {
            LevelFinnishedEvent();
        }
    }

    private void LoadNextLevel()
    {

    }

    private void SetInitialReference()
    {
        eventMasterScript = GameObject.Find("GameManager").GetComponent<GameManager>();

        var tmp = GameObject.Find("Eye Candy").GetComponent<Transform>();

        Transform north = tmp;
        Transform south = tmp;

        foreach (Transform curr in tmp)
        {
            if (curr.position.y > north.position.y)
            {
                north = curr;
            }
            if (curr.position.y < south.position.y)
            {
                south = curr;
            }
        }

        offset = Mathf.FloorToInt(north.position.y - south.position.y);
      
        foreach (Transform curr in tmp)
        {
            curr.GetComponent<Renderer>().sortingOrder = getLayer(curr.position.y);
        }

        var player = GameObject.Find("Player");
        player.GetComponent<Renderer>().sortingOrder = getLayer(player.transform.position.y);
    }

    public int getLayer(float y)
    {
        return Mathf.FloorToInt(offset - y);
    }

    void Awake()
    {
        SetInitialReference();
        Debug.Log("Level manager running");
    }

    // Use this for initialization
    void Start()
    {
        CallSpawnEvent();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
