using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerBT : Tree
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    protected override Node Setup()
    {
        Node root = new Root(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new Not(new List<Node>{
                //MyLeaveNode
                }),
                new Sequence(new List<Node>{
                    new Not(new List<Node>(
                        //MyLeaveNode2
                        ))
                    //MyLeaveNode3
                })
            })
        });
        return root;
    }
}
