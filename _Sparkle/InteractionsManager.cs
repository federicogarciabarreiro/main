using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class InteractionsManager  {

    Hero hero;
    public List<List<IInteractable>> listOfInteractionObjects = new List<List<IInteractable>>();
    IInteractable currentInteraction;


    public void Initialize()
    {
        listOfInteractionObjects.Clear();
        currentInteraction = null;
        hero = Config.me.hero.GetComponent<Hero>();
        for (int i = 0; i < (int)Skill.Count; i++)
            listOfInteractionObjects.Add(new List<IInteractable>());

    }

    public void OutlineObject()
    {
        for (int i = 0; i < (int)Skill.Count; i++)
        {
            foreach (var item in listOfInteractionObjects[i])
                item.SwitchOutline(false, CursorTextureType.PRIMARY);
            var obj = GetClosestInteractionObject(hero.skillTypes[i]);
            if (obj != null) obj.SwitchOutline(true, CursorTextureType.PRIMARY);
        }
    }

    public void AddInteractionObject(IInteractable obj, Skill list)
    {
        listOfInteractionObjects[(int)list].Add(obj);
    }

    public void RemoveInteractionObject(IInteractable obj, Skill list)
    {
        listOfInteractionObjects[(int)list].Remove(obj);
    }

    public IInteractable GetClosestInteractionObject(Skill list)
    {
        currentInteraction = listOfInteractionObjects[(int)list].Where(x => x.CanInteract(hero)).OrderBy(x => Vector3.Distance(x.GetPosition(), hero.transform.position)).FirstOrDefault();
        return currentInteraction;
    }
}
