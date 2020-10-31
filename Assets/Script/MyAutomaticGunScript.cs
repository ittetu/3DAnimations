using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyAutomaticGunScript : MonoBehaviour
{
    Animator anim;

    public float swayAmount = 0.02f;
    public float maxSwayAmount = 0.06f;
    public float swaySmoothValue = 4.0f;
    public float lastFired = 0f;
    public float fireRate;

    public Vector3 initialSwayPoint;

    public bool weaponSway;

    public bool isAiming;
    public bool isReloading;
    public bool isInspecting;

    public bool outOfAmmo;

    public Camera gunCamera;
    public float aimFov = 25.0f;
    public float fovSpeed = 15.0f;
    public float defaultFov = 40.0f;

    public int currentAmmo = 30;

    public Text currentAmmoText;
    public Text totalAmmoText;

    public bool enableMuzzleflash = true;
    public ParticleSystem muzzleParticles;
    public bool enableSparks = true;
    public ParticleSystem sparkParticles;
    public int minSparkEmission = 1;
    public int maxSparkEmission = 7;

    [Header("Muzzleflash Light Settings")]
    public Light muzzleflashLight;
    public float lightDuration = 0.02f;

    [Header("Audio Sources")]
    public AudioSource shootAudioSource;
    public AudioSource mainAudioSource;

    [Header("Grenade Setting")]
    public float grenadeSpawnDelay = 0.35f;

    //prefab spownpoint soundClipをリスト化
    [System.Serializable]
    public class prefabs
    {
        [Header("Prefabs")]
        public Transform bulletPrefab;
        public Transform casingPrefab;
        public Transform grenadePrefab;
    }
    public prefabs Prefabs;

    [System.Serializable]
    public class spawnpoints
    {
        [Header("Spawnpoints")]
        public Transform casingSpawnPoint;
        public Transform bulletSpawnPoint;
        public Transform grenadeSpawnPoint;
    }
    public spawnpoints Spawnpoints;

    [System.Serializable]
    public class soundClips
    {
        public AudioClip shootSound;
        public AudioClip takeOutSound;
        public AudioClip holsterSound;
        public AudioClip reloadSoundOutOfAmmo;
        public AudioClip reloadSoundAmmoLeft;
        public AudioClip aimSound;
    }
    public soundClips SoundClips;
    //

    private void Awake()
    {
        //アニメーターコンポーネントを取得
        anim = GetComponent<Animator>();
        //現在の弾数を取得
        //
    }

    // Start is called before the first frame update
    void Start()
    {
        //武器のゆれの初期位置
        initialSwayPoint = transform.localPosition;
        //audio set
        shootAudioSource.clip = SoundClips.shootSound;
    }

    // Update is called once per frame
    void Update()
    {
        //ADSと通常時の視野角の切替
        if (Input.GetButton("Fire2") && !isReloading && !isInspecting)
        {
            isAiming = true;
            //アニメーションをエイムに遷移
            anim.SetBool("Aim", true);
            //fovをエイム状態に
            gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView,
                aimFov, fovSpeed * Time.deltaTime);
        }
        else
        {
            //fobをデフォルトに
            gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView,
                defaultFov, fovSpeed * Time.deltaTime);
            //アニメーションを通常に遷移
            anim.SetBool("Aim", false);

            isAiming = false;
        }

        //currentAmmoText.text = currentAmmo.ToString();

        //アニメーションチェック
        AnimationCheck();

        //play knife attack
        if(Input.GetKeyDown(KeyCode.Q) && !isInspecting)
        {
            anim.Play("Knife Attack 1", 0, 0f);
        }
        if(Input.GetKeyDown(KeyCode.F) && !isInspecting)
        {
            anim.Play("Knife Attack 2", 0, 0f);
        }

        //グレネードモーション
        if(Input.GetKey(KeyCode.G) && !isInspecting)
        {
            StartCoroutine(GrenadeSpawnDelay());
            anim.Play("GrenadeThrow", 0, 0.0f);
        }

        //out of ammo
        if(currentAmmo == 0)
        {
            outOfAmmo = true;
        }
        else
        {
            outOfAmmo = false;
        }

        //左クリックで射撃
        if(Input.GetMouseButton(0) /*&& !outOfAmmo && !isReloading && !isInspecting*/)
        {
            Debug.Log(fireRate);
            if(Time.time - lastFired > 1f / fireRate)
            {
                Debug.Log(Time.time);
                lastFired = Time.time;
                //弾消費
                currentAmmo -= 1;
                //銃声
                shootAudioSource.clip = SoundClips.shootSound;
                shootAudioSource.Play();
                if (!isAiming)
                {
                    anim.Play("Fire", 0, 0.0f);
                    muzzleParticles.Emit(1);
                    //corutine
                    StartCoroutine(MuzzleFlashLight());
                }
            }
        }

    }

    private void LateUpdate()
    {
        //振り向き時の武器のブレ
        if(weaponSway == true)
        {
            float movementX = -Input.GetAxis("Mouse X") * swayAmount;
            float movementY = -Input.GetAxis("Mouse Y") * swayAmount;
            //ブレの上限設定
            movementX = Mathf.Clamp(movementX, -maxSwayAmount, maxSwayAmount);
            movementY = Mathf.Clamp(movementY, -maxSwayAmount, maxSwayAmount);
            //ブレのターゲット
            Vector3 finalSwayPosition = new Vector3(movementX, movementY, 0);
            //lerpで中心からブレを作る
            transform.localPosition = Vector3.Lerp
                (transform.localPosition, finalSwayPosition + initialSwayPoint,
                Time.deltaTime * swaySmoothValue);
        }
    }

    //現在のアニメーションをチェック
    private void AnimationCheck()
    {
        //check reloading
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Reload Out Of Ammo")||
           anim.GetCurrentAnimatorStateInfo(0).IsName("Reload Ammo Left"))
        {
            isReloading = true;
        }
        else
        {
            isReloading = false;
        }

        //check inspecting
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Inspect"))
        {
            isInspecting = true;
        }
        else
        {
            isInspecting = false;
        }
    }

    //グレネードのディレイ
    private IEnumerator GrenadeSpawnDelay()
    {
        //wait set seconds
        yield return new WaitForSeconds(grenadeSpawnDelay);
        //Spawn grenade spawnpoint
        Instantiate(Prefabs.grenadePrefab,
            Spawnpoints.grenadeSpawnPoint.transform.position,
            Spawnpoints.grenadeSpawnPoint.transform.rotation);
    }

    //muzleflashlight corutine
    private IEnumerator MuzzleFlashLight()
    {
        muzzleflashLight.enabled = true;
        yield return new WaitForSeconds(lightDuration);
        muzzleflashLight.enabled = false;
    }
}
