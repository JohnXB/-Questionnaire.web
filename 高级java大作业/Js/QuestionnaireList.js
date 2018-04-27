layui.use('form', function () {
    var form = layui.form();

});
window.onload(SerchQ('所有'));


function SerchQ(serchKey) {
    var serchMode = $("#serchByMode").val();
    layui.use(['laypage', 'layer'], function () {
        var laypage = layui.laypage
        , layer = layui.layer;
        $.ajax({
            url: "/Home/Serch",
            type: "POST",
            data: { "serchKey": serchKey, "serchMode": serchMode },
            async: false,
            dataType: "Json",
            beforeSend: function () {
                layer.load(2)
            },
            complete: function () {
                layer.closeAll('loading')
            },
            error: function (xhr) {
                document.getElementById('biuuu_city_list').innerHTML = '<span>没找到问卷</span>'
            },
            success: function (data) {
                if (data == false) {
                    document.getElementById('biuuu_city_list').innerHTML = '<span>没找到问卷</span>';
                    return;
                }
                var questionnaires = JSON.parse(data);
                var questionnaireArray = new Array(questionnaires.length);
                for (var i = 0; i < questionnaireArray.length; i++) {
                    questionnaireArray[i] = questionnaires[i];
                }
                //将一段数组分页展示

                //测试数据


                var nums = 5; //每页出现的数据量

                //模拟渲染
                var render = function (data, curr) {
                    var arr = []
                    , thisData = data.concat().splice(curr * nums - nums, nums);
                    layui.each(thisData, function (index, item) {
                        arr.push(
                            '<li style="margin:auto;text-align:center;margin-bottom:40px;">' +
                                '<div class="questionnaire">' +
                                    '<div style="margin-bottom:30px;">' +
                                      '  <a class="questionnaireTitle" href="/Home/FillIn?QId=' + item.QId + '" id="' + item.QId + '">' +
                                          '  <span class="questionnaireTitle">' + item.QName + '</span>' +
                                       ' </a>' +
                                       ' <span class="questionNum">共' + item.Questions.length+ '个问题</span>' +
                                    '</div>' +
                                    '<div>' +
                                      '  <span>作者：' + item.UserName + '|填写人数：' + item.NumOfPeople + '人</span>' +
                                    '</div>' +
                                '</div>' +
                            '</li>');

                    });
                    return arr.join('');
                };
                //调用分页
                laypage({
                    cont: 'paging'
                  , pages: Math.ceil(questionnaireArray.length / nums) //得到总页数
                  , jump: function (obj) {
                      document.getElementById('biuuu_city_list').innerHTML = render(questionnaireArray, obj.curr);
                  }
                });



            }

        })
    });

}