function fixAccountNumber(account) {
    if (account != null && account.accountNumber != null)
        return account.accountNumber;
    else return "";
}
$(document).ready(function () {
        $('#searchtext').on('keyup', function () {
            var searchtext = $('#searchtext').val();
            var searchtype = $('#searchtype').val();
                $.ajax({
                    type: 'post',
                    url: '/Admin/SearchRecords?searchtext=' + searchtext + '&searchtype=' + searchtype,
                    success: function (result) {
                        $('#tbdata').html("");

                        if (result.length == 0) {
                            $('#tbdata').append("<tr><td colspan='4'> شخصی یافت نشد</td></tr>");
                        }
                        else {
                            //debugger;
                            $.each(result, function (index, value) {
                                if (searchtype == '1') {
                                    var data = "<tr><td>" + value.customerId + "</td>" +
                                        "<td>" + value.nationalCode + "</td>" +
                                        "<td>" + value.mobileNumber + "</td>" +
                                        "<td>" + value.fullName + "</td>" +
                                        "<td>" + value.fatherName + "</td>" +
                                        "<td>" + value.varizId + "</td>" +
                                        "<td>" + value.customerDescription + "</td>" +
                                        "<td><a type=\"button\" class =\"btn btn-success btn-rounded w-md waves-effect waves-light m-b-5\" href =\"/Admin/AddMessage/" + value.customerId + "\"> ارسال پیامک</a></td>" +
                                        "<td><a type=\"button\" class =\"btn btn-warning btn-rounded w-md waves-effect waves-light m-b-5\" href =\"/Admin/EditCustomer/" + value.customerId + "\"> ویرایش</a></td>" +
                                        "<td><a type=\"button\" class =\"btn btn-danger btn-rounded w-md waves-effect waves-light m-b-5\" href =\"/Admin/DeleteCustomer/" + value.customerId + "\"> حذف</a></td></tr>";
                                }
                                if (searchtype == '2') {
                                    var data = "<tr><td>" + value.accountNumber + "</td>" +
                                        "<td>" + value.score + "</td>" +
                                        "<td>" + value.customer.fullName + "</td>" +
                                        "<td><a type=\"button\" class =\"btn btn-warning btn-rounded w-md waves-effect waves-light m-b-5\" href =\"/Admin/EditAccount/" + value.acountId + "\"> ویرایش</a></td>" +
                                        "<td><a type=\"button\" class =\"btn btn-danger btn-rounded w-md waves-effect waves-light m-b-5\" href =\"/Admin/DeleteAccount/" + value.acountId + "\"> حذف</a></td></tr>";
                                }
                                if (searchtype == '4') {
                                    var data = "<tr><td>" + value.accountNumber + "</td>" +
                                        "<td>" + value.righterName + "</td>" +
                                        "<td>" + value.rightOwnerName + "</td>" +
                                        "<td><a type=\"button\" class =\"btn btn-danger btn-rounded w-md waves-effect waves-light m-b-5\" href =\"/Admin/DeleteRight/" + value.rightId + "\"> حذف</a></td></tr>";
                                }
                                if (searchtype == '5') {
                                    var data = "<tr><td>" + value.userName + "</td>" +
                                        "<td>" + value.adminType + "</td>" +
                                        "<td><a type=\"button\" class =\"btn btn-warning btn-rounded w-md waves-effect waves-light m-b-5\" href =\"/Admin/EditAdmin/" + value.adminId + "\"> ویرایش</a></td>" +
                                        "<td><a type=\"button\" class =\"btn btn-danger btn-rounded w-md waves-effect waves-light m-b-5\" href =\"/Admin/DeleteAdmin/" + value.adminId + "\"> حذف</a></td></tr>";
                                }
                                if (searchtype == '6') {
                                    var data = "<tr><td>" + value.customer.fullName + "</td>" +
                                        "<td>" + value.customer.mobileNumber + "</td>" +
                                        "<td>" + value.messageText + "</td>" +
                                        "<td>" + value.sentDate + "</td>" +
                                        "<td>" + value.sentState + "</td>" +
                                        "<td><a type=\"button\" class =\"btn btn-primary btn-rounded w-md waves-effect waves-light m-b-5\" href =\"/Admin/ReadMessage/" + value.messageId + "\"> نمایش</a></td>" +
                                        "<td><a type=\"button\" class =\"btn btn-danger btn-rounded w-md waves-effect waves-light m-b-5\" href =\"/Admin/DeleteMessage/" + value.messageId + "\"> حذف</a></td></tr>";
                                }
                                if (searchtype == '8' || searchtype == '9') {
                                    var data = "<tr><td>" + value.meetingId + "</td>" +
                                        "<td>" + value.customer.fullName + "</td>" +
                                        "<td>" + value.accountNumber + "</td>" +
                                        "<td>" + value.amount + "</td>" +
                                        "<td>" + value.count + "</td>" +
                                        "<td>" + value.meetingDate + "</td>" +
                                        "<td>" + value.description + "</td>" +
                                        "<td>" + value.status + "</td>" +
                                        "<td>" + value.response + "</td>" +
                                        "<td><a type=\"button\" class =\"btn btn-primary btn-rounded w-md waves-effect waves-light m-b-5\" href =\"/Admin/CheckMeeting/" + value.meetingId + "\"> بررسی</a></td>" +
                                        "<td><a type=\"button\" class =\"btn btn-danger btn-rounded w-md waves-effect waves-light m-b-5\" href =\"/Admin/DeleteMeeting/" + value.meetingId + "\"> حذف</a></td></tr>";
                                }
                                if (searchtype == '10') {
                                    var data = "<tr><td>" + value.notificationTitle + "</td>" +
                                        "<td>" + value.notificationDescription + "</td>" +
                                        "<td>" + value.notificationStatus + "</td>" +
                                        "<td><a type=\"button\" class =\"btn btn-warning btn-rounded w-md waves-effect waves-light m-b-5\" href =\"/Admin/EditNotification/" + value.notificationId + "\"> ویرایش</a></td>" +
                                        "<td><a type=\"button\" class =\"btn btn-danger btn-rounded w-md waves-effect waves-light m-b-5\" href =\"/Admin/DeleteNotification/" + value.notificationId + "\"> حذف</a></td></tr>";
                                }
                                $('#tbdata').append(data);
                                
                            })
                        }
                    }
                })
           

        });
    });
