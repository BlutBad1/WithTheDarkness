using MyConstants.WeaponConstants;
using PoolableObjectsNS;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UtilitiesNS;
using WeaponNS.ShootingWeaponNS.Inspector;
namespace WeaponNS.ShootingWeaponNS
{
    [Serializable]
    public struct DamageDecalObject
    {
        public static List<string> Tags = UnityEditorInternal.InternalEditorUtility.tags.ToList();
        [EnumMask]
        public WeaponEntity WeaponEntity;
        [ListToMultiplePopup(typeof(DamageDecalObject), "Tags")]
        public int Tag;
        public LayerMask LayerMask;
        [DamageDecalObjectPopupField]
        public string DamageDecalType;
    }
    public class DamageDecalDataBase : MonoBehaviour
    {
        public DamageDecalObject[] DamageDecalObjects;
        protected DamageDecalPoolsManager DamageDecalPoolMng;
        protected virtual void Start()
        {
            DamageDecalPoolMng = GameObject.FindObjectOfType<DamageDecalPoolsManager>();
        }
        public void MakeBulletHoleByInfo(RaycastHit hitInfo, Vector3 rayCastOrigin, WeaponEntity weaponType)
        {
            DamageDecalObject[] damageDecals = new DamageDecalObject[0];
            DamageDecalObject[] weaponMatched = Array.FindAll(DamageDecalObjects, x => ((int)x.WeaponEntity & (1 << Utilities.GetIndexOfElementInEnum(weaponType))) != 0);
            DamageDecalObject[] tagMatched = Array.FindAll(DamageDecalObjects, x => (x.Tag & (1 << DamageDecalObject.Tags.IndexOf(hitInfo.collider.gameObject.tag))) != 0);
            DamageDecalObject[] layerMatched = Array.FindAll(DamageDecalObjects, x => x.LayerMask == (x.LayerMask | (1 << hitInfo.collider.gameObject.layer)));
            damageDecals = weaponMatched.Intersect(tagMatched).Intersect(layerMatched).ToArray();
            if (damageDecals == null || damageDecals.Length == 0)
                damageDecals = weaponMatched.Intersect(tagMatched).ToArray();
            if (damageDecals == null || damageDecals.Length == 0)
                damageDecals = weaponMatched.Intersect(layerMatched).ToArray();
            if (damageDecals == null || damageDecals.Length == 0)
                damageDecals = weaponMatched;
            if (damageDecals == null || damageDecals.Length == 0)
                damageDecals = new DamageDecalObject[1] { new DamageDecalObject { DamageDecalType = MainWeaponConstants.DEFAULT_DAMAGE_DECAL } };
            if (damageDecals.Length > 1)
                damageDecals = damageDecals.Where(x => x.DamageDecalType != MainWeaponConstants.DEFAULT_DAMAGE_DECAL).ToArray();
            int randomNumber = UnityEngine.Random.Range(0, damageDecals.Length);
            CreateBulletHole(damageDecals[randomNumber].DamageDecalType, hitInfo, rayCastOrigin);
        }
        protected virtual void CreateBulletHole(string damageDecalName, RaycastHit hitInfo, Vector3 rayCastOrigin)
        {
            GameObject damageDecal = DamageDecalPoolMng.GetObject(damageDecalName);
            damageDecal.SetActive(false);
            damageDecal.SetActive(true);
            for (int i = 0; i < damageDecal.transform.childCount; i++)
            {
                damageDecal.transform.GetChild(i).gameObject.SetActive(false);
                damageDecal.transform.GetChild(i).gameObject.SetActive(true);
            }
            damageDecal.transform.parent = null;
            //Rotation
            Quaternion originalRot = DamageDecalPoolMng.GetPrefab(damageDecalName).transform.rotation;
            damageDecal.transform.rotation = Quaternion.identity;
            Vector3 playerToHitDirection = (hitInfo.point - rayCastOrigin).normalized;
            Vector3 referenceDirection = Vector3.up;  // By default it is world up
            if (Mathf.Abs(Vector3.Dot(hitInfo.normal, Vector3.up)) > 0.9f)
                referenceDirection = playerToHitDirection;  // Change for player's wall direction 
            Vector3 projectedDirection = referenceDirection - Vector3.Dot(referenceDirection, hitInfo.normal) * hitInfo.normal;
            Quaternion targetRotation = Quaternion.LookRotation(hitInfo.normal, projectedDirection.normalized);
            damageDecal.transform.rotation = targetRotation * Quaternion.Inverse(originalRot);
            //Position and new parent
            DefinePositionAndParent(damageDecal, hitInfo);
            if (!damageDecal.TryGetComponent(out ParticleSystem particleSystem))
            {
                if (damageDecal.GetComponentInChildren<ParticleSystem>())
                    damageDecal.GetComponentInChildren<ParticleSystem>().Play();
            }
            else
                particleSystem.Play();
        }
        protected void DefinePositionAndParent(GameObject damageDecal, RaycastHit hitInfo)
        {
            SkinnedMeshRenderer renderer = Utilities.GetClosestComponentInGameObject<SkinnedMeshRenderer>(hitInfo.point, hitInfo.collider.gameObject.transform.root.gameObject);//HARDCODE
            //renderer = renderer == null ? Utilities.GetClosestComponentInGameObject<SkinnedMeshRenderer>(hitInfo.point, hitInfo.rigidbody.gameObject) : renderer;//HARDCODE
            if (renderer)
            {
                Vector3 localTargetPoint = renderer.transform.InverseTransformPoint(hitInfo.point);
                damageDecal.transform.position = renderer.transform.TransformPoint(localTargetPoint);
            }
            else
                damageDecal.transform.position = hitInfo.point + (hitInfo.normal * 0.0001f);
            damageDecal.transform.parent = hitInfo.collider.gameObject.transform;
        }
    }
}