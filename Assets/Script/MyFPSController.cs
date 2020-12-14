using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

//Rigidbody CapsuleCollider AudioSorce component
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(AudioSource))]

public class MyFPSController : NetworkBehaviour
{
    public Image _Life;
    float LifeGuage;

    GameObject _AnimalGeneratorObj;
    AnimalGenerator AnimalGeneratorSc;

#pragma warning disable 649
    [Header("Arms")]
    [Tooltip("The transform component that holds the gun camera."), SerializeField]
#pragma warning restore 649
    Transform arms;
    Vector3 armPosition;
    Rigidbody _rigidbody;
    bool is_Grounded = false;
    float jumpForce = 30f;
    float runSpeed = 5f;
    float minVerticalAngle = -90f;
    float maxVerticalAngle = 90f;
    SmoothRotation _rotateX;
    SmoothRotation _rotateY;
    SmoothVelocity _velocityX;
    SmoothVelocity _velocityZ;

    float rotationSmoothness = 0.05f;
    float movementSmoothness = 0.125f;

    public Image _Image;

    float pushForce = 50f;

    float mouseX
    {
        get { return Input.GetAxisRaw("Mouse X") * mouseSensitivity; }
    }
    float mouseY
    {
        get { return Input.GetAxisRaw("Mouse Y") * mouseSensitivity; }
    }
    float hMove
    {
        get { return Input.GetAxis("Horizontal"); }
    }
    float vMove
    {
        get { return Input.GetAxis("Vertical"); }
    }
    bool jump
    {
        get { return Input.GetButtonDown("Jump"); }
    }
    float mouseSensitivity = 15f;


    private Transform AssignCharactersCamera()　
    {
        var t = transform;
        arms.SetPositionAndRotation(t.position, t.rotation);
        return arms;
    }

    //MovementSetting LookSetting

    // Start is called before the first frame update
    void Start()
    {
        //アニマルに位置を同期
        _AnimalGeneratorObj = GameObject.Find("AnimalGeneratorObj");
        _AnimalGeneratorObj.GetComponent<AnimalGenerator>().playerObj = this.gameObject;

        //armのポジションとローテーションをthisと同期？
        arms = AssignCharactersCamera();
        //スタート時のマウス移動格納
        _rotateX = new SmoothRotation(mouseX);
        _rotateY = new SmoothRotation(mouseY);

        _velocityX = new SmoothVelocity();
        _velocityZ = new SmoothVelocity();

        _rigidbody = GetComponent<Rigidbody>();

        LifeGuage = _Life.fillAmount;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;

        //armのポジション
        arms.position = transform.position + transform.TransformVector(armPosition);
        
        //サウンド
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) return;

        //カメラとキャラクターの向き
        RotationCameraAndCharacter();
        //キャラクターの動き
        MoveCharacter();
        is_Grounded = true;
    }

    //ジャンプメソッド
    void Jump()
    {
        if (!is_Grounded || !jump) return;
        _rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        is_Grounded = false;
    }

    //キャラクターとカメラのrotationメソッド
    void RotationCameraAndCharacter()
    {
        //SmoothUpdataメソッドでマウス移動調整
        var rotateX = _rotateX.SmoothUpdate(mouseX, rotationSmoothness);
        var rotateY = _rotateY.SmoothUpdate(mouseY, rotationSmoothness);
        //Y方向入力のみ限界値指定
        var clampedY = RestrictAngle(rotateY);
        //カメラのrotationを更新
        Vector3 worldUp = arms.InverseTransformDirection(Vector3.up);
        var rotation = arms.rotation *
            Quaternion.AngleAxis(rotateX, worldUp) *
            Quaternion.AngleAxis(clampedY, Vector3.left);
        transform.eulerAngles = new Vector3(0, rotation.eulerAngles.y, 0);
        arms.rotation = rotation;
    }

    //垂直方向の視点移動を規制
    float RestrictAngle(float rotateY)
    {
        //armsのアングルを90~-90に変換
        var currentAngle = NormalizeAngle(arms.eulerAngles.x);
        //垂直視点で０になるように計算
        var minY = minVerticalAngle + currentAngle;
        var maxY = maxVerticalAngle + currentAngle;
        //垂直視点より少し斜めで規制
        return Mathf.Clamp(rotateY, minY + 0.02f, maxY - 0.02f);
    }

    //オイラーアングルを９０〜ー９０に変換
    float NormalizeAngle(float angle)
    {
        while (angle > 180)
        {
            angle -= 360;
        }
        while (angle <= -180)
        {
            angle += 360;
        }

        return angle;
    }

    //キャラクターの移動メソッド
    void MoveCharacter()
    {
        //実行毎に初期化
        Vector3 horizonMove = Vector3.zero;

        
        //キャラクターの移動
        horizonMove = new Vector3(hMove, 0f, vMove).normalized;
        Vector3 worldDirection = transform.TransformDirection(horizonMove);
        Vector3 velocity = worldDirection * runSpeed;

        var velocityX = _velocityX.Update(velocity.x, movementSmoothness);
        var velocityZ = _velocityZ.Update(velocity.z, movementSmoothness);

        Vector3 rbVelocity = _rigidbody.velocity;
        Vector3 force = new Vector3(velocityX - rbVelocity.x, 0f, velocityZ - rbVelocity.z);
        _rigidbody.AddForce(force, ForceMode.VelocityChange);
    }

    //SmoothRotationクラス
    private class SmoothRotation
    {
        private float _current;
        private float _currentVelocity;

        public SmoothRotation(float startAngle)
        {
            _current = startAngle;
        }

        public float SmoothUpdate(float target, float smoothTime)
        {
            return _current = Mathf.SmoothDampAngle(_current, target, ref _currentVelocity, smoothTime);
        }
    }

    //SmoothVelocityクラス
    private class SmoothVelocity
    {
        private float _current;
        private float _currentVelocity;

        public float Update(float target,float smoothTime)
        {
            return _current = Mathf.SmoothDamp(_current, target, ref _currentVelocity, smoothTime);
        }
    }

    //敵との衝突　tag
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Animal")
        {
            Debug.Log(collision.gameObject.tag + "と衝突");
            //画面を赤点
            _Image.GetComponent<DamageScreen>().isDamaging = true;
            //Animalと衝突時ノックバック
            var pushVec = collision.contacts[0].normal.normalized;
            var push = pushVec * pushForce;
            var localPushVec = transform.InverseTransformDirection(push);

            _rigidbody.AddForce(localPushVec, ForceMode.VelocityChange);

            //Life減少
            _Life.fillAmount -= 0.1f;
        }
    }

    //Inputボタン設定

    //OnCollisionStayでsphereRayCast コライダーとの衝突判定を調べる

    //視点角度上限設定

    //垂直視点の規制

    //
}
