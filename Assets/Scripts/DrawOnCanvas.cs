using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// While drawing on the UI image, this class rapidly instantiates spheres on the car's body's position
/// I'm sorry for repedeately used code
/// </summary>
public class DrawOnCanvas : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerExitHandler
{
    public Transform frontWheel;
    public Transform backWheel;

    public GameObject generationUnit;
    public float unitSpawnSeperationConstant = 0.01f;

    private Vector3 lastMousePosOnDrag;

    private Transform carRoot;
    private Transform carBody;
    private float carInitialPosY;
    private float carInitialRotationZ;

    private float spawnPositionX = -1f;
    private float spawnPositionY = 0.5f;

    public static bool touchOnUIElement;

    private void Start()
    {
        carBody = GameObject.Find("Body").transform;
        carRoot = GameObject.Find("Car").transform;

        carInitialPosY = carRoot.transform.localPosition.y;

        carInitialRotationZ = carRoot.transform.rotation.z;

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        touchOnUIElement = true;

        //Raise the car 2 units in the air
        carRoot.transform.localPosition += new Vector3(0, carInitialPosY, 0);

        //Make z rotation 0 
        carRoot.transform.rotation = Quaternion.Euler(new Vector3(carRoot.transform.rotation.x, carRoot.transform.rotation.y, carInitialRotationZ));

        //Destroy previously drawn car
        foreach (Transform child in carBody)
        {
            Destroy(child.gameObject);
        }

        Time.timeScale = 0;

        GameObject instantiatedSphere = Instantiate(generationUnit, transform.position, Quaternion.identity);
        instantiatedSphere.transform.SetParent(carBody);
        instantiatedSphere.transform.localPosition = new Vector3(spawnPositionX, spawnPositionY, 0);

        //Relocate back wheel to the start of the drawing
        backWheel.localPosition = instantiatedSphere.transform.localPosition - new Vector3(0, 0.25f, 0) - new Vector3(-1.36f, +1.36f, 0);

        lastMousePosOnDrag = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (touchOnUIElement)
        {
            spawnPositionX += (eventData.position.x - lastMousePosOnDrag.x) * unitSpawnSeperationConstant;
            spawnPositionY += (eventData.position.y - lastMousePosOnDrag.y) * unitSpawnSeperationConstant;

            GameObject instantiatedSphere = Instantiate(generationUnit, transform.position, Quaternion.identity);
            instantiatedSphere.transform.SetParent(carBody);
            instantiatedSphere.transform.localPosition = new Vector3(spawnPositionX, spawnPositionY, 0);

            lastMousePosOnDrag = eventData.position;
        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        spawnPositionX += (eventData.position.x - lastMousePosOnDrag.x) * unitSpawnSeperationConstant;
        spawnPositionY += (eventData.position.y - lastMousePosOnDrag.y) * unitSpawnSeperationConstant;

        GameObject instantiatedSphere = Instantiate(generationUnit, transform.position, Quaternion.identity);
        instantiatedSphere.transform.SetParent(carBody);
        instantiatedSphere.transform.localPosition = new Vector3(spawnPositionX, spawnPositionY, 0);

        frontWheel.localPosition = instantiatedSphere.transform.localPosition - new Vector3(0, 0.25f, 0) - new Vector3(-1.36f, +1.36f, 0);

        Time.timeScale = 1f;

        spawnPositionX = -1f;
        spawnPositionY = 0.5f;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        touchOnUIElement = false;

        spawnPositionX += (eventData.position.x - lastMousePosOnDrag.x) * unitSpawnSeperationConstant;
        spawnPositionY += (eventData.position.y - lastMousePosOnDrag.y) * unitSpawnSeperationConstant;

        GameObject instantiatedSphere = Instantiate(generationUnit, transform.position, Quaternion.identity);
        instantiatedSphere.transform.SetParent(carBody);
        instantiatedSphere.transform.localPosition = new Vector3(spawnPositionX, spawnPositionY, 0);

        //Relocate front wheel to the end of the drawing
        frontWheel.localPosition = instantiatedSphere.transform.localPosition - new Vector3(0, 0.25f, 0) - new Vector3(-1.36f, +1.36f, 0);

        Time.timeScale = 1f;

        spawnPositionX = -1f;
        spawnPositionY = 0.5f;
    }
}
