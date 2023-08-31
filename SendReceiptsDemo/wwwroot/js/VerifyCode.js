var resend = document.getElementById("resend");

//حذف دکمه و نشان دادن تایمر
function styledogme() {
    
    showBox();
    hideDogme();
    var timer = 120, minutes, seconds;
    startTimer(timer);

}

function hideBox() {
    const box3 = document.getElementById('time');
    box3.style.display = 'none';
}
function hideDogme() {
    resend.style.display = 'none';
}

function showBox() {
    const box3 = document.getElementById('time');
    box3.style.display = 'block';
    display = document.querySelector('#timeSpan');
    display.textContent = "01:00";

}
function showDogme() {
    resend.style.display = 'block';
}
//دقیقه

function startTimer(timer) {
    display = document.querySelector('#timeSpan');
     var ptimer = timer;
     var timerOn = setInterval(function () {
        minutes = parseInt(ptimer / 60, 10);
        seconds = parseInt(ptimer % 60, 10);

        minutes = minutes < 10 ? "0" + minutes : minutes;
        seconds = seconds < 10 ? "0" + seconds : seconds;

        display.textContent = minutes + ":" + seconds;

        if (--ptimer < 0) {
            showDogme();
            hideBox();
            clearInterval(timerOn);
        }
    }, 1000);
    return false;
}

function ReCode() {
    $.ajax({
        type: 'Post',
        url: '/Home/ReSendCode', // we are calling json method
        //dataType: 'json',
       /* data: { mobileNumber: $("#mobileNumber").val() },*/
        success: function (state) {
            // states contains the JSON formatted list
            // of states passed from the controller
            //if (state != undefined) {
            //    mobLbl.innerText = state;
            //}

        },
        error: function (ex) {
            alert('Failed to retrieve name.' + ex);
        }
    });
}

document.addEventListener('DOMContentLoaded', function () {
    var timer = 120, minutes, seconds;
    startTimer(timer);
}, false);
resend.addEventListener('click', () => {
    ReCode();
});
