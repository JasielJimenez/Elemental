using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusType
    {
        KnockDown,      //If stance drops below zero
        Stun,           
        Wounded,        
        BadlyWounded,   
        Bound,
        Exhausted,      //If stamina drops below zero
        Overflow,       //If element drops below zero
        Weakened,       //Inflicted by null attacks
        Burned,         //Inflicted by fire attacks
        Soaked,         //Inflicted by water attacks
        Chilled,        //Inflicted by ice attacks
        Tremored,       //Inflicted by earth attacks
        Gusted,         //Inflicted by wind attacks
        Shocked,        //Inflicted by electric attacks
        Locked,         //Inflicted by tech attacks
        Blinded,        //Inflicted by light attacks
        Petrified,      //Inflicted by shadow attacks
        Infested,       //Inflicted by life attacks
        Despair,        //Inflicted by death attacks
        Lunatic         //Inflicted by lunar attacks
    }
