var QuestionId = 2;
layui.use('form', 'layer', function () {
    var form = layui.form()
    var layer = layui.layer;
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
        , title: function (value, item) {
            if (!/^[\S]{6,20}$/.test(value)) {

                return '标题必须6到16位，且不能出现空格'
            }

        }
    });

    //监听提交
    form.on('submit(demo1)', function (data) {
        layer.alert(JSON.stringify(data.field), {
            title: '最终的提交信息'
        })
        return false;
    });

});
function AddQuestion() {
    if (QuestionId > 50) {
        layer.msg('最多50个问题');
        return;
    }
    var oDiv = document.createElement('div');
    oDiv.className = "ques"
    oDiv.id = "question"+QuestionId
    oDiv.innerHTML =
        '<div class="layui-form-item iQuestion">' +
            '<label class="layui-form-label iLabel">问题' + QuestionId + '：</label>' +
           ' <div class="layui-input-block">' +
              '  <input type="text" name="questionnaire.Questions[' + (QuestionId - 1) + '].QuestionName" required lay-verify="required" placeholder="请输入问题" autocomplete="off" class="layui-input input">' +
           ' </div>' +
         '</div>' +
        '<div class="layui-form-item iQuestion">' +
            '<label class="layui-form-label iLabel">问题类型</label>' +
           ' <div class="layui-input-block">' +
            '    <input type="radio" name="questionnaire.Questions[' + (QuestionId - 1) + '].Type" value="单选" title="单选"checked>' +
            '    <input type="radio" name="questionnaire.Questions[' + (QuestionId - 1) + '].Type" value="多选" title="多选" >' +
           ' </div>' +
        ' </div>' +
        '<div id="ques' + QuestionId + '">' +
            '<div class="layui-form-item iAnswer">' +
                    '<label class="layui-form-label iLabel">选项1：</label>' +
                   ' <div class="layui-input-block">' +
                      '  <input type="text" name="questionnaire.Questions[' + (QuestionId - 1) + '].Options[0].OptionName" required lay-verify="required" placeholder="请输入答案" autocomplete="off" class="layui-input answer">' +
                   ' </div>' +
             '</div>' +
             '<div class="layui-form-item iAnswer">' +
                    '<label class="layui-form-label iLabel">选项2：</label>' +
                   ' <div class="layui-input-block">' +
                      '  <input type="text" name="questionnaire.Questions[' + (QuestionId - 1) + '].Options[1].OptionName" required lay-verify="required" placeholder="请输入答案" autocomplete="off" class="layui-input answer">' +
                   ' </div>' +
             '</div>' +
         '</div>' +

             '<button class="layui-btn" id="addAnswerBt" value="' + QuestionId + '"style="display:inline-block" onclick="AddAnswer(value)">' +
                  '<i class="layui-icon">&#xe608;</i> 添加' +
             '</button>' +
             '<button class="layui-btn" id="addAnswerBt" value="' + QuestionId + '"style="display:inline-block" onclick="RemoveAnswer(value)">' +
                  '<i class="layui-icon">&#xe640;</i> 删除' +
             '</button>' ;

    document.getElementById('Question').appendChild(oDiv);
    QuestionId++;
    layui.use('form', function () {
        var form = layui.form()
        form.render('radio');
    });
}
function AddAnswer(value) {
    var oDiv = document.createElement('div');
    oDiv.className = 'layui-form-item iAnswer'
    var oDivId = 'ques' + value;
    var AnswerId = document.getElementById(oDivId).childNodes.length + 1
    oDiv.id = 'ques' + value+'answer' + AnswerId
    if (AnswerId > 20) {
        layer.msg('最多20个答案');
        return;
    }
    oDiv.innerHTML =
                    '<label class="layui-form-label iLabel">选项' + AnswerId + '：</label>' +
                   ' <div class="layui-input-block">' +
                      '  <input type="text" name="questionnaire.Questions[' + (value - 1) + '].Options[' + (AnswerId - 1) + '].OptionName" required lay-verify="required" placeholder="请输入答案" autocomplete="off" class="layui-input answer">' +
                   ' </div>';
    document.getElementById(oDivId).appendChild(oDiv);
    AnswerId++;

}
function RemoveQuestion() {
    if (QuestionId - 1 <= 1) {
        layer.msg('最少1个问题');
        return;
    }
    var quesId = "question" + (QuestionId - 1);
    var ques = document.getElementById(quesId)
    document.getElementById('Question').removeChild(ques)
    QuestionId--;
}
function RemoveAnswer(value) {
    var oDivId = 'ques' + value;
    var AnswerId = document.getElementById(oDivId).childNodes.length
    var answerId = 'ques' + value + 'answer' + AnswerId
    if (AnswerId  <= 2) {
        layer.msg('最少2个选项');
        return;
    }
    var answer = document.getElementById(answerId)
    document.getElementById(oDivId).removeChild(answer)
}