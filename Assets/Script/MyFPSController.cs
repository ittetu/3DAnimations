﻿using System.Collections;
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
    float minVerticalAngle = -90f;
    float maxVerticalAngle = 90f;
    SmoothRotation _rotateX;
    SmoothRotation _rotateY;
    float rotationSmoothness = 0.05f;
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
        //armのポジションとローテーションをthisと同期？
        arms = AssignCharactersCamera();
        //スタート時のマウス移動格納
        _rotateX = new SmoothRotation(mouseX);
        _rotateY = new SmoothRotation(mouseY);

        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //armのポジション
        arms.position = transform.position + transform.TransformVector(armPosition);
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
        //SmoothUpdataメソッドでマウス移動調整
        var rotateX = _rotateX.SmoothUpdata(mouseX, rotationSmoothness);
        var rotateY = _rotateY.SmoothUpdata(mouseY, rotationSmoothness);
        //Y方向入力のみ限界値指定
        var clampedY = RestrictAngle(rotateY);
        //カメラのrotationを更新
        Vector3 worldUp = arms.InverseTransformDirection(Vector3.up);
        var rotation = arms.rotation *
            Quaternion.AngleAxis(rotateX , worldUp) *
            Quaternion.AngleAxis(clampedY ,Vector3.left);
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
        Vector3 rbVelocity = _rigidbody.velocity;
        Vector3 force = new Vector3(velocity.x - rbVelocity.x, 0f, velocity.z - rbVelocity.z);
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

        public float SmoothUpdata(float target,float smoothTime)
        {
            return _current = Mathf.SmoothDampAngle(_current, target, ref _currentVelocity,smoothTime);
        }
    }

    //Inputボタン設定

    //OnCollisionStayでsphereRayCast コライダーとの衝突判定を調べる

    //視点角度上限設定

    //垂直視点の規制

    //
}
