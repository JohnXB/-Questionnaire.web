layui.use('layer', function () {
    var layer = layui.layer;
});

function Login_click() {
    layer.open({
        title: "登录"
        , type: 1
        , content: $('#loginModel')
        , shade: 0.3
        , btnAlign: 'c' //按钮居中
        , move: false
    });
}

function Login_Ajax() {
    var account = document.getElementById("account").value;
    var password = document.getElementById("password").value;
   
    $.ajax({
        url: "/Home/Login",
        type: "POST",
        data: { "account": account, "password": password },
        async: false,
        dataType: "Json",
        beforeSend: function () {
            layer.load(2); //
        },
        complete: function () {
            layer.closeAll('loading')
        },
        error: function (xhr) {
            layer.msg('用户名或密码错误，请重新登录')
        },
        success: function (data) {
            var obj = JSON.parse(data);
            Login(obj)
            layer.closeAll('page')
        }

    })
}

function Logout() {
    $.ajax({
        url: "/Home/Logout",
        type: "POST",
        data: {},
        async: false,
        dataType: "Json",
        beforeSend: function () {
          var tip=layer.open({
                type: 3
                , shade: 0.3
                , move: false
            });
            
        },
        complete: function () {
            layer.closeAll()
        },
        error: function (xhr) {
            layer.msg('注销失败')
        },

        success: function (data) {
           window.location.href = "/Home/Index"
        }

    })
}

function Login(obj) {
    document.getElementById('login').innerHTML = '';
    var aDiv = document.createElement('span');

    aDiv.innerHTML = '<img src="'+obj.UserImg+'" id="UserImg">' +
                    '<a>' + obj.UserName + '</a>' +
                    '<a>,你好</a>' +
                '<a href="#" onclick="Logout()" id="logout" >退出</a>';
    document.getElementById('login').appendChild(aDiv);
    
}

function cancel() {
    
    layer.closeAll('page');
}



