var track = document.getElementById("track");
var send = document.getElementById("send");
function changeHide(x) { 
    var upfiles = document.getElementById("upbill");
    var rivision = document.getElementById("rivisionreq");
    if (x == 1) {
        upfiles.hidden = false;
        rivision.hidden = false;
    }
    else if (x == 2) {
        upfiles.hidden = true;
        rivision.hidden = false;
    }
    else {
        rivision.hidden = true;
        upfiles.hidden = true;
    }
}
function trackingCode() {
    var data = "";
    var code = $("#code").val();
    if (code.length < 4) {
        changeHide(0);
        $('#stateboard').html("");
        data = "<br /><button type=\"button\" class=\"btn btn-danger btn-lg btn-block\">کد پیگیری وارد شده نامعتبر است.</button> ";
        $('#stateboard').append(data);
    }
    else {
        $.ajax({
            type: 'Post',
            url: '/Home/TrackCode?code=' + parseInt(code),
            success: function (state) {
                $('#stateboard').html("");
                if (state.status == "نامعتبر") {
                    changeHide(0);
                    data = "<br /><button type=\"button\" class=\"btn btn-danger btn-lg btn-block\">کد پیگیری وارد شده نامعتبر است.</button> ";
                }
                if (state.status.includes("در حال بررسی")) {
                    changeHide(0);
                    data = "<br /><button type=\"button\" class=\"btn btn-warning btn-lg btn-block\">درخواست شما در حال بررسی است.</button> ";
                }
                if (state.status.includes( "رد شده")) {
                    changeHide(2);
                    data = "<br /><button type=\"button\" class=\"btn btn-danger btn-lg btn-block\">درخواست شما رد شده است.<br/> دلیل: <br/>" + state.response + " </button> ";
                }
                if (state.status.includes("تایید شده")) {
                    changeHide(1);
                    data = "<br /><button type=\"button\" class=\"btn btn-Success btn-lg btn-block\">درخواست شما تایید شده است لطفا تصاویر صورت حساب را وارد کنید.<br/> پاسخ: <br/>" + state.response + " </button><br />";
                }
                $('#stateboard').append(data);
                $('#xfiles').on("change", function () {
                    $('#send').prop('disabled', !$(this).val());
                });
            },
            error: function (ex) {
                alert('Failed to retrieve name.' + ex);
            }
        });
    }
}
track.addEventListener('click', () => {
    trackingCode();
});