using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;
using Math = ExMath;

public class XORshiftModule : MonoBehaviour {

   public KMBombInfo Bomb;
   public KMAudio Audio;

   public KMSelectable[] Buttons;
   public TextMesh DisplayText;

   static int ModuleIdCounter = 1;
   int ModuleId;
   private bool ModuleSolved;

   public int ButtonToPress = 0;

   void Awake () {
      ModuleId = ModuleIdCounter++;
      GetComponent<KMBombModule>().OnActivate += Activate;

      foreach (KMSelectable Button in Buttons) {
         Button.OnInteract += delegate () { ButtonPress(Button); return false; };
      }
   }

   void ButtonPress(KMSelectable Button)
   {
      if (ModuleSolved){
         return;
      }
      int buttonIndex = Array.IndexOf(Buttons, Button);

      if (buttonIndex == ButtonToPress)
      {
         ModuleSolved = true;
         Solve();
      }
      else
      {
         Strike();
      }
   }

   void OnDestroy () { 
      
   }

   void Activate () { 

   }

   void Start () { 
         string[] colors = { "blue", "red", "yellow", "magenta", "green" };
         int color = Rnd.Range(0, 5); // Random color index from 0 to 4
         var initialnum = Bomb.GetSerialNumber().Select(c => c >= 'A' && c <= 'Z' ? c - 'A' + 1 : c - '0').Sum(); //add all values in the serial number together
         var batterycount = Bomb.GetBatteryCount();
         int shift = 0;
         int displaynum = 0;

         if (color == 0) // blue
               if (batterycount <= 1)
               shift = 1;
               else if (batterycount == 2)
               shift = 4;
               else if (batterycount == 3)
               shift = 0;
               else if (batterycount == 4)
               shift = 3;
               else
               shift = 2;
         else if (color == 1) // red
            if (batterycount <= 1)
               shift = 3;
            else if (batterycount == 2)
               shift = 2;
            else if (batterycount == 3)
               shift = 4;
            else if (batterycount == 4)
               shift = 3;
            else
               shift = 3;
         else if (color == 2) //yellow
            if (batterycount <= 1)
               shift = 2;
            else if (batterycount == 2)
               shift = 3;
            else if (batterycount == 3)
               shift = 1;
            else if (batterycount == 4)
               shift = 4;
            else
               shift = 2;
         else if (color == 3) //magenta
            if (batterycount <= 1)
               shift = 2;
            else if (batterycount == 2)
               shift = 3;
            else if (batterycount == 3)
               shift = 4;
            else if (batterycount == 4)
               shift = 2;
            else
               shift = 1;
         else
            if (batterycount <= 1)
               shift = 3;
            else if (batterycount == 2)
               shift = 2;
            else if (batterycount == 3)
               shift = 4;
            else if (batterycount == 4)
               shift = 3;
            else
               shift = 4;

         Debug.LogFormat("[XORShift #{0}] The color of the number is {1}, and the battery count is {2}. Shift offset = {3}.", ModuleId, colors[color], batterycount, shift);

         int secondnum = initialnum >> shift;

         Debug.LogFormat("[XORShift #{0}] The initial number after adding together the values of the Serial # was {1}. Shifting by amount {2}, resulted in the second number being {3}.", ModuleId, initialnum, shift, secondnum);

         int finalnum = initialnum^secondnum;
         
         Debug.LogFormat("[XORShift #{0}] Preforming XOR on initial number {1} and second number {2}, resulted in {3}.", ModuleId, initialnum, secondnum, finalnum);

         int chance = Rnd.Range(0, 10);

         if (chance >= 5){
            displaynum = finalnum;
            ButtonToPress = 0;
            Debug.LogFormat("[XORShift #{0}] The resulting number {1} matches the display number {2}. Expecting answer 'Yes'.", ModuleId, finalnum, displaynum);
         }
         else{
            displaynum = Rnd.Range(0, 100);
            ButtonToPress = 1;
            Debug.LogFormat("[XORShift #{0}] The resulting number {1} does not match the display number {2}. Expecting answer 'No'", ModuleId, finalnum, displaynum);
         }

         DisplayText.color = GetColorByName(colors[color]);
         DisplayText.text = displaynum.ToString();
   }


   Color GetColorByName(string name)
   {
        if (name.Equals("red"))
            return Color.red;
        else if (name.Equals("blue"))
            return Color.blue;
        else if (name.Equals("magenta"))
            return Color.magenta;
        else if (name.Equals("yellow"))
            return Color.yellow;
        else if (name.Equals("green"))
            return Color.green;
        else
            return Color.white; //never gonna happen
   }


   void Update () { 

   }

   void Solve () {
      GetComponent<KMBombModule>().HandlePass();
   }

   void Strike () {
      GetComponent<KMBombModule>().HandleStrike();
   }

#pragma warning disable 414
   private readonly string TwitchHelpMessage = @"Use !{0} to do something.";
#pragma warning restore 414

   IEnumerator ProcessTwitchCommand (string Command) {
      yield return null;
   }

   IEnumerator TwitchHandleForcedSolve () {
      yield return null;
   }
}
