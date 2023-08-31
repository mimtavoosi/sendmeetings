var accept = document.getElementById("accept");
var reject = document.getElementById("reject");
var form = document.getElementById("checkform");

function acceptReq() {
    form.setAttribute('action', '/Admin/ResponseMeeting?btn=1');
}

function rejectReq() {
    form.setAttribute('action', '/Admin/ResponseMeeting?btn=2');
}

accept.addEventListener('click', () => {
    acceptReq();
});
reject.addEventListener('click', () => {
    rejectReq();
});