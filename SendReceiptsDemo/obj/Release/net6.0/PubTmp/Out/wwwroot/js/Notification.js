var notificationBox = document.querySelector('.notification-box');
var closeButton = document.querySelector('.close-button');
closeButton.addEventListener('click', function () {
    notificationBox.hidden = true;
});

window.addEventListener('DOMContentLoaded', function () {
    $.ajax({
        type: 'Post',
        url: '/Home/ExistEnableNotification',
        success: function (state) {
            if (state == true) {
                notificationBox.hidden = false;
            }
            else {
                notificationBox.hidden = true;
            }

        },
        error: function (ex) {
            alert('Failed to retrieve name.' + ex);
        }
    });

    });