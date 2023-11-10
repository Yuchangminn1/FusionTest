using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using Firebase.Database;
using System;

public class NewFire : MonoBehaviour
{
    string defual = "https://chanfps-3ef6c-default-rtdb.firebaseio.com/";

    FirebaseAuth auth;
    FirebaseUser user;

    public InputField nickNameField;

    public InputField emailField;
    public InputField password;

    public InputField levelField;
    public InputField goldField;
    public Text text;

    public string email;
    public string nickName;
    public string level;
    public string gold;

    public string QQ = "gold";
    public string QW = "gold";

    public class Data
    {
        public string ID;
        public string level;
        public string gold;

        public Data(string level, string gold, string iD)
        {
            this.level = level;
            this.gold = gold;
            this.ID = iD;
        }
    }

    private DatabaseReference databaseReference;



    // Start is called before the first frame update
    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        LogOut();
    }

    public void Create()
    {
        string _nickName = nickNameField.text.Trim();
        if (_nickName == "")
        {
            text.text = ($"닉네임을 입력하세요 ");
            return;
        }
        auth.CreateUserWithEmailAndPasswordAsync(emailField.text, password.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                text.text = ("회원가입 취소");
                return;
            }
            if (task.IsFaulted)
            {
                //회원가입 실패 이유
                text.text = ("회원가입 실패");
                return;
            }
            FirebaseUser newUser = task.Result.User;
            email = emailField.text.Trim();
            nickName = nickNameField.text.Trim();
            text.text = "닉네임 = " + nickName;
            Debug.Log("회원가입 완료");

        }
        );
    }

    public void Login()
    {
        auth.SignInWithEmailAndPasswordAsync(emailField.text, password.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                text.text = ("로그인 취소");
                return;
            }
            if (task.IsFaulted)
            {
                //회원가입 실패 이유
                text.text = ("로그인 실패");
                return;
            }
            FirebaseUser newUser = task.Result.User;

            text.text = ("로그인 완료");

        }
        );
    }

    public void LogOut()
    {
        auth.SignOut();
        text.text = ("로그아웃");
    }


    public void OnClickSaveButton()
    {
        if (auth.CurrentUser == null)
        {
            text.text = ("로그인 필요");
            return;
        }
        email = emailField.text.Trim();
        level = levelField.text.Trim();
        gold = goldField.text.Trim();

        var data = new Data(level, gold,email);
        string jsonData = JsonUtility.ToJson(data);

        databaseReference.Child(nickName).SetRawJsonValueAsync(jsonData);

        text.text = "저장";
    }

    public void OnClickLoadButton()
    {
        if (auth.CurrentUser == null)
        {
            text.text = ("로그인 필요");
            return;
        }
        // email = emailField.text.Trim();

        databaseReference.Child(nickName).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                text.text = "로드 취소";
            }
            else if (task.IsFaulted)
            {
                text.text = "로드 실패";
            }
            else
            {
                var dataSnapshot = task.Result;

                string dataString = "";
                foreach (var data in dataSnapshot.Children)
                {
                    dataString += data.Key + " " + data.Value + "\n";
                }

                text.text = dataString;
            }
        });
    }
    //public void OnClickSaveButton()
    //{
    //    if (auth.CurrentUser == null)
    //    {
    //        text.text = ("로그인 필요");
    //        return;
    //    }
    //    email = emailField.text.Trim();
    //    nickName = nickNameField.text.Trim();
    //    level = levelField.text.Trim();
    //    gold = goldField.text.Trim();

    //    var data = new Data(level, gold);
    //    string jsonData = JsonUtility.ToJson(data);

    //    databaseReference.Child(email).SetRawJsonValueAsync(jsonData);
    //    text.text = "저장";
    //}

    //public void OnClickLoadButton()
    //{
    //    if (auth.CurrentUser == null)
    //    {
    //        text.text = ("로그인 필요");
    //        return;
    //    }
    //    email = emailField.text.Trim();
    //    nickName = nickNameField.text.Trim();
    //    databaseReference.Child(email).GetValueAsync().ContinueWith(task =>
    //    {
    //        if (task.IsCanceled)
    //        {
    //            text.text = "로드 취소";
    //            return;
    //        }
    //        else if (task.IsFaulted)
    //        {
    //            text.text = "로드 실패";
    //            return;

    //        }
    //        else
    //        {
    //            text.text = ("로드 진행중");
    //            var dataSnapshot = task.Result;


    //            string dataString = "";

    //            foreach (var datas in dataSnapshot.Children)
    //            {
    //                if (datas.Key == QQ)
    //                {
    //                    QW = datas.Value.ToString();
    //                }
    //                dataString += datas.Key + " " + datas.Value + "\n";
    //            }

    //            text.text = "QW = " + QW;
    //        }
    //        //text.text = "QW = " + QW;
    //    });






    //}


}
