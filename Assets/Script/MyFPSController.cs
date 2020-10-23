using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rigidbody CapsuleCollider AudioSorce component
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(AudioSource))]

public class MyFPSController : MonoBehaviour
{
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
    float mouseX
    {
        get { return Input.GetAxis("Mouse X"); }
    }
    float mouseY
    {
        get { return Input.GetAxis("Mouse Y"); }
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
    float mouseSensitivity = 30f;


    private Transform AssignCharactersCamera()　//むし
    {
        var t = transform;
        arms.SetPositionAndRotation(t.position, t.rotation);
        return arms;
    }

    //MovementSetting LookSetting

    // Start is called before the first frame update
    void Start()
    {
        /*arms = AssignCharactersCamera();*/
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //arms.position = transform.position + transform.TransformVector(armPosition); 質問削除
        //手のposition
        //ジャンプメソッド
        Jump();
        //サウンド
    }

    private void FixedUpdate()
    {
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
        //カメラのrotationを更新
        Vector3 worldUp = /*arms*/transform.InverseTransformDirection(Vector3.up);
        var rotation = /*arms*/transform.rotation *
            Quaternion.AngleAxis(mouseX * mouseSensitivity, worldUp) *
            Quaternion.AngleAxis(-1 * mouseY * mouseSensitivity,Vector3.right);
        transform.eulerAngles = new Vector3(0, rotation.eulerAngles.y, 0);
        /*arms*/transform.rotation = rotation;
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
        Vector3 rbVelocity = _rigidbody.velocity;
        Vector3 force = new Vector3(velocity.x - rbVelocity.x, 0f, velocity.z - rbVelocity.z);
        _rigidbody.AddForce(force, ForceMode.VelocityChange);
    }

    //Inputボタン設定

    //OnCollisionStayでsphereRayCast コライダーとの衝突判定を調べる

    //視点角度上限設定

    //垂直視点の規制

    //
}
