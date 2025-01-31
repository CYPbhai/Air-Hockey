using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    protected MenuUI previousMenuUI;

    public virtual void Show(MenuUI previous)
    {
         if(previous)
             previousMenuUI = previous;
         gameObject.SetActive(true);
    }

    public virtual void Hide(bool returnToPrevious)
    {
         gameObject.SetActive(false);

         if(previousMenuUI && returnToPrevious)
         {
             previousMenuUI.Show(null);
         }
    }
}
