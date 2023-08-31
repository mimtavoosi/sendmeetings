var loan = document.getElementById("ckemtiaz");
var block = document.getElementById("ckblock");
var opfields = document.getElementsByClassName("optionalfield");
var opfields2 = document.getElementsByClassName("optionalfield2");
var rules = document.getElementById("flexCheckChecked");
var send = document.getElementById("send");
function ChangeNeededs() {
    if (loan.checked) {
        for (var i = 0; i < opfields.length; i++) {
            var x = opfields[i];
            if (x.hidden == true) {
                x.hidden = false;
                $("#AccountOwnerName").val("0000");
                $("#reqscore").val("0");
            }
        }
    }
    else {
        for (var i = 0; i < opfields.length; i++) {
            var x = opfields[i];
            if (x.hidden == false) {
                x.hidden = true;
                $("#showAccountOwnerName").val("");
                $("#reqscore").val("");
            }
        }
    }
    if (block.checked) {
        for (var i = 0; i < opfields2.length; i++) {
            var x = opfields2[i];
            if (x.hidden == true) {
                x.hidden = false;
                $("#AccountOwnerName2").val("0000");
                $("#blockreqscore").val("0");
            }
        }
    }
    else {
        for (var i = 0; i < opfields2.length; i++) {
            var x = opfields2[i];
            if (x.hidden == false) {
                x.hidden = true;
                $("#showAccountOwnerName2").val("");
                $("#blockreqscore").val("");
            }
        }
    }
    return false;
}
function EnableSend() {
    if (rules.checked) {
        send.disabled = false;
    }
    else {
        send.disabled = true;
    }
    return false;
}
loan.addEventListener('click', () => {
    ChangeNeededs();
});
block.addEventListener('click', () => {
    ChangeNeededs();
});
rules.addEventListener('click', () => {
    EnableSend();
});