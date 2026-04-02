using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Gun : MonoBehaviour
{
    private float damage = 25f;
    private float maxDistance = 50f;
    private LineRenderer bulletLine;
    private AudioSource audio;

    public Transform shotPivot;
    public ParticleSystem gunParticle;
    public Light pointLight;
    public LayerMask layerMask;
    public AudioClip shotClip;

    private Coroutine coShot;
    private float timeBetFire = 0.1f;
    private float lastFireTime = 0f;

    private void Awake()
    {
        bulletLine = GetComponent<LineRenderer>();
        audio = GetComponent<AudioSource>();
        coShot = null;
        pointLight.enabled = false;
    }

    private void Start()
    {
        bulletLine.enabled = false;
    }
    public void Fire()
    {
        if (Time.time > lastFireTime + timeBetFire)
        {
            lastFireTime = Time.time;
            Shot();
        }
    }
    public void Shot()
    {
        Vector3 hitPosition = Vector3.zero;

        Ray ray = new Ray(shotPivot.position, shotPivot.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layerMask))
        {
            hitPosition = hit.point;

            var target = hit.collider.GetComponent<IDamageable>();
            if (target != null)
            {
                target.OnDamage(damage, hit.point, hit.normal);
            }
        }
        else
        {
            hitPosition = shotPivot.position + shotPivot.forward * maxDistance;
        }
        if (coShot != null)
        {
            StopCoroutine(coShot);
            bulletLine.enabled = false;
            pointLight.enabled = false;
            coShot = null;
        }
        coShot = StartCoroutine(CoShotEffect(hitPosition));
    }

    private IEnumerator CoShotEffect(Vector3 hitPosition) // 발사된 후 플레이어를 안 따라와서 로컬 포지션으로 할지 고민
    {
        gunParticle.Play();
        audio.PlayOneShot(shotClip);
        bulletLine.SetPosition(0, shotPivot.position);
        bulletLine.SetPosition(1, hitPosition);

        bulletLine.enabled = true;
        pointLight.enabled = true;
        yield return new WaitForSeconds(0.03f);

        bulletLine.enabled = false;
        pointLight.enabled = false;
        coShot = null;
    }
}
