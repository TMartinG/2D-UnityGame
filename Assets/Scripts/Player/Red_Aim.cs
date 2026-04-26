using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Red_Aim : MonoBehaviour
{
    public Camera cam;
    public LineRenderer lineRenderer;
    public Transform firePoint;
    public Quaternion rotation;
    public Shader shader;
    //public GameObject startVFX;
    public GameObject endVFX;
    private bool vfxPlaying = false;
    private int groundMask;
    private int enemyMask;
    private int mirrorMask;

    private Light_Red red;

    public List<ParticleSystem> particles = new List<ParticleSystem>();
    float damageTimer = 0f;

    [SerializeField] int maxReflections = 10;
    [SerializeField] float maxDistance = 200f;
    bool finalHit = false;
    Vector2 finalPoint = Vector2.zero;

    void Start()
    {   

        FillLists();
        groundMask = LayerMask.GetMask("Ground");
        enemyMask = LayerMask.GetMask("Enemy");
        mirrorMask = LayerMask.GetMask("Mirror");
        red = GetComponentInParent<Light_Red>();

    }


    public void UpdateLaser()
    {


        finalHit = false;

        /*var mousePos = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, mousePos);*/

        // ---------------------  Mozgás behatások ---------------------


        // -------------------------------------------------------------

        var mousePos = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);

        

        //-------------------Egér végéig van csak a lézer ---------------------------------------

        // jelenlegi végpont
        Vector3 currentEnd = lineRenderer.GetPosition(1);

        // interpoláció a cél felé
        Vector3 smoothedEnd = Vector3.Lerp(
            currentEnd,
            mousePos,
            Time.deltaTime * red.playerMovement.GetHeadMoveSpeed // sebesség faktor
        );

        //--------------------------------------------------------------------------------------

        

        //-------------------Statikus hossszúságú a lézer --------------------------------------
        // Ilyenkor a RayCast-nál át kell írni a dircetion.magnitude-ot a hosszúság értékére!!!!

       /* Vector2 direction2 = (mousePos - (Vector2)firePoint.position).normalized;

        // jelenlegi végpont
        Vector3 currentEnd = lineRenderer.GetPosition(1);

        // cél végpont mindig fix hosszban az irányba
        Vector3 targetEnd = firePoint.position + (Vector3)direction2 * 200f;

        // interpoláció a cél felé
        Vector3 smoothedEnd = Vector3.Lerp(
            currentEnd,
            targetEnd,
            Time.deltaTime * playerMovement.Get_headMoveSpeed // sebesség faktor
        );*/

        /*Vector3 smoothedDir = Vector3.Lerp(
        lineRenderer.GetPosition(1) - firePoint.position,
        targetEnd - firePoint.position,
        Time.deltaTime * playerMovement.Get_headMoveSpeed
        );*/

        //------------------------------------------------------------------------------------

        Vector2 start = firePoint.position;
        Vector2 direction = mousePos - (Vector2)firePoint.position;
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, start);

        //lineRenderer.SetPosition(1, smoothedEnd);

        //float distance = Vector2.Distance(firePoint.position, smoothedEnd);
        int mask = groundMask | enemyMask | mirrorMask;

        //RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction.normalized, distance, mask); //statikusnál a distance 200f mondjuk

        int reflections = 0;
        damageTimer += Time.deltaTime;

        while (reflections < maxReflections)
        {
            RaycastHit2D hit = Physics2D.Raycast(start, direction.normalized, maxDistance, mask);

            if (!hit)
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, start + direction.normalized * maxDistance);
                break;
            }

            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
            Enemy_Base enemy = hit.collider.GetComponentInParent<Enemy_Base>();

            if (hit.collider.CompareTag("Mirror"))
            {
                direction = Vector2.Reflect(direction.normalized, hit.normal);
                start = hit.point + direction * 0.05f; // kis offset hogy ne ugyanabba a colliderbe lőjön
                reflections++;
                continue;
            }
            else if (hit.collider.CompareTag("Enemy"))
            {
                finalHit = true;
                finalPoint = hit.point;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);

                //damageTimer += Time.deltaTime;  (?ezzel is lehet?)

                if (enemy != null)
                {
                    if (red.fireEnabled == true)
                    {
                        red.ApplyEffect(hit.transform.gameObject, Time.deltaTime * 2f);
                    }
                    if (red.iceEnabled == true)
                    {
                        red.ApplyEffect(hit.transform.gameObject, Time.deltaTime * 2f);
                    }

                    //damageTimer += Time.deltaTime;  (?ezzel is lehet?)

                    if (damageTimer >= red.GetFireRate())
                    {
                        enemy.GetDamaged((short)red.GetDamage(), transform.position);
                        damageTimer = 0f;
                    }
                }
                break;
                
            }
            else if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Platform"))
            {
                finalHit = true;
                finalPoint = hit.point;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                break;
            }
            else
            {
                break;
            }

           
        }
        if (finalHit)
        {
            endVFX.transform.position = finalPoint;
            endVFX.SetActive(true);

            if (!vfxPlaying)
                StartVFX();
        }
        else
        {
            if (vfxPlaying)
                StopVFX();

        }
    }

    public void RotateMouse()
    {
        Vector2 direction = cam.ScreenToWorldPoint(Input.mousePosition).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rotation.eulerAngles = new Vector3(0, 0, angle);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * red.playerMovement.GetHeadMoveSpeed);
    }

    void FillLists()
    {

        for (int i = 0; i < endVFX.transform.childCount; i++)
        {
            var ps = endVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (ps != null)
            {
                particles.Add(ps);
            }
        }
    }

    public void StopVFX()
    {
        foreach (var ps in particles) ps.Stop();
        endVFX.SetActive(false);
        vfxPlaying = false;
    }

    public void StartVFX()
    {
        foreach (var ps in particles) ps.Play();
        vfxPlaying = true;
    }

    public void ResetLaser()
    {
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, firePoint.position);
    }
}
