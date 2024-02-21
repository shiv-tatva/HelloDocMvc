

    console.log("script start load")

    $('#home-tab').click(function () {
        let action = "/adminDashboard" + "/" + "LoadPartialDashboard"
    $.ajax({
        url: action,
    type: 'GET',
    data: {req = 1},
    success: function (result) {
        $('#content').html(result);
            },
    error: function () {
        alert('Error loading partial view');
            }
        });
    });
    function onclickcheck() {
        alert("clicked button")
    }
    // function loaddash(){
        //     alert("called")
        //     $.ajax({
        //     url: @Url.Action("",""),
        //         type: 'get',

        //         data: { req: 1 },

        //         success: function (result) {
        //             $("#content").html = result
        //         }



        //     })
        // }

        console.log("script end load")
