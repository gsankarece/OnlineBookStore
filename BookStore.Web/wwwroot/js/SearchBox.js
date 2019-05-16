$(document).ready(function () {
    $("#search").on('click', function () {
        let obj = {
            searchtext: $("input[name=searchbox]").val()
        };
        let userProfile = JSON.parse(localStorage.getItem("userprofile"));

        $.ajax({
            method: "GET",
            url: 'http://localhost:5000/api/books',
            data: { searchtext: obj.searchtext },
            headers: { "Authorization": "bearer "+ userProfile.email + " " + userProfile.token },        
            success: function (response) {
                console.log(response);
                var templateHead = '<table class="table table-striped table-bordered table-hover">';
                var tableheader = '<tr><th>Title</th><th>Author</th><th>Price</th><th>Quantity</th><th>AddToCart</th></tr>';
                var template = '';
                var templateEnd = '</table>';
                $.each(response.result, function (index, value) {
                    template += '<tr><td>' + value.title + '</td><td>' + value.author + '</td><td>' + value.price + '</td><td><div><input class="form-control" type="number" value="' + value.quanity + '"/></div></td><td><div><input type="button" value="Add" class="btn btn-primary" id="dummy"></input><input type="hidden" value ="' + JSON.stringify(value) +'"></input></div></td></tr>';
                });
                $('.displayArea').html("");
                $('.displayArea').append(templateHead + tableheader + template + templateEnd);
                $('#dummy').on('click', function () {
                    let book = {
                        title: $(this).closest('tr').find('td:eq(0)').text(),
                        author: $(this).closest('tr').find('td:eq(1)').text(),
                        price: $(this).closest('tr').find('td:eq(2)').text(),
                        quantity: '' 
                    }
                    $(this).closest('tr').find('td').each(function () {
                        if ($(this).attr("class") == "form-control") book.quanity = $(this).val();
                    })
                   
                    var cartheader = '<tr><th>Title</th><th>Author</th><th>Price</th><th>Quantity</th></tr>';
                    var order = '<tr><td>' + book.title + '</td><td>' + book.author + '</td><td>' + book.price + '</td><td>' + book.quantity + '</tr>';
                    $('#orderdetails').html("");
                    $('#orderdetails').append(cartheader + order);
                    console.log(book);
                });
            },
            failure: function (xhr) {
                console.log(xhr);
            }
        });

       
    });

    
});
