//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Basic throwable object
//
//=============================================================================

using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
    //-------------------------------------------------------------------------
    [RequireComponent(typeof(Interactable))]
    [RequireComponent(typeof(Rigidbody))]
    public class Moveable : MonoBehaviour
    {
        [EnumFlags]
        [Tooltip("The flags used to attach this object to the hand.")]
        public Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.TurnOnKinematic;

        [Tooltip("The local point which acts as a positional and rotational offset to use while held")]
        public Transform attachmentOffset;

        [Tooltip("How fast must this object be moving to attach due to a trigger hold instead of a trigger press? (-1 to disable)")]
        public float catchingSpeedThreshold = -1;

        public ReleaseStyle releaseVelocityStyle = ReleaseStyle.GetFromHand;

        [Tooltip("The time offset used when releasing the object with the RawFromHand option")]
        public float releaseVelocityTimeOffset = -0.011f;

        public float scaleReleaseVelocity = 1.1f;

        [Tooltip("The release velocity magnitude representing the end of the scale release velocity curve. (-1 to disable)")]
        public float scaleReleaseVelocityThreshold = -1.0f;
        [Tooltip("Use this curve to ease into the scaled release velocity based on the magnitude of the measured release velocity. This allows greater differentiation between a drop, toss, and throw.")]
        public AnimationCurve scaleReleaseVelocityCurve = AnimationCurve.EaseInOut(0.0f, 0.1f, 1.0f, 1.0f);

        [Tooltip("When detaching the object, should it return to its original parent?")]
        public bool restoreOriginalParent = false;



        protected VelocityEstimator velocityEstimator;
        protected bool attached = false;
        protected float attachTime;
        protected Vector3 attachPosition;
        protected Quaternion attachRotation;
        protected Transform attachEaseInTransform;

        public UnityEvent onPickUp;
        public UnityEvent onDetachFromHand;
        public HandEvent onHeldUpdate;


        protected RigidbodyInterpolation hadInterpolation = RigidbodyInterpolation.None;

        protected new Rigidbody rigidbody;

        [HideInInspector]
        public Interactable interactable;

        public CommandManager commandManager = null;
        private Vector3 oldPos;
        private Quaternion oldRot;

        //-------------------------------------------------
        protected virtual void Awake()
        {
            velocityEstimator = GetComponent<VelocityEstimator>();
            interactable = GetComponent<Interactable>();

            rigidbody = GetComponent<Rigidbody>();
            rigidbody.maxAngularVelocity = 50.0f;


            if (attachmentOffset != null)
            {
                // remove?
                //interactable.handFollowTransform = attachmentOffset;
            }

            oldPos = transform.position;
            oldRot = transform.rotation;
        }

        private void Start()
        {
            GameObject managerObj = GameObject.Find("CommandManager");
            if (managerObj != null)
                commandManager = managerObj.GetComponent<CommandManager>();
            else Debug.Log("could not find command manager");
        }

        //-------------------------------------------------
        protected virtual void OnHandHoverBegin(Hand hand)
        {
            bool showHint = false;

            // "Catch" the throwable by holding down the interaction button instead of pressing it.
            // Only do this if the throwable is moving faster than the prescribed threshold speed,
            // and if it isn't attached to another hand
            if (!attached && catchingSpeedThreshold != -1)
            {
                float catchingThreshold = catchingSpeedThreshold * SteamVR_Utils.GetLossyScale(Player.instance.trackingOriginTransform);

                GrabTypes bestGrabType = hand.GetBestGrabbingType();

                if (bestGrabType != GrabTypes.None)
                {
                    if (rigidbody.velocity.magnitude >= catchingThreshold)
                    {
                        hand.AttachObject(gameObject, bestGrabType, attachmentFlags);
                        showHint = false;
                    }
                }
            }

            if (showHint)
            {
                hand.ShowGrabHint();
            }
        }


        //-------------------------------------------------
        protected virtual void OnHandHoverEnd(Hand hand)
        {
            hand.HideGrabHint();
        }


        //-------------------------------------------------
        protected virtual void HandHoverUpdate(Hand hand)
        {
            GrabTypes startingGrabType = hand.GetGrabStarting();

            if (startingGrabType != GrabTypes.None)
            {
                hand.AttachObject(gameObject, startingGrabType, attachmentFlags, attachmentOffset);
                hand.HideGrabHint();
            }
        }

        //-------------------------------------------------
        protected virtual void OnAttachedToHand(Hand hand)
        {
            //Debug.Log("<b>[SteamVR Interaction]</b> Pickup: " + hand.GetGrabStarting().ToString());

            hadInterpolation = this.rigidbody.interpolation;

            attached = true;

            onPickUp.Invoke();

            hand.HoverLock(null);

            rigidbody.interpolation = RigidbodyInterpolation.None;

            if (velocityEstimator != null)
                velocityEstimator.BeginEstimatingVelocity();

            attachTime = Time.time;
            attachPosition = transform.position;
            attachRotation = transform.rotation;

            oldPos = transform.position;
            oldRot = transform.rotation;
        }


        //-------------------------------------------------
        protected virtual void OnDetachedFromHand(Hand hand)
        {
            attached = false;

            onDetachFromHand.Invoke();

            hand.HoverUnlock(null);

            rigidbody.interpolation = hadInterpolation;

            commandManager.Execute(new MoveCommand(transform.position, transform.rotation, oldPos, oldRot, this.gameObject));
        }

        //-------------------------------------------------
        protected virtual void HandAttachedUpdate(Hand hand)
        {


            if (hand.IsGrabEnding(this.gameObject))
            {
                hand.DetachObject(gameObject, restoreOriginalParent);

                // Uncomment to detach ourselves late in the frame.
                // This is so that any vehicles the player is attached to
                // have a chance to finish updating themselves.
                // If we detach now, our position could be behind what it
                // will be at the end of the frame, and the object may appear
                // to teleport behind the hand when the player releases it.
                //StartCoroutine( LateDetach( hand ) );
            }

            if (onHeldUpdate != null)
                onHeldUpdate.Invoke(hand);
        }


        //-------------------------------------------------
        protected virtual IEnumerator LateDetach(Hand hand)
        {
            yield return new WaitForEndOfFrame();

            hand.DetachObject(gameObject, restoreOriginalParent);
        }


        //-------------------------------------------------
        protected virtual void OnHandFocusAcquired(Hand hand)
        {
            gameObject.SetActive(true);

            if (velocityEstimator != null)
                velocityEstimator.BeginEstimatingVelocity();
        }


        //-------------------------------------------------
        protected virtual void OnHandFocusLost(Hand hand)
        {
            gameObject.SetActive(false);

            if (velocityEstimator != null)
                velocityEstimator.FinishEstimatingVelocity();
        }
    }

    public enum ReleaseStyle
    {
        NoChange,
        GetFromHand,
        ShortEstimation,
        AdvancedEstimation,
    }
}