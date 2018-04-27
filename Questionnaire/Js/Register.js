
layui.use('form', function () {
    var form = layui.form()
    //自定义验证规则
    form.verify({
        account: function (value, item) { //value：表单的值、item：表单的DOM对象
            if (!/^[\S]{6,16}$/.test(value)) {
                return '账号必须6到16位，且不能出现空格'
            }
            if (!(/^\d+\d+\d$/.test(value))) {
                return '账号必须全为数字';
            }
        },
        username: function (value, item) { //value：表单的值、item：表单的DOM对象
            if (!new RegExp("^[a-zA-Z0-9_\u4e00-\u9fa5\\s·]+$").test(value)) {
                return '用户名不能有特殊字符';
            }
            if (/(^\_)|(\__)|(\_+$)/.test(value)) {
                return '用户名首尾不能出现下划线\'_\'';
            }
            if (/^\d+\d+\d$/.test(value)) {
                return '用户名不能全为数字';
            }
        }
        , password: function (value, item) {
            if (!/^[\S]{6,16}$/.test(value)) {
                return '密码必须6到16位，且不能出现空格'
            }

        }
        , checkPassword: function (value, item) {
            if ($('#pass').val() != value) {

                return '两次输入不一致'
            }

        }
    });
    //监听提交
 


});
