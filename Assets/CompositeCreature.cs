using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompositeCreature : MonoBehaviour
{
    [SerializeField] private Creature firstCreature;
    [SerializeField] private Creature secondCreature;

    [SerializeField] private Image clazz;
    [SerializeField] private Image family;

    public void RegisterChange()
    {
        //var creatureToChange = firstCreature.gameObject == creature ? firstCreature : secondCreature;
        //creatureToChange.SetCreature(model);

        if (firstCreature.isSet && secondCreature.isSet)
        {
            clazz.color = Color.magenta;
            family.color = Color.magenta;
        } 
        else if (firstCreature.isSet)
        {
            clazz.color = Color.cyan;
            family.color = Color.cyan;
        }
        else if (secondCreature.isSet)
        {
            clazz.color = Color.blue;
            family.color = Color.blue;
        }
        else
        {
            clazz.color = Color.red;
            family.color = Color.red;
        }
    }
}
