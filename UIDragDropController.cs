using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#region delegates
public delegate void OnItemPicked(GameObject pickedItem);
public delegate void OnItemDropped(GameObject pickedItem, GameObject droppedItem);
public delegate void OnTouchUp(Vector2 position, int fingerID);
public delegate void OnTouchDown(Vector2 position, int fingerID);
public delegate void OnTouchDrag(Vector2 position, int fingerID);
public delegate void OnTouchHeld(Vector2 position, int fingerID);
public delegate void OnTouchExit(Vector2 position, int fingerID);
#endregion

public class UIDragDropController : MonoBehaviour 
{
    public OnItemPicked _OnItemPickedEvent;
    public OnItemDropped _OnItemDroppedEvent;
    public OnTouchUp _OnTouchUpEvent;
    public OnTouchDown _OnTouchDownEvent;
    public OnTouchDrag _OnTouchDragEvent;
    public OnTouchHeld _OnTouchHeldEvent;
    public OnTouchExit _OnTouchExitEvent;

    public LayerMask _PickLayerMask;
    public LayerMask _DropLayerMask;

    private List<RaycastResult> mRaycastResults = new List<RaycastResult>();

    private GameObject mPickedItem;
    private GameObject mDroppedItem;

    private int mPickedFingerID;
    public int pPickedFingerID
    {
        get{ return mPickedFingerID; }
    }
    private Vector2 mPickedPosition;
    public Vector2 pPickedPosition
    {
        get{ return mPickedPosition; }
    }

	void Update () 
    {
#if UNITY_EDITOR
        if(Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {
            if(Input.GetMouseButtonDown(0))
                OnTouchDown(Input.mousePosition, 0);
            if(Input.GetMouseButtonUp(0))
                OnTouchUp(Input.mousePosition, 0);
            if(Input.GetMouseButton(0))
                OnDrag(Input.mousePosition, 0);
        }
#else
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                    OnTouchDown(touch.position, touch.fingerId);
                if (touch.phase == TouchPhase.Ended)
                    OnTouchUp(touch.position, touch.fingerId);
                if (touch.phase == TouchPhase.Moved)
                    OnDrag(touch.position, touch.fingerId);
                if (touch.phase == TouchPhase.Stationary)
                    OnTouchHeld(touch.position, touch.fingerId);
                if (touch.phase == TouchPhase.Canceled)
                    OnTouchExit(touch.position, touch.fingerId);
            }
        }
#endif
	}

    void OnTouchDown(Vector2 position, int fingerID = -1)
    {
        if (_OnTouchDownEvent != null)
            _OnTouchDownEvent(position, fingerID);

        if(mPickedItem == null)
            HandleItemPickUp(position, fingerID);
    }

    void OnTouchUp(Vector2 position, int fingerID = -1)
    {
        if (_OnTouchUpEvent != null)
            _OnTouchUpEvent(position, fingerID);

        if(fingerID == mPickedFingerID && mPickedItem != null)
            HandleItemDrop(position, fingerID);
    }

    void OnDrag(Vector2 position, int fingerID = -1)
    {
        if (_OnTouchDragEvent != null)
            _OnTouchDragEvent(position, fingerID);

        if (mPickedItem != null && fingerID == mPickedFingerID)
            mPickedItem.transform.position = position;
    }

    void OnTouchHeld(Vector2 position, int fingerID = -1)
    {
        if (_OnTouchHeldEvent != null)
            OnTouchHeld(position, fingerID);
    }

    void OnTouchExit(Vector2 position, int fingerID = -1)
    {
        if (_OnTouchExitEvent != null)
            _OnTouchExitEvent(position, fingerID);
    }

    void HandleItemPickUp(Vector2 position, int fingerID)
    {
        mPickedItem = CheckForUIItem(position, _PickLayerMask);
        if (mPickedItem != null)
        {
            mPickedItem.transform.SetAsLastSibling();
            mPickedFingerID = fingerID;

            if(_OnItemPickedEvent != null)
                _OnItemPickedEvent(mPickedItem);
        }
    }

    void HandleItemDrop(Vector2 position, int fingerID)
    {
        mDroppedItem = CheckForUIItem(position, _DropLayerMask);
        if (mPickedItem != null && mDroppedItem != null)
        {
            if (_OnItemDroppedEvent != null)
                _OnItemDroppedEvent(mPickedItem, mDroppedItem);
        }
        else if (mPickedItem != null && mDroppedItem == null)
        {
            mPickedItem.transform.position = mPickedPosition;
        }

        mPickedFingerID = -1;
        mPickedItem = null;
    }


    GameObject CheckForUIItem(Vector2 pos, LayerMask layermask)
    {
        mRaycastResults.Clear();
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = pos;
        EventSystem.current.RaycastAll(pointerData, mRaycastResults);

        if (mRaycastResults.Count > 0)
        {
            for (int i = 0; i < mRaycastResults.Count; i++)
                if ((layermask & 1 << mRaycastResults[i].gameObject.layer) == 1 << mRaycastResults[i].gameObject.layer)
                {
                    if(layermask == _PickLayerMask)
                        mPickedPosition = mRaycastResults[i].gameObject.transform.position;
                    
                    return mRaycastResults[i].gameObject;
                }
        }
        return null;
    }
}
