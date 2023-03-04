using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Iitem
{
    //if I were to make multiple items, like offensive items, defensive items and useable items, there should be an item interface

    //each item should probably have animations tied to it, with the pickup, these animations will be replaced by relevant ones in the player animator
    //Although more complex animations might become, as the name implies, more complicated
    void OnPickUp();
    void BasicAbility();
    void SpecialAbility();
}
