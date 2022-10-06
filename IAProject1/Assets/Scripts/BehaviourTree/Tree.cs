using UnityEngine;

public abstract class Tree : MonoBehaviour
{
   protected TreeNode root = null;

   private void Start()
   {
      root = Setup();
   }

   void Update()
   {
      if (root != null)
      {
         root.Evaluate();
      }
   }

   protected abstract TreeNode Setup();
}