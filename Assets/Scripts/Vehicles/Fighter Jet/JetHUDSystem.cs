using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Vehicle.FighterJet;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Vehicle
{
    namespace FighterJet
    {
        public class JetHUDSystem : MonoBehaviour
        {
            private Camera playerCamera;
            public Behaviour[] HUDComponents;
            public MeshRenderer[] pitchLadderRenderers;

            private float maxDistance = Mathf.Infinity;

            [SerializeField]
            private Color HUDColour;
            [SerializeField]
            private Color warningColour;

            private Vector3 originalScale;
            private Vector3 tempPos;

            public static JetHUDSystem instance;

            void Awake()
            {
                if (instance)
                {
                    DestroyImmediate(gameObject);
                }
                else
                {
                    instance = this;
                }
            }

            void Start()
            {
                playerCamera = Camera.main;
                originalScale = transform.localScale;
                ColourHUD(HUDColour);
            }

            void Update()
            {
                SetHUDDepth();
            }

            void SetHUDDepth()
            {
                float distance = GazeRaycaster.GetRaycastHitDistance(maxDistance);

                // Set the new crosshair position - which is the current position multiplied by the distance of the object we hit
                tempPos.x = transform.position.x;
                tempPos.y = transform.position.y;
                tempPos.z = distance;

                //transform.position = tempPos;
                transform.position = playerCamera.transform.position + (playerCamera.transform.forward * distance);
                //transform.LookAt(playerCamera.transform.position);

                // Set the scale to the original scale multiplied by the distance of the object we hit
                transform.localScale = originalScale * distance;
            }

            /// <summary>
            /// Updates the UI Text to the Message supplied.
            /// </summary>
            public void UpdateText(Text text, string message)
            {
                text.text = message;
            }

            public IEnumerator AnimateText(Text t, string msg)
            {
                int i = 0;
                //t.text = "";
                while (i < msg.Length)
                {
                    t.text += msg[i++]; 
                    yield return new WaitForSeconds(.00005f);
                }

                yield return new WaitForSeconds(.5f);
            }

            public void ResetHUDColour()
            {
                ColourHUD(HUDColour);
            }

            public void WarningHUDColour()
            {
                ColourHUD(warningColour);
            }

            void ColourHUD(Color colour)
            {
                for (int i = 0; i < HUDComponents.Length; i++)
                {
                    Behaviour b = HUDComponents[i];

                    if (b.GetComponent<Image>())
                        b.GetComponent<Image>().color = colour;
                    if (b.GetComponent<Text>())
                        b.GetComponent<Text>().color = colour;
                    if (b.GetComponent<Material>())
                        b.GetComponent<Material>().color = colour;
                }

                pitchLadderRenderers[0].sharedMaterial.color = colour;
                pitchLadderRenderers[1].sharedMaterial.color = colour;
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(JetHUDSystem))]
public class HUDEditor : Editor
{
    JetHUDSystem hud = null;

    void OnEnable()
    {
        hud = (JetHUDSystem)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Colour HUD"))
        {
            hud.ResetHUDColour();
        }
    }
}
#endif

