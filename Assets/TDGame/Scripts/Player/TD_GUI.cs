using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
/// <summary>
/// This class is in charge of all if the GUI elements and their INteractivity. EDIT: Should probably be changed to a canvas instead of doing is manually
/// </summary>
public class TD_GUI : MonoBehaviour
{

    public bool foundFloor;
    float guiHeight;
    ObjectShooter m_ObjectShooter;

    private void Start()
    {
        // cahce the shooty bang bang component
        m_ObjectShooter = GetComponent<ObjectShooter>();
    }

    // On GUI function, draws shit every frame
    void OnGUI()
    {

        // some values
        guiHeight = Screen.height / 5;
        var buttonWidth = Screen.width / 2;

        // if im still placing the board
        if (!foundFloor)
        {
            // only duispaly this message
            GUI.Box(new Rect(Screen.width - buttonWidth * 1.5f, Screen.height * 0.05f, buttonWidth, guiHeight / 2), "Please look around you to find a suitable surface and select the surface");

            return;

        }



        // in game show the switch  mode button
        if (GUI.Button(new Rect(0, Screen.height - guiHeight, buttonWidth, guiHeight), "SwitchMode"))
        {

            m_ObjectShooter.switchMode();
        }

        if (m_ObjectShooter.mode == ObjectShooter.fireMode.single)
        {
            // display fire  button
            if (GUI.Button(new Rect(Screen.width - buttonWidth, Screen.height - guiHeight, buttonWidth, guiHeight), "Fire Single!"))
            {
              
                m_ObjectShooter.RequestFire();
            }
        }
        else if (m_ObjectShooter.mode == ObjectShooter.fireMode.all)
        {
            // display fire  button
            if (GUI.Button(new Rect(Screen.width - buttonWidth, Screen.height - guiHeight, buttonWidth, guiHeight), "Cast Area Spell!"))
            {
                // var cam = Camera.main;
                // var v = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, cam.nearClipPlane));
                // request fire in the object shooter
                m_ObjectShooter.RequestFire();
            }


        }
        else if (m_ObjectShooter.mode == ObjectShooter.fireMode.placing)
        {


            // display fire  button
            if (GUI.Button(new Rect(Screen.width - buttonWidth, Screen.height - guiHeight, buttonWidth, guiHeight), "Place Wall"))
            {

                if (!m_ObjectShooter.placing)
                {
                    m_ObjectShooter.RequestPlacing();
                }
                else
                {
                    m_ObjectShooter.finalizePlacing();
                }
            
            }



        }
        else if (m_ObjectShooter.mode == ObjectShooter.fireMode.defensive)
        {


            // display fire  button
            if (GUI.Button(new Rect(Screen.width - buttonWidth, Screen.height - guiHeight, buttonWidth, guiHeight), "Place Barrier"))
            {

                if (!m_ObjectShooter.placing)
                {
                    m_ObjectShooter.RequestPlacing();
                }
                else
                {
                    m_ObjectShooter.finalizePlacing();
                }

            }



        }






    }


}
