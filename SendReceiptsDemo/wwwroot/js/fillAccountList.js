$(document).ready(function () {
    //Dropdownlist Selectedchange event
    $("#RighterNationalCode").change(function () {
        $("#BankAcountId").empty();
        $.ajax({
            type: 'POST',
            url: 'GetAccountsOfCustomer', // we are calling json method
            //dataType: 'json',
            data: { customerId: $("#RighterNationalCode").val() },
            success: function (states) {
                // states contains the JSON formatted list
                // of states passed from the controller

                $("#BankAcountId").append('<option disabled selected >' + "یک حساب را انتخاب کنید" + '</option>');
                $.each(states, function (i, state) {
                    $("#BankAcountId").append('<option value="' + state.bankAcountId + '">' + state.accountText + '</option>');
                    // here we are adding option for States
                });

            },
            error: function (ex) {
                alert('Failed to retrieve list.' + ex);
            }
        });
        return false;
    })
});