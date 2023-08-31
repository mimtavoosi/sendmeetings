var righterLbl = document.getElementById('showRighterName');
var righteOwnerLbl = document.getElementById('showRightOwnerName');
var accountOwnerLbl = document.getElementById('showAccountOwnerName');
var accountOwnerLbl2 = document.getElementById('showAccountOwnerName2');
var mobLbl = document.getElementById('showMobileNumber');
function ShowRighterName(input) {
    if (input.value.length < 5) {
        righterLbl.innerText = "";
    }
    else {
        $.ajax({
            type: 'Post',
            url: '/Admin/GetCustomerNameByCustomerId', // we are calling json method
            //dataType: 'json',
            data: { customerId: $("#RighterNationalCode").val() },
            success: function (state) {
                // states contains the JSON formatted list
                // of states passed from the controller
                if (state != undefined) {
                    righterLbl.innerText = state;
                }

            },
            error: function (ex) {
                alert('Failed to retrieve name.' + ex);
            }
        });
    }
}
function ShowRightOwnerName(input) {
    if (input.value.length < 4) {
        righteOwnerLbl.innerText = "";
    }
    else {
        $.ajax({
            type: 'POST',
            url: '/Admin/GetCustomerNameByAccountNumber', // we are calling json method
            //dataType: 'json',
            data: { accountNumber: $("#RightOwnerNationalCode").val() },
            success: function (state) {
              
                // states contains the JSON formatted list
                // of states passed from the controller
                if (state != undefined) {
                    righteOwnerLbl.innerText = state;
                }

            },
            error: function (ex) {
                alert('Failed to retrieve states.' + ex);
            }
        });
    }
}

function ShowAccountOwnerName(input) {
    if (input.value.length < 4) {
        righteOwnerLbl.innerText = "";
    }
    else {
        $.ajax({
            type: 'POST',
            url: '/Home/GetCustomerNameByAccountNumber', // we are calling json method
            //dataType: 'json',
            data: { accountNumber: $("#AccountOwnerName").val() },
            success: function (state) {

                // states contains the JSON formatted list
                // of states passed from the controller
                if (state != undefined) {
                    accountOwnerLbl.innerText = state;
                }

            },
            error: function (ex) {
                alert('Failed to retrieve states.' + ex);
            }
        });
    }
}
function ShowAccountOwnerName2(input) {
    if (input.value.length < 4) {
        righteOwnerLbl.innerText = "";
    }
    else {
        $.ajax({
            type: 'POST',
            url: '/Home/GetCustomerNameByAccountNumber', // we are calling json method
            //dataType: 'json',
            data: { accountNumber: $("#AccountOwnerName2").val() },
            success: function (state) {

                // states contains the JSON formatted list
                // of states passed from the controller
                if (state != undefined) {
                    accountOwnerLbl2.innerText = state;
                }

            },
            error: function (ex) {
                alert('Failed to retrieve states.' + ex);
            }
        });
    }
}

function ShowMobileNumber(input) {
    if (input.value.length < 11) {
        //  debugger;
        mobLbl.innerText = "";
    }
    else {
        $.ajax({
            type: 'Post',
            url: '/Admin/GetCustomerNameByMobileNumber', // we are calling json method
            //dataType: 'json',
            data: { mobileNumber: $("#mobileNumber").val() },
            success: function (state) {
                // states contains the JSON formatted list
                // of states passed from the controller
                if (state != undefined) {
                    mobLbl.innerText = state;
                }

            },
            error: function (ex) {
                alert('Failed to retrieve name.' + ex);
            }
        });
    }
}