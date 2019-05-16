$(document).ready(function () {
    $("#loginbutton").on('click', function () {
        let obj = {
            Email: $("input[id=Email]").val(),
            Password: $("input[id=Password]").val()
        };

        $.ajax({
            method: "POST",
            url: 'http://localhost:5000/api/user/login',
            data: { emailId: obj.Email, password: obj.Password },
            success: function (json) {                
                localStorage.setItem("userprofile", JSON.stringify(json));
                window.location.href = "/Search";
            }, 
            failure: function (error) {
                console.log(error);
            }
        })
    });
});

