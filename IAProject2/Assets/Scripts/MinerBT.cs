using System.Collections.Generic;

public class MinerBT : Tree
{
    protected override Node Setup()
    {
        Node root = new Root(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new Not(new List<Node>
                {
                    new Leave()
                }),
                new Sequence(new List<Node>
                {
                    new Not(new List<Node>
                    {
                        new Leave()
                    }),
                    new Leave()
                })
            })
        });
        return root;
    }
}