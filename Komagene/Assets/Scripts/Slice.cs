using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Slice : MonoBehaviour
{
    [SerializeField] List<ItemSO> slicedObjects;
    [SerializeField] private VoidEvent onSliceToggle;


    [SerializeField] private bool playerHere;


    void Update()
    {
        SliceObject();
    }

    void SliceObject()
    {
        if (playerHere && Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (this.transform.childCount > 0 && transform.GetChild(0).GetComponent<Item>().ItemData.CanSlice)
            {
                onSliceToggle.Raise();
                Invoke(nameof(Destroy), 3f);
            }
        }
    }

    void Destroy()
    {
        onSliceToggle.Raise();
        string newObjectID = transform.GetChild(0).GetComponent<Item>().ItemData.ItemID + "D";
        CreateNewObject(newObjectID);
        Debug.Log("BEFORE: " + transform.GetChild(0).GetComponent<Item>().ItemData.ItemID + "AFTER: " + newObjectID);
        Destroy(transform.GetChild(0).gameObject);
        
    }

    void CreateNewObject(string objID)
    {
        
        foreach (var item in slicedObjects)
        {
            
            if (item.ItemID == objID)
            {
                GameObject slicedObject = Instantiate(item.prefab);
                slicedObject.transform.parent = this.transform;
                slicedObject.transform.position = new Vector3(transform.position.x, transform.position.y + 0.38f, transform.position.z);

                return;
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerHere = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        playerHere = false;
    }

}
