using UnityEngine;
using System.Collections;

public class Conversation : MonoBehaviour {

    // Use this for initialization
    void Start () {
        //loadConversation(0);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public static string[] loadConversation(int whatConversation)
    {
        switch (whatConversation) //define conversation based on what conversation is going to be loaded
        {
            case 0: //Todd's dialogue
                string[] messages = { "... <color=#ff2222>▼</color>",
                "H…Hi! Who awe you? <color=#ff2222>▼</color>",
                "... <color=#ff2222>▼</color>",
                "Kristy...so cute <color=#ff2222>▼</color>",
                "!!! <color=#ff2222>▼</color>",
                "I-I was talking about your name… I mean, actually… <color=#ff2222>▼</color>",
                "*blushes off intensively* <color=#ff2222>▼</color>",
                "Um... Nice to meet you, I’m Todd Ino. <color=#ff2222>▼</color>",
                "Please let me check your KUC.<color=#ff2222>▼</color>",
                "...<color=#ff2222>▼</color>",
                "Kristine Adams from Canvas Town. Got it, i'll call you Kristy ^_^<color=#ff2222>▼</color>",
                "So. Are you ready to face me? Don't think i'll take on easy on an older giwl, HEHEH! :D<color=#ff2222>▼</color>",
                "N-no, i'm not calling you a grandma, just...<color=#ff2222>▼</color>",
                "Well fowget it!<color=#ff2222>▼</color>",
                "   Let's 'gow'!"};
                return messages;
            case 1: //PP consumption - Woman
                string[] messages1 = { "Regular <color=#181880>movements</color> consume 1 <color=#181880>process point</color>, whilst <color=#A91E1B>attacks</color> consume all your 2 <color=#A91E1B>APs</color>. <color=#ff2222>▼</color>",
                "'Process points'? They're the amount of actions each kamibot can do per turn<color=#ff2222>▼</color>",
                "In other words, kamibots can <i>move twice</i>, <i>move and attack</i> or <i>just attack</i>. <color=#ff2222>▼</color>",
                "Think wisely before acting."};
                return messages1;
            case 2: //Carry restoring items - Man
                string[] messages2 = { "Going on kamibot battles? <color=#ff2222>▼</color>",
                "You should always carry some restoring items, like <color=#27C400>glue</color> or spare <color=#27C400>batteries</color>.<color=#ff2222>▼</color>",
                "Battlefields are tough… and so is life."};
                return messages2;
            case 3: //Summary of battles - Girl
                string[] messages3 = { "Select your bot, choose their action, choose a target, watch their <i>stats</i> and <i>specs</i>… <color=#ff2222>▼</color>",
                    "Kamibot battle is not really a rocket science. <color=#ff2222>▼</color>",
                "It’s all about trial and error, that’s my motto."};
                return messages3;
            case 4: //visual cues - Girl
                string[] messages4 = { "Always pay attention to the visual cues of a kamibot battlefield.<color=#ff2222>▼</color>",
                "They save you from being annoyed checking every inch of your screen.<color=#ff2222>▼</color>",
                "Hell yeah!"};
                return messages4; 
            case 5: //grinding - Man
                string[] messages5 = { "Level up your kamibot team by defeating other kamibots. <color=#ff2222>▼</color>",
                "You may even get prizes like money or items too. <color=#ff2222>▼</color>",
                "Oh, gosh. I should retire and become a kamibot user, teehee."};
                return messages5;
                
            case 6: //be careful on battles - Boy
                string[] messages6 = { "Be cautious when battling.<color=#ff2222>▼</color>",
                "When possible, use restoring items from your inventory on an active kamibot.<color=#ff2222>▼</color>",
                "Always look for strategic points and best actions too.<color=#ff2222>▼</color>",
                "Kamibots are at disadvantage in small numbers.<color=#ff2222>▼</color>",
                "Trust me, ya wouldn't like that thing."};
                return messages6;
            case 7: //relief - Woman
                string[] messages7 = { "'Everyone here is a kamibot guru!', you might be thinking.<color=#ff2222>▼</color>",
                "Well, blame the children of this town, teehehe."};
                return messages7;
            case 8: //tips about kami classes - Man
                string[] messages8 = { "Aquasniper! Shoot 'em down!~♪ \n Sliceknight! Raise thy blade~♪<color=#ff2222>▼</color>",
                "Mantial! Show me your moves and fight!~♪ \n Metaman! Copy in the speed of the light!<color=#ff2222>▼</color>",
                "Kamibots!~♪ Kamibots all the waaay~♪<color=#ff2222>▼</color>",
                "Did you like it? They call me the Kamibard!"};
                return messages8;
            case 9: //todd localization - Woman
                string[] messages9 = { "In this small town, you shall battle <color=#ff2222>Todd</color>, a paperchief.<color=#ff2222>▼</color>",
                "He is often playing in that sandbox at north.<color=#ff2222>▼</color>",
                "Todd is pretty <color=#ff2222>naive</color>, but don't understimate that cute boy :P"};
                return messages9;
            case 10: //relief2 - Boy
                string[] messages10 = { "In 1999, Ada Marie Siens created the first intelligent papercraft known.<color=#ff2222>▼</color>",
                "and 10 years later she founded PRISM.<color=#ff2222>▼</color>",
                "Right after, she started development of Azuretooth techonology by the age of 23!<color=#ff2222>▼</color>",
                "It was also her decision to put the invention in public domain.<color=#ff2222>▼</color>",
                "Ahh.. she's so wonderful~❤"};
                return messages10;

            case 100: //Todd's defeated conversation
                string[] messages100 = { "That was gweat!! <color=#ff2222>▼</color>",
                "Never thought you would get such an epic win! <color=#ff2222>▼</color>",
                "N-no! I’m not calling you a weakling, I just… <color=#ff2222>▼</color>",
                "Well, fowget it. <color=#ff2222>▼</color>",
                "Anyway, here is your Kinderbot Medal for your victory. Please take it! <color=#ff2222>▼</color>",

                "You got <color=#ff2222>Kinderbot Medal</color>! You can now assign and use <color=#ff2222>Kinderbots</color> in your team. <color=#ff2222>▼</color>",

                "That's it. Congratulations ^_^ <color=#ff2222>▼</color>",
                "!!! Oh no! <color=#ff2222>▼</color>",
                "I should be back at home by now! Sowwy, I have to leave!! <color=#ff2222>▼</color>",
                "(I’ll never forget you… Kristy.) <color=#ff2222>▼</color>",

                "*Todd leaves in a hurry*"};

                return messages100;

            case 20:
                string[] messages20 = { "Welcome to Crayun Town - Where Colors Gather Like ButterFlies" };
                return messages20;
            case 101:
                string[] messages101 = {"Congrats, Kristy! ^_^<color=#ff2222>▼</color>",
                    "You won your first kamibattle!<color=#ff2222>▼</color>",
                    "We're all proud here at PRISM.<color=#ff2222>▼</color>",
                    "Since you're in a prototype, i'm afraid to tell it's over for now. Now go rest :)<color=#ff2222>▼</color>",
                    "Uh? What do I mean with 'prototype'?<color=#ff2222>▼</color>",
                    "GOD DAMMIT SIENS! What have I told you about breaking the fourth wall??<color=#ff2222>▼</color>",
                    "    OMG, I'm so sorry!!!!"};

                return messages101;

            default:
                Debug.Log("Inexistent conversation");
                return null;

        }

    }

}
