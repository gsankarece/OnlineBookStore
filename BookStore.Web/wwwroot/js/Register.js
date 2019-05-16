$(document).ready(function () {
    $("#submit").on('click', function () {
        let obj = {
            UserName: $("input[name=username]").val(),
            Email: $("input[name=email]").val(),
            Password: $("input[name=password]").val()
        };

        $.ajax({
            method: "POST",
            url: 'http://localhost:5000/api/user/register',
            data: { UserName: obj.UserName, Password: obj.Password, Email: obj.Email },
            success: function (result) {
                    window.location.href = "/Login";
                },
            failure: function (json) {
                console.log(json);
            }
        })
    });
});
